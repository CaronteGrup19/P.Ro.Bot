using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;

using System.Text;
using System.Threading;

public class Socket : MonoBehaviour
{
    MovimentEix mEix = new MovimentEix();
    Thread recibirHilo; //Definimos nustra variable   recibirHilo de tipo Thread que permitira continuamente correr a la par del programa
    UdpClient cliente;  //Definimos cliente la cual servira para la recepción de datos
    int puerto;         //Definimos el entero que idicará nuestro puerto
    private int accio = 7;

    // Start is called before the first frame update
    void Start()
    {
        puerto = 5000;     //Asignamos el puerto
        InicializarUDP();  //Llamamos el metodo InicializarUDP()         
    }

    void InicializarUDP()
    {
        print("Inicializando UDP");
        recibirHilo = new Thread(new ThreadStart(RecibirDatos));  //Se inicializa recibirHilo con el método RecibirDatos como argumento
        recibirHilo.IsBackground = true;                          //Decimos que recibirHilo se ejecute a la par de nuestro programa
        recibirHilo.Start();                                      //Damos partida a recibirHilo
    }

    void RecibirDatos()
    {
        print("Recibiendo datos");
        cliente = new UdpClient(puerto);   //Inicializamos cliente con puerto como argumento
        while (true)
        {
            try
            {
                //IPEndPoint IPFinal = new IPEndPoint(IPAddress.Parse("192.168.1.36"), puerto);  //Definimos he inicializamos IP como punto final de IP con argumentos IPAdress y la variable port 
                IPEndPoint IPFinal = new IPEndPoint(IPAddress.Parse("0.0.0.0"), puerto);
                byte[] datos = cliente.Receive(ref IPFinal);                              //Lectura de datos de la IPFinal
                string recibidos = Encoding.UTF8.GetString(datos);                        //Los datos se codifican y se asignan a recibidos
                print(recibidos);

                
                switch (recibidos)
                {
                    case "coneix":
                        accio = 10;
                        break;
                    case "pilota":
                        accio = 7;
                        break;
                    default:
                        accio = 1;
                        break;
                }
                
                print("Acció = " + accio);

            }
            catch (Exception e)
            {
                print("-1");
                print(e.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getAccio(){
        return this.accio;
    }
}