using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class stoplichtBus : MonoBehaviour
{
    public GameObject Stoplicht;
    Renderer sl;
    public Material green;
    public Material red;
    public Material orange;

    public GameObject currentBus;

    public int b1 = 0;
    public int b2 = 0;

    public bool statusChanged = false;
    public int statuss = 0;
    public int hasb1 = 0;
    public int hasb2 = 0;
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
        if (statusChanged)
        {
            //als b1 en b2 0 zijn zet het stoplicht op rood
            if (b1 == 0 && b2 == 0)
            {
                sl.material = red;
            }
            //als b1 of b2 1 is zet het stoplicht op groen
            else if (b1 == 1 || b2 == 1)
            {
                sl.material = green;
            }
        }

        jason jsn = jason.instance;

        //pas de json aan
        StartCoroutine("changeJson", jsn);
    }

    /// <summary>
    /// pas de json aan
    /// </summary>
    /// <param name="jsn"></param>
    /// <returns></returns>
    IEnumerator changeJson(jason jsn)
    {
        //als b1 voor het stoplicht staat
        if (hasb1 == 1)
        {
            //B1-1 in de json wordt op 1 gezet
            jsn.jobj["B1-1"] = 1;
        }
        //als b1 niet voor het stoplicht staat
        else if (hasb1 == 0)
        {
            //B1-1 in de json wordt op 0 gezet
            jsn.jobj["B1-1"] = 0;
        }

        //als b2 voor het stoplicht staat
        if (hasb2 == 1)
        {
            //B1-2 in de json wordt op 1 gezet
            jsn.jobj["B1-2"] = 1;
        }
        //als b2 niet voor het stoplicht staat
        else if (hasb2 == 0)
        {
            //B1-2 in de json wordt op 0 gezet
            jsn.jobj["B1-2"] = 0;
        }

        //wacht 0.2 seconden
        yield return new WaitForSeconds(0.2f);
    }
}
