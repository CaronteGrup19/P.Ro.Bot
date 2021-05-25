using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

using System.Text;
using System.Threading;




public class MovimentEix : MonoBehaviour
{
    public const int STAND = 0;
    public const int WALK = 1;
    public const int T_LEFT = 2;
    public const int T_RIGHT = 3;
    public const int BACK = 4;
    public const int HAPPY = 5;
    public const int RUN = 6;
    public const int BUSCA = 7;
    public const int NERVIOS = 8;
    public const int TRIST = 9;

    public const int PILOTA = 0;
    public const int PERSONA = 1;

    private const int DIST_PINCES = 20;

    private GameObject posicio_inicial;
    public Vector3 Eix_1;
    public float speed;
    public float speedbusca = -3.0f;
    public int moviment = 0;
    private int i = 0;
    private float defaultspeed = 0f;
    private float distancia;
    private bool pilotaAgafada;
    private bool arribatDesti;
    private int target;
    private float timeBetweenBark;
    private float time;
    private int nervi = 0;
    private bool tancant = false;



    public GameObject EixFR;
    public GameObject EixFL;
    public GameObject EixBR;
    public GameObject EixBL;
    private GameObject cua;
    private Quaternion rot_cua;
    private GameObject Pivot;
    private GameObject puntacua;
    [SerializeField] GameObject bocaD;
    [SerializeField] GameObject bocaE;
    [SerializeField] GameObject pilota;
    [SerializeField] GameObject persona;
    [SerializeField] GameObject probot;

    // Atributs de la cua
    private Vector3 cuaHappy;
    private int min = -30;
    private int max = 30;
    private int canvi = 0;
    private float actualy = 0;
    private float actualz = 0;
    private System.Random _random = new System.Random();
    private float minZcuaHappy = -35.0f;
    private float maxZcuaHappy = -15.0f;
    private float auxz;

    //Arxius d'audio
    [SerializeField] public AudioSource repos_track;
    [SerializeField] public AudioSource lladruc_track;
    [SerializeField] public AudioSource sad_track;
    private int countdown;
    //private AudioSource Audio;


    //Variables Pinces
    [SerializeField] public Vector3 pindreta;
    [SerializeField] public Vector3 pinesquerra;
    [SerializeField] private int minpinces = -1;
    [SerializeField] private int maxpinces = 1;
    [SerializeField] private float actualpinces;


    //public int num = -2;
    //[SerializeField] private Socket mySocket;


