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

    // Start is called before the first frame update
    void Start()
    {
        sl = Stoplicht.GetComponent<Renderer>();

        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        transform.GetChild(2).GetComponent<Renderer>().enabled = false;

        sl.material = red;
    }

    //Update is called once per frame
    void Update()
    {
        if (statusChanged)
        {
            if (b1 == 0 && b2 == 0)
            {
                //StartCoroutine("changeToRed", 5f);
                //statuss = 0;
                sl.material = red;
            }
            else if (b1 == 1 || b2 == 1)
            {
                //StartCoroutine("changeToGreen", 0.5f);
                //statuss = 1;
                sl.material = green;
            }
        }

        jason jsn = jason.instance;

        StartCoroutine("changeJson", jsn);
    }

    IEnumerator changeToGreen(float delay)
    {
        yield return new WaitForSeconds(delay);
        sl.material = green;
        statuss = 1;
    }

    IEnumerator changeToRed(float delay)
    {
        sl.material = orange;
        statuss = 0;
        yield return new WaitForSeconds(delay);
        sl.material = red;
    }

    IEnumerator changeJson(jason jsn)
    {
        if (hasb1 == 1)
        {
            jsn.jobj["B1-1"] = 1;
        }
        else if (hasb1 == 0)
        {
            jsn.jobj["B1-1"] = 0;
        }

        if (hasb2 == 1)
        {
            jsn.jobj["B1-2"] = 1;
        }
        else if (hasb2 == 0)
        {
            jsn.jobj["B1-2"] = 0;
        }

        yield return new WaitForSeconds(0.2f);
    }
}
