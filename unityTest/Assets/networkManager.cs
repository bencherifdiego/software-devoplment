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
                        stoplicht stoplicht = stoplichten[i].GetComponent<stoplicht>();

                        try
                        {
                            if (jobj[stoplichten[i].name].ToString() == "0" || Convert.ToInt32(jobj[stoplichten[i].name]) == 0)
                            {
                                if (stoplicht.statuss != 0)
                                {
                                    //stoplicht.status = 0;
                                    //stoplicht.statusChanged = true;
                                    stoplicht.StartCoroutine("changeToRed", 2f);
                                }
                            }
                            else if (jobj[stoplichten[i].name].ToString() == "1" || Convert.ToInt32(jobj[stoplichten[i].name]) == 1)
                            {
                                if (stoplicht.statuss != 1)
                                {
                                    //stoplicht.status = 1;
                                    //stoplicht.statusChanged = true;
                                    stoplicht.StartCoroutine("changeToGreen", 4f);
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
                    sender.Send(Encoding.UTF8.GetBytes(formatHeader(jason.instance.jobj.ToString())));
                }

                Thread.Sleep(1000);
            }
        }
    }
}