    // Start is called before the first frame update
    void Start()
    {
        cuaHappy = new Vector3(0.0f, 0.5f, 0.0f);
        EixFL = GameObject.Find("RodaFrontLeft");
        EixFR = GameObject.Find("RodaFrontRight");
        EixBL = GameObject.Find("RodaRearLeft");
        EixBR = GameObject.Find("RodaRearRight");
        cua = GameObject.Find("PuntUnioCua");
        bocaD = GameObject.Find("GripDreta");
        bocaE = GameObject.Find("GripEsquerra");
        pilota = GameObject.Find("Pilota");
        probot = GameObject.Find("P.Ro.Bot");
        persona = GameObject.Find("PosicioInicial");
        posicio_inicial = GameObject.Find("PosicioInicial");
        Pivot = GameObject.Find("Pivot");

        timeBetweenBark = 5.0f;
        time = 0.0f;

        puntacua = GameObject.Find("EsqueletCua");
        defaultspeed = speed;
        pilotaAgafada = false;
        arribatDesti = false;
        rot_cua = cua.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        //mySocket = FindObjectOfType<mySocket>();
        //moviment = mySocket.getAccio();

        switch (moviment)
        {
            case STAND:
                Stand();

                break;

            case WALK: //walk
                speed = defaultspeed;
                Ahead();
                Stand();
                break;

            case T_LEFT:
                speed = defaultspeed;
                TurnLeft();
                break;

            case T_RIGHT:
                speed = defaultspeed;
                TurnRight();
                break;

            case BACK:
                speed = defaultspeed;
                Backwards();
                break;

            case HAPPY: //Content per anar a correr o buscar la pilota
                speed = defaultspeed;
                if (speed == defaultspeed) speed = 3 * defaultspeed;
                if (i < 50)
                {
                    Ahead();
                }
                else Backwards();
                i++;
                if (i == 100) i = 0;
                Happy();

                TancaBoca();

                if (Vector3.Distance(transform.position, pilota.transform.position) > (DIST_PINCES * 3))
                {
                    pilotaAgafada = false;
                    moviment = BUSCA;
                }
                break;

            case RUN: //run
                speed = defaultspeed;
                if (speed == defaultspeed) speed = 5 * defaultspeed;
                Ahead();
                Happy();
                break;

            case BUSCA:
                //Comprovem si quin objecte és a menor distància
                //Busca pilota
                if (speed == defaultspeed) speed = defaultspeed * 1;
                repos_track.Play();

                //Guardem les dades de la cua
                if (pilota.transform.position.y >= 10)
                {
                    time += Time.deltaTime;
                    if(time >= timeBetweenBark)
                    {
                        repos_track.Pause();
                        lladruc_track.Play();
                        time = 0.0f;
                        
                    }else if((!lladruc_track.isPlaying) && (!repos_track.isPlaying))
                        repos_track.Play();

                }
                else
                {
                    
                    if (!pilotaAgafada)
                    {
                        if (Vector3.Distance(transform.position, pilota.transform.position) < Vector3.Distance(transform.position, persona.transform.position))
                        {
                            target = PILOTA;
                            distancia = Vector3.Distance(transform.position, pilota.transform.position);
                            if (distancia >= DIST_PINCES)
                            {
                                if (!Busca(pilota))
                                {
                                    Ahead();
                                    Curios(0);
                                }
                            }
                        }
                        //Busca persona
                        else
                        {
                            target = PERSONA;
                            distancia = Vector3.Distance(transform.position, persona.transform.position);
                            if (distancia >= DIST_PINCES)
                            {
                                if (!Busca(persona))
                                {
                                    Ahead();
                                    Curios(0);
                                }

                            }
                        }


                        //Quan s'apropa a l'objecte obre les pinces per agafar-lo i es posa content
                        if (distancia < DIST_PINCES * 2)
                        {
                            Curios(1);
                            Happy();
                            if(target == PILOTA)
                            {
                                ObreBoca();
                                if (distancia < DIST_PINCES)
                                {
                                    //Desactivem l'script de la pilota
                                    pilota.GetComponent<ScriptPilota>().enabled = false;
                                    pilota.GetComponent<Rigidbody>().mass = 0.0f;


                                    pilota.transform.position = Pivot.transform.position;

                                    pilotaAgafada = true;

                                }
                            }
                            else
                            {
                                lladruc_track.Play();

                                moviment = HAPPY;
                            }
                            
                        }
                    }
                    else if (!arribatDesti)
                    {
                        pilota.transform.position = Pivot.transform.position;
                        pilota.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                        Happy();
                        if (Vector3.Distance(transform.position, posicio_inicial.transform.position) >= DIST_PINCES)
                        {
                            if (!Busca(posicio_inicial))
                                Ahead();
                        }
                        else
                            arribatDesti = true;
                    }
                    else
                    {

                        pilota.transform.position = pilota.transform.position;

                        if (Vector3.Distance(transform.position, posicio_inicial.transform.position) < (DIST_PINCES * 2))
                        {
                            Backwards();
                        }
                        else
                        {
                            
                            ObreBoca();
                            lladruc_track.Play();
                            moviment = HAPPY;

                        }
                    }
                }
                break;
            case NERVIOS:
                nervi++;
                if (nervi < 1000) MossegatLaCuaR();
                else MossegatLaCuaL();

                if (nervi == 2000) nervi = 0;
                break;

            case TRIST:
                time += Time.deltaTime;
                Trist(0);
                if (time >= (timeBetweenBark))
                {
                    sad_track.Stop();
                    lladruc_track.Play();
                    time = 0.0f;
                }
                else if ((!sad_track.isPlaying) && (!lladruc_track.isPlaying))
                    sad_track.Play();
                break;

        }

    }


    private void Curios(int opcio)
    {
        if (opcio == 0)
        {
            Quaternion rot = Quaternion.Euler(0, 0, -90);
            cua.transform.localRotation = Quaternion.SlerpUnclamped(cua.transform.localRotation, rot, Time.deltaTime * speed);
        }else if(opcio == 1)
        {
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            cua.transform.localRotation = Quaternion.SlerpUnclamped(cua.transform.localRotation, rot, Time.deltaTime * speed);
        }
    }

    private void Trist(int opcio)
    {
        if (opcio == 0)
        {
            Quaternion rot = Quaternion.Euler(0, 0, 30);
            cua.transform.localRotation = Quaternion.SlerpUnclamped(cua.transform.localRotation, rot, Time.deltaTime * (speed/10));

        }
        else if (opcio == 1)
        {
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            cua.transform.localRotation = Quaternion.SlerpUnclamped(cua.transform.localRotation, rot, Time.deltaTime * speed);
        }
    }

