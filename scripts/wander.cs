using UnityEngine;
using System.Collections;

public class wander : MonoBehaviour
{
    public Vector3 targetPos;
    private int maxx = 250;
    private int maxz = 250;

    public bool isMoving = false;
    //public float waitTime = 3f;
    public float speed = 0.02f;

    void Update()
    {
        if (isMoving == false)
        {
            FindNewTargetPos();
        }
    }

    private void FindNewTargetPos()
    {
        Vector3 pos = transform.position;
        targetPos = new Vector3();
        targetPos.x = Random.Range(-1* maxx, maxx);
        targetPos.y = pos.y;
        targetPos.z = Random.Range(-1* maxz, maxz);

        transform.LookAt(targetPos);
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        isMoving = true;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
        {

            transform.position = Vector3.MoveTowards(transform.position, targetPos, t);
            yield return null;
        }

        //yield return new WaitForSeconds(waitTime);
        isMoving = false;
        yield return null;
    }



}
