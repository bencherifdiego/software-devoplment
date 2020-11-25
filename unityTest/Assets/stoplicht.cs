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
            if (status == 0)
            {
                StartCoroutine("changeToRed", 5f);
                //statuss = 0;
                //sl.material = red;
            }
            else if (status == 1)
            {
                //StartCoroutine("changeToGreen", 0.5f);
                statuss = 1;
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
        if (hasCar == 1)
        {
            jsn.jobj[transform.name] = 1;
        }
        else if (hasCar == 0)
        {
            jsn.jobj[transform.name] = 0;
        }

        yield return new WaitForSeconds(0.2f);
    }
}