    public void Stand()
    {
        actualy += cuaHappy.y;
        canvi = 0;
        if (actualy > (max/2) || actualy < (min/2))
        {
            cuaHappy.y = cuaHappy.y * -1;
            canvi = 1;
        }
        if (canvi == 1)
        {
            actualz = cua.transform.localRotation.eulerAngles.z;
            auxz = _random.Next(-100, 100);
            auxz = auxz / 100;
            if (actualz > 180) actualz -= 360;
            if (actualz < minZcuaHappy + auxz) auxz = 0.60f;
            if (actualz > maxZcuaHappy + auxz) auxz = -0.60f;
            cuaHappy.z += auxz;
        }
        else cuaHappy.z /= 2;
        cua.transform.Rotate(cuaHappy);
    }

    public void Ahead()
    {
        EixFR.transform.Rotate(speed * Eix_1);
        EixFL.transform.Rotate(speed * Eix_1);
        EixBL.transform.Rotate(speed * Eix_1);
        EixBR.transform.Rotate(speed * Eix_1);
        probot.transform.position += probot.transform.forward * Time.deltaTime * speed * 5;
    }

    public void Backwards()
    {
        EixFR.transform.Rotate(speed * -1 * Eix_1);
        EixFL.transform.Rotate(speed * -1 * Eix_1);
        EixBL.transform.Rotate(speed * -1 * Eix_1);
        EixBR.transform.Rotate(speed * -1 * Eix_1);
        probot.transform.position += probot.transform.forward * Time.deltaTime * speed * -5;
    }

    public void TurnLeft()
    {
        EixFR.transform.Rotate(speed * 1 * Eix_1);
        EixFL.transform.Rotate(speed * -1 * Eix_1);
        EixBL.transform.Rotate(speed * -1 * Eix_1);
        EixBR.transform.Rotate(speed * 1 * Eix_1);
    }

    public void TurnRight()
    {
        EixFR.transform.Rotate(speed * -1 * Eix_1);
        EixFL.transform.Rotate(speed * 1 * Eix_1);
        EixBL.transform.Rotate(speed * 1 * Eix_1);
        EixBR.transform.Rotate(speed * -1 * Eix_1);
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
            actualz = cua.transform.localRotation.eulerAngles.z;
            //Debug.Log("IN:"+actualz);
            auxz = _random.Next(-100, 100);
            auxz = auxz / 100;
            if (actualz > 180) actualz -= 360;
            if (actualz < minZcuaHappy+auxz) auxz = 0.60f;
            if (actualz > maxZcuaHappy+auxz) auxz = -0.60f;
            //Debug.Log("OUT"+actualz);
            cuaHappy.z += auxz;
        }
        else cuaHappy.z /= 2;
        cua.transform.Rotate(cuaHappy);
    }

    bool Busca(GameObject objecte)
    {

        Pivot.transform.LookAt(objecte.transform.position); 
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Pivot.transform.rotation, speed * 4 * Time.deltaTime);
        if (Pivot.transform.rotation == transform.rotation)
            return false;
        else
            return true;
    }

    void ObreBoca()
    {
        tancant = false;
        actualpinces += pindreta.z;
        if (actualpinces <= maxpinces)
        {
            pindreta.z += 0.001f;
            bocaD.transform.Rotate(pindreta);
            bocaE.transform.Rotate(pindreta);
        }
        else
            tancant = true;

    }
    void TancaBoca()
    {
        tancant = true;
        actualpinces = pindreta.z;
        if (actualpinces >= minpinces)
        {
            pindreta.z -= 0.001f;
            bocaD.transform.Rotate(-1 * pindreta);
            bocaE.transform.Rotate(-1 * pindreta);
        }
        else
            tancant = false;
    }

    void CuaPerseguir(string direccio)
    {
        if (direccio == "R")
        {
            Curios(0);
            Quaternion rot = Quaternion.Euler(-90, -45, 0);
            cua.transform.localRotation = Quaternion.SlerpUnclamped(cua.transform.localRotation, rot, Time.deltaTime * speed);

            Pivot.transform.LookAt(puntacua.transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Pivot.transform.rotation, speed * 100 * Time.deltaTime);
        }
        if (direccio == "L")
        {
            Curios(0);
            Quaternion rot = Quaternion.Euler(90, 45, 0);
            cua.transform.localRotation = Quaternion.SlerpUnclamped(cua.transform.localRotation, rot, Time.deltaTime * speed);

            Pivot.transform.LookAt(puntacua.transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Pivot.transform.rotation, speed * 100 * Time.deltaTime);
        }


    }

    void MossegatLaCuaR()
    {
        TurnRight();
        CuaPerseguir("R");
        if (tancant)
            TancaBoca();
        else ObreBoca();
    }

    void MossegatLaCuaL()
    {
        TurnLeft();
        CuaPerseguir("L");
        if (tancant) TancaBoca();
        else ObreBoca();
    }

}
