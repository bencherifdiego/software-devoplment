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

    public void addTime(float time)
    {
        Debug.Log("time added");
        instance.times.Add(time);
    }

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

            instance.showTime.text = time.ToString();

            yield return new WaitForSeconds(1f);
        }
    }
}
