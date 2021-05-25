Cada fitxer és d'execució individual.

recognitionRedBall.py (v. 1.0) --> detecta objectes vermells

buildFaceDataset.py (v. 1.0) --> serveix per crear un dataset amb les images propies,
                                 a partir de la webcam. Un cop iniciada l'execució i
                                 després d'apareixer el display prement la tecla 'k'
                                 es creen les imatges.
                                 PARAMETRES
                                    -c --cascade    Path del fitxer Haar cascade
                                    -o --output     Path del directori d'emmagatzematge
                                                    de les imatges. Ex. si el teu nom és
                                                    "John Smith", el directori és
                                                    dataset/john_smith

encodeFaces.py (v. 1.0) --> entrena un model de reconeixement facial.
                            Parametres
                                -i --dataset            Path del dataset
                                -e --encodings          Path del fitxer encodings.pickle
                                -d --detection-method   Metode de detecció `hog` o `cnn`
                                                        per defecte `cnn`

recognizedFaceImage.py (v. 1.0) --> reconeixment de cares a partir d'una imatge
                                    PARAMETRES
                                        -e --encodings          Path del fitxer encodings.pickle
                                        -i --image              Imatge pel reconeixement "facial"
                                        -d --detection-method   Metode de detecció `hog` o `cnn`
                                                                per defecte `cnn`

recognizedFaceVideo.py (v. 1.0) --> asd
                                    PARAMETRES
                                        -e --encodings          Path del fitxer encodings.pickle
                                        -y --display            Bandera per mostar els frame
                                                                per pantalla 1 = Si / 0 = No
                                        -d --detection-method   Metode de detecció `hog` o `cnn`
                                                                per defecte `cnn`