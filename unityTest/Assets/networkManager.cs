using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class networkManager : MonoBehaviour
{
    private WebSocket ws;

    public List<GameObject> stoplichten = new List<GameObject>();

    public float timerMax = 2f;
    public float timer = 0;

    public string message;
    public bool newMessage = false;

    Socket sender;

    IPAddress ipAddr;
    IPEndPoint localEndPoint;

    public bool sendToServer = true;

    /// <summary>
    /// verbind met een tcp socket server
    /// start een nieuwe thread die zich bezig houd met het ontvangen van berichten
    /// start een nieuwe thread die zich bezig houd met het versturen van berichten
    /// </summary>
    // Start is called before the first frame update
    public void Start()
    {
        try
        {
            //stel het ip addres en port nummer in
            ipAddr = IPAddress.Parse("127.0.0.1");
            localEndPoint = new IPEndPoint(ipAddr, 54000);

            sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //maak verbinding met de server
            sender.Connect(localEndPoint);

            Debug.Log("Socket connected to -> {0} " + sender.RemoteEndPoint.ToString());

        }
        catch
        {
            Debug.Log("failed");
        }

        //start een thread die zich bezig houd met het ontvangen van berichten
        ThreadStart childref = new ThreadStart(receive);
        Thread recvThread = new Thread(childref);
        recvThread.Start();

        //start een thread die zich bezig houd met het versturen van berichten
        ThreadStart childref2 = new ThreadStart(send);
        Thread sendd = new Thread(childref2);
        sendd.Start();

        //verwerk ontvangen berichten
        StartCoroutine("proccesMessage");
    }

    public void Open()
    {

    }

    //formateer de header
    string formatHeader(string message)
    {
        //voeg de lengte van het bericht gevolgd door : aan het begin van het bericht toe
        string x = message.Length.ToString() + ':' + message;
        return x;
    }

    /// <summary>
    /// verwerk alle binnenkomende berichten
    /// </summary>
    /// <returns></returns>
    IEnumerator proccesMessage()
    {
        while (true)
        {
            //wanneer er een nieuw bericht binnengekomen is
            if (newMessage)
            {
                newMessage = false;

                //split het bericht op in de header en het bericht zelf
                var splitString = message.Split(new[] { ':' }, 2);
                //kijk of de lengte van het bericht klopt door deze te vergelijken met de header
                if (splitString[1].Length == Convert.ToInt32(splitString[0]))
                {
                    //zet het bericht om in json
                    JObject jobj = JObject.Parse(splitString[1]);

                    //ga elk stoplicht wat in de lijst staat bij langs
                    for (int i = 0; i < stoplichten.Count; i++)
                    {
                        try
                        {
                            //als het stoplicht Bus heet
                            if (stoplichten[i].name == "Bus")
                            {
                                //als B1-1 in de json 0 is
                                if (jobj["B1-1"].ToString() == "0" || Convert.ToInt32(jobj["B1-1"]) == 0)
                                {
                                    //zet het stoplicht op rood
                                    stoplichten[i].GetComponent<stoplichtBus>().b1 = 0;
                                    stoplichten[i].GetComponent<stoplichtBus>().statusChanged = true;
                                }
                                //als B1-1 in de json 1 is
                                else if (jobj["B1-1"].ToString() == "1" || Convert.ToInt32(jobj["B1-1"]) == 1)
                                {
                                    //zet het stoplicht op groen
                                    stoplichten[i].GetComponent<stoplichtBus>().b1 = 1;
                                    stoplichten[i].GetComponent<stoplichtBus>().statusChanged = true;
                                }

                                //als B1-2 in de json 0 is
                                if (jobj["B1-2"].ToString() == "0" || Convert.ToInt32(jobj["B1-2"]) == 0)
                                {
                                    //zet het stoplicht op rood
                                    stoplichten[i].GetComponent<stoplichtBus>().b2 = 0;
                                    stoplichten[i].GetComponent<stoplichtBus>().statusChanged = true;
                                }
                                //als B1-2 in de json 1 is
                                else if (jobj["B1-2"].ToString() == "1" || Convert.ToInt32(jobj["B1-2"]) == 1)
                                {
                                    //zet het stoplicht op groen
                                    stoplichten[i].GetComponent<stoplichtBus>().b2 = 1;
                                    stoplichten[i].GetComponent<stoplichtBus>().statusChanged = true;
                                }
                            }
                            //pak de naam van het stoplicht en kijk of deze in de json op 0 staat
                            else if (jobj[stoplichten[i].name].ToString() == "0" || Convert.ToInt32(jobj[stoplichten[i].name]) == 0)
                            {
                                //als het stoplicht nog niet op rood staat
                                if (stoplichten[i].GetComponent<stoplicht>().statuss != 0)
                                {
                                    //zet het stoplicht op rood
                                    stoplichten[i].GetComponent<stoplicht>().StartCoroutine("changeToRed", 3f);
                                }
                            }
                            //pak de naam van het stoplicht en kijk of deze in de json op 1 staat
                            else if (jobj[stoplichten[i].name].ToString() == "1" || Convert.ToInt32(jobj[stoplichten[i].name]) == 1)
                            {
                                //als het stoplicht nog niet op groen staat
                                if (stoplichten[i].GetComponent<stoplicht>().statuss != 1)
                                {
                                    //zet het stoplicht op groen
                                    stoplichten[i].GetComponent<stoplicht>().StartCoroutine("changeToGreen", 0f);
                                }
                            }
                        }
                        catch
                        {
                            Debug.Log("failed to change stoplicht");
                        }
                    }
                }
                else
                {
                    Debug.Log("invallid message");
                    Debug.Log(splitString[1].Length);
                    Debug.Log(splitString[1]);
                    Debug.Log(Convert.ToInt32(splitString[0]));
                }
            }

            //wacht 0.1 seconde
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// ontvang berichten van de server
    /// </summary>
    void receive()
    {
        while (true)
        {
            //als er een connectie is
            if (sender.Connected)
            {
                try
                {
                    byte[] messageReceived = new byte[1024];

                    //ontvang bericht
                    int byteRecv = sender.Receive(messageReceived);
                    //zet het ontvangen bericht om naar een string
                    message = Encoding.UTF8.GetString(messageReceived, 0, byteRecv);
                    Debug.Log("message received");
                    Debug.Log(message);
                    newMessage = true;
                }
                catch
                {
                    Debug.Log("message failed to receive");
                }
            }
            else
            {
                //maak verbinding
                sender.Connect(localEndPoint);
            }
            Thread.Sleep(50);
        }
    }

    /// <summary>
    /// verstuur berichten naar de server
    /// </summary>
    void send()
    {
        while (true)
        {
            //als er een connectie is
            if (sender.Connected)
            {
                //als er naar de server gestuurd mag worden
                if (sendToServer)
                {
                    //zet de json om naar een string
                    string message = jason.instance.jobj.ToString();

                    //haal geforceerd bepaalde tekens uit de string
                    message = message.Replace(" ", string.Empty);
                    message = message.Replace("\n", string.Empty);
                    message = message.Replace("\r", string.Empty);
                    message = message.Replace("\\", string.Empty);

                    //voeg de header toe aan de string
                    string formattedMessage = formatHeader(message);

                    //verstuur de string naar de server
                    sender.Send(Encoding.UTF8.GetBytes(formattedMessage));
                }

                Thread.Sleep(1000);
            }
        }
    }
}
