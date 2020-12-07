using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class averageTime : MonoBehaviour
{
    public static averageTime instance = new averageTime();
    public List<float> times = new List<float>();

    public Text showTime;

    // Start is called before the first frame update
    void Start()
    {
        instance.showTime = showTime;

        StartCoroutine("calculate");
    }

    /// <summary>
    /// voegt een tijd toe aan de lijst met tijden
    /// </summary>
    /// <param name="time"></param>
    public void addTime(float time)
    {
        Debug.Log("time added");
        instance.times.Add(time);
    }

    /// <summary>
    /// berekent de gemiddelde tijd door elke tijd bij elkaar op te tellen en daarna te delen door het aantal tijden
    /// </summary>
    /// <returns></returns>
    IEnumerator calculate()
    {
        while (true)
        {
            Debug.Log("calculating");
            float total = 0;
            foreach (float t in instance.times)
            {
                total += t;
            }
            float time = total / instance.times.Count;

            //laat zien hoelang een voertuig gemiddeld doet om het kruispunt door te komen
            instance.showTime.text = time.ToString();

            //wacht 1 seconde
            yield return new WaitForSeconds(1f);
        }
    }
}
