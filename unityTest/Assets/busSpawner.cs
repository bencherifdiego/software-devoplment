using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class busSpawner : MonoBehaviour
{
    public GameObject car;

    public float timer = 0.01f;
    public float time = 0f;

    public int hasCar = 0;

    public bool maySpawn;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (maySpawn)
        {
            if (time <= 0)
            {
                if (hasCar == 0)
                {
                    StartCoroutine("spawnCar");
                    time = timer;
                }
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
    }

    IEnumerator spawnCar()
    {
        if (hasCar == 0)
        {
            GameObject Car = Instantiate(car, transform.position, transform.rotation);
            spawnPoint spawnpoint = GetComponent<spawnPoint>();
            if (spawnpoint.numPaths == 1)
            {
                Car.GetComponent<car2>().path = spawnpoint.path1;
            }
            else if (spawnpoint.numPaths == 2)
            {
                int rnd = UnityEngine.Random.Range(0, 2);
                if (rnd == 0)
                {
                    Car.GetComponent<car2>().path = spawnpoint.path1;
                }
                else
                {
                    Car.GetComponent<car2>().path = spawnpoint.path2;
                }
            }
        }
        yield return new WaitForSeconds(0f);
    }
}
