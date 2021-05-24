using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCua2 : MonoBehaviour
{
    public int estatAnim = 0;

    private Vector3 cuaHappy;
    private Vector3 cuaAnxiety;
    private Vector3 cuaNeutral;
    private Vector3 cuaSleep;
    [SerializeField] public GameObject Cua;


    private int min = -30;
    private int max = 30;
    private int canvi = 0;
    private float actualy = 0;
    private float actualz = 0;
    private System.Random _random = new System.Random();
    private float minZcuaHappy = -30.0f;
    private float maxZcuaHappy = 10.0f;
    private float maxZcuaSleep = 30.0f;
    private float auxz;

    // Start is called before the first frame update
    void Start()
    {
        cuaHappy = new Vector3(0.0f, 0.5f, 0.0f);
        cuaAnxiety = new Vector3(0.0f, 0.06f, 0.0f);
        cuaNeutral = new Vector3(0.0f, 0.2f, 0.0f);
        cuaSleep = new Vector3(0.1f, 0.1f, 0.1f);
        Cua = GameObject.Find("PuntUnioCua");
        Neutral();
    }

    // Update is called once per frame
    void Update()
    {
        switch (estatAnim)
        {
            case 0:
                Dormit();
                break;
            case 1:
                Happy();
                break;
            case 2:
                Neutral();
                break;
            case 3:
                Fear();
                break;
            case 4:
                Aburrit();
                break;
            case 5:
                Curios();
                break;
            case 6:
                Anxiety();
                break;
        }

    }

    void Dormit()
    {
        actualz += cuaSleep.z;
        if (actualz < maxZcuaSleep)
            Cua.transform.Rotate(cuaSleep);
    }

    void Happy()
    {
        actualy += cuaHappy.y;
        canvi = 0;
        if (actualy > max || actualy < min)
        {
            cuaHappy.y = cuaHappy.y * -1;
            canvi = 1;
        }
        if (canvi == 1)
        {
            auxz = _random.Next(-100, 100);
            auxz = auxz / 1000;
            if (actualz < minZcuaHappy) auxz = 0;
            if (actualz > maxZcuaHappy) auxz = 0;
            actualz += auxz;
            cuaHappy.z = auxz;
        }
        Cua.transform.Rotate(cuaHappy);
    }
    void Neutral()
    {
        double maxSpeed = 0.1;
        double minSpeed = 0.06;
        System.Random rnd = new System.Random();
        double spd = rnd.NextDouble() * (maxSpeed - minSpeed) + minSpeed;
        cuaNeutral.y = Convert.ToSingle(spd);
        actualy += cuaNeutral.y;
        if (actualy > (max / 2) || actualy < (min / 2))
            cuaNeutral.y = cuaNeutral.y * -1;
        Cua.transform.Rotate(cuaNeutral);
    }

    void Fear()
    {

    }

    void Aburrit()
    {

    }

    void Curios()
    {

    }

    void Anxiety()
    {

    }

}