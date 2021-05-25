using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCua : MonoBehaviour
{
    public Vector3 cuaHappy;

    private int min = -30;
    private int max = 30;
    private float actual;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        actual += cuaHappy.y;
        if (actual > max || actual < min) cuaHappy.y = cuaHappy.y * -1;
        transform.Rotate(cuaHappy);
    }
}
