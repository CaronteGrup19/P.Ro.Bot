from imutils.video import VideoStream
from collections import deque
import face_recognition
import numpy as np
import argparse
import imutils
import pickle
import socket
import time
import cv2

UDP_IP = "192.168.1.62"
UDP_PORT = 5000

mi_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# face recognition variables
detectionMethod = "cnn"
encodings = "encodings.pickle"

# load the know face and embeddings
data = pickle.loads(open(encodings, "rb").read())

# red ball variables
redLower0 = (0,170,50)
redUpper0 = (10,255,255)
redLower1 = (170,170,50)
redUpper1 = (180,255,255)
pts = deque(maxlen=64)

ballFound = False
faceFound = False

print("[INFO] starting video stream...")
#qvs = cv2.VideoCapture(0)
vs = VideoStream(src=0).start()
writer = None
time.sleep(2.0)

# loop over frame from the video file stream
while True:
    # grab the current frame
    frame = vs.read()
    # handle the frame from VideoCapture or VideoStream
    #frame = frame[1]
    # if we are viewing a video and we did not grab a frame,
    # then we have reached the end of the video
    if frame is None:
        break
    # resize the frame, blur it, and convert it to the HSV
    # color space
    frame = imutils.resize(frame, width=600)
    blurred = cv2.GaussianBlur(frame, (11, 11), 0)
    hsv = cv2.cvtColor(blurred, cv2.COLOR_BGR2HSV)
    # construct a mask for the color "green", then perform
    # a series of dilations and erosions to remove any small
    # blobs left in the mask
    mask0 = cv2.inRange(hsv, redLower0, redUpper0)
    mask1 = cv2.inRange(hsv, redLower1, redUpper1)
    mask = mask0 + mask1
    # mask = cv2.inRange(hsv, greenLower, greenUpper)
    mask = cv2.erode(mask, None, iterations=2)
    mask = cv2.dilate(mask, None, iterations=2)

    # find contours in the mask and initialize the current
    # (x, y) center of the ball
    cnts = cv2.findContours(mask.copy(), cv2.RETR_EXTERNAL,
                            cv2.CHAIN_APPROX_SIMPLE)
    cnts = imutils.grab_contours(cnts)
    center = None

    # only proceed if at least one contour was found
    if len(cnts) > 0:
        # find the largest contour in the mask, then use
        # it to compute the minimum enclosing circle and
        # centroid
        c = max(cnts, key=cv2.contourArea)
        ((x, y), radius) = cv2.minEnclosingCircle(c)
        M = cv2.moments(c)
        center = (int(M["m10"] / M["m00"]), int(M["m01"] / M["m00"]))
        # only proceed if the radius meets a minimum size
        if radius > 10:
            # draw the circle and centroid on the frame,
            # then update the list of tracked points
            cv2.circle(frame, (int(x), int(y)), int(radius),
                       (0, 255, 255), 2)
            cv2.circle(frame, center, 5, (0, 0, 255), -1)
    # update the points queue
        pts.appendleft(center)

        print("[INFO] a la espera pilota")

        print("[INFO] pilota detectada")
        mi_socket.sendto("pilota".encode(), (UDP_IP, UDP_PORT))
        time.sleep(0.01)

    else:
        # convert the input frame from BGR to RGB then resize it to have
        # a width of 750px (to speedup processing)
        rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        rgb = imutils.resize(rgb, width=750)
        r = frame.shape[1] / float(rgb.shape[1])

        # detect the (x, y)-coordinates of the bounding boxes
        # corresponfing to each face in the input frame, then compute
        # the facial embedding for each face
        boxes = face_recognition.face_locations(rgb, model=detectionMethod)
        encodings = face_recognition.face_encodings(rgb, boxes)
        names = []

        # loop over the facial embeddings
        for encoding in encodings:
            # attempt to match each face in the input image to our know
            # encodings
            matches = face_recognition.compare_faces(data["encodings"], encoding)
            name = "Unknow"

            # check to see if we have found a match
            if True in matches:
                # find the indexes of all matched faces then initialize a
                # dictionary to count the total number of times each face
                # was matched
                matchedIdxs = [i for (i, b) in enumerate(matches) if b]
                counts = {}

                # loop over the matched indexes and maintain a count for
                # each recognized face face
                for i in matchedIdxs:
                    name = data["names"][i]
                    counts[name] = counts.get(name, 0) + 1

                # determine the recognized face with the largest number
                # of votes (note: in the event of an unlikely tie Python
                # will select first entry in the dectionary)
                name = max(counts, key=counts.get)

            # update the list of names
            names.append(name)

        print("[INFO] a la espera de cara")

        if len(names) >= 1:
            if names[0] == "Unknow":
                print("[INFO] no match in recognized face")
                mi_socket.sendto("noconeix".encode(), (UDP_IP, UDP_PORT))
                time.sleep(0.01)
            else:
                print("[INFO] match in recognized face")
                mi_socket.sendto("coneix".encode(), (UDP_IP, UDP_PORT))
                time.sleep(0.01)
        elif len(names) < 1:
            print("[INFO] no match in recognized face")
            mi_socket.sendto("noconeix".encode(), (UDP_IP, UDP_PORT))
            time.sleep(0.01)



    ''' ---------------------- '''
    # show the frame to our screen
    cv2.imshow("Frame", frame)
    key = cv2.waitKey(1) & 0xFF
    # if the 'q' key is pressed, stop the loop
    if key == ord("q"):
        break
    # if we are not using a video file, stop the camera video stream

vs.stop()
# close all windows
cv2.destroyAllWindows()