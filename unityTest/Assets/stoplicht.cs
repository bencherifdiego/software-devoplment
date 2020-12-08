using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class stoplicht : MonoBehaviour
{
    public GameObject Stoplicht;
    Renderer sl;
    public Material green;
    public Material red;
    public Material orange;

    public int status = 0;
    public bool statusChanged = false;
    public int statuss = 0;
    public int hasCar = 0;
    public int hasCarTimer;
    public int hasCarMax = 5;

    public float delay = 5f;

    float timer = 5f;
    float time = 0;

    /// <summary>
    /// wanneer dit object inspawned wordt het visueele deel van het object uit gezet
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        sl = Stoplicht.GetComponent<Renderer>();

        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        transform.GetChild(2).GetComponent<Renderer>().enabled = false;

        sl.material = red;
    }

    /// <summary>
    /// kijkt of het stoplicht op groen of op rood moet
    /// </summary>
    //Update is called once per frame
    void Update()
    {
        //als de status van het stoplicht is verranderd
        if (statusChanged)
        {
            //als de nieuwe status 0 is
            if (status == 0)
            {
                //verrander stoplicht naar rood
                StartCoroutine("changeToRed", 5f);
            }
            //als de nieuwe status 1 is
            else if (status == 1)
            {
                //verrander stoplicht naar groen
                statuss = 1;
                sl.material = green;
            }
        }

        jason jsn = jason.instance;

        //pas de json aan
        StartCoroutine("changeJson", jsn);
    }

    /// <summary>
    /// zet het stoplicht naar oranje en naar een delay naar rood
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator changeToRed(float delay)
    {
        sl.material = orange;
        statuss = 0;
        yield return new WaitForSeconds(delay);
        sl.material = red;
    }

    /// <summary>
    /// pas de json aan
    /// </summary>
    /// <param name="jsn"></param>
    /// <returns></returns>
    IEnumerator changeJson(jason jsn)
    {
        //als er een auto voor het stoplicht staat
        if (hasCar == 1)
        {
            //de json van dit stoplicht wordt op 1 gezet
            jsn.jobj[transform.name] = 1;
        }
        //als er geen auto voor het stoplicht staat
        else if (hasCar == 0)
        {
            //de json van dit stoplicht wordt op 0 gezet
            jsn.jobj[transform.name] = 0;
        }

        //wacht 0.2 seconden
        yield return new WaitForSeconds(0.2f);
    }
}
