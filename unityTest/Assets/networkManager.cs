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

    //public GameObject sl;
    //stoplicht stoplicht;

    public List<GameObject> stoplichten = new List<GameObject>();

    public float timerMax = 2f;
    public float timer = 0;

    public string message;
    public bool newMessage = false;

    Socket sender;

    IPAddress ipAddr;
    IPEndPoint localEndPoint;

    public bool sendToServer = true;

    // Start is called before the first frame update
    public void Start()
    {
        try
        {
            ipAddr = IPAddress.Parse("127.0.0.1");
            localEndPoint = new IPEndPoint(ipAddr, 54000);

            sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(localEndPoint);

            Debug.Log("Socket connected to -> {0} " + sender.RemoteEndPoint.ToString());

        }
        catch
        {
            Debug.Log("failed");
        }

        ThreadStart childref = new ThreadStart(receive);
        Thread recvThread = new Thread(childref);
        recvThread.Start();

        ThreadStart childref2 = new ThreadStart(send);
        Thread sendd = new Thread(childref2);
        sendd.Start();

        StartCoroutine("proccesMessage");
    }

    public void Open()
    {

    }

    string formatHeader(string message)
    {
        string x = message.Length.ToString() + ':' + message;
        return x;
    }

    IEnumerator proccesMessage()
    {
        while (true)
        {
            if (newMessage)
            {
                newMessage = false;

                var splitString = message.Split(new[] { ':' }, 2);
                if (splitString[1].Length == Convert.ToInt32(splitString[0]))
                {
                    Debug.Log(splitString[1]);
                    JObject jobj = JObject.Parse(splitString[1]);

                    for (int i = 0; i < stoplichten.Count; i++)
                    {
                        try
                        {
                            Debug.Log(stoplichten[i]);
                            if (stoplichten[i].name == "Bus")
                            {
                                Debug.Log("b1");
                                if (jobj["B1-1"].ToString() == "0" || Convert.ToInt32(jobj["B1-1"]) == 0)
                                {
                                    stoplichten[i].GetComponent<stoplichtBus>().b1 = 0;
                                    stoplichten[i].GetComponent<stoplichtBus>().statusChanged = true;
                                }
                                else if (jobj["B1-1"].ToString() == "1" || Convert.ToInt32(jobj["B1-1"]) == 1)
                                {
                                    stoplichten[i].GetComponent<stoplichtBus>().b1 = 1;
                                    stoplichten[i].GetComponent<stoplichtBus>().statusChanged = true;
                                }

                                Debug.Log("b2");
                                if (jobj["B1-2"].ToString() == "0" || Convert.ToInt32(jobj["B1-2"]) == 0)
                                {
                                    stoplichten[i].GetComponent<stoplichtBus>().b2 = 0;
                                    stoplichten[i].GetComponent<stoplichtBus>().statusChanged = true;
                                }
                                else if (jobj["B1-2"].ToString() == "1" || Convert.ToInt32(jobj["B1-2"]) == 1)
                                {
                                    stoplichten[i].GetComponent<stoplichtBus>().b2 = 1;
                                    stoplichten[i].GetComponent<stoplichtBus>().statusChanged = true;
                                }
                            }
                            else if (jobj[stoplichten[i].name].ToString() == "0" || Convert.ToInt32(jobj[stoplichten[i].name]) == 0)
                            {
                                if (stoplichten[i].GetComponent<stoplicht>().statuss != 0)
                                {
                                    //stoplicht.status = 0;
                                    //stoplicht.statusChanged = true;
                                    stoplichten[i].GetComponent<stoplicht>().StartCoroutine("changeToRed", 3f);
                                }
                            }
                            else if (jobj[stoplichten[i].name].ToString() == "1" || Convert.ToInt32(jobj[stoplichten[i].name]) == 1)
                            {
                                if (stoplichten[i].GetComponent<stoplicht>().statuss != 1)
                                {
                                    //stoplicht.status = 1;
                                    //stoplicht.statusChanged = true;
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

            yield return new WaitForSeconds(0.1f);
        }
    }

    //Update is called once per frame
    void Update()
    {

    }

    void receive()
    {
        while (true)
        {
            if (sender.Connected)
            {
                try
                {
                    byte[] messageReceived = new byte[1024];

                    int byteRecv = sender.Receive(messageReceived);
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
                sender.Connect(localEndPoint);
            }
            Thread.Sleep(50);
        }
    }

    void send()
    {
        while (true)
        {
            if (sender.Connected)
            {
                if (sendToServer)
                {
                    string message = jason.instance.jobj.ToString().Replace(" ", string.Empty);
                    message = message.Replace("\n", string.Empty);
                    Debug.Log(message);
                    sender.Send(Encoding.UTF8.GetBytes(formatHeader(message)));
                }

                Thread.Sleep(1000);
            }
        }
    }
}
