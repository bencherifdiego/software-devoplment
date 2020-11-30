using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carSpawner3 : MonoBehaviour
{
    public GameObject car;

    public GameObject lastSpawned;

    public int hasCar = 0;

    public bool maySpawn;

    public bool spawnCarB = true;

    public float carTimer = 2f;
    public float carTime = 0f;

    public bool spawnBusB = false;

    public float busTimer = 10f;
    public float busTime = 0f;

    public GameObject bus;

    public int busPaths = 1;
    public List<GameObject> busPath1 = new List<GameObject>();
    public List<GameObject> busPath2 = new List<GameObject>();

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
            if (spawnCarB)
            {
                if (carTime <= 0)
                {
                    if (hasCar == 0)
                    {
                        StartCoroutine("spawnCar");
                        carTime = carTimer;
                    }
                }
                else
                {
                    carTime -= Time.deltaTime;
                }
            }
            if (spawnBusB)
            {
                if (busTime <= 0)
                {
                    if (hasCar == 0)
                    {
                        StartCoroutine("spawnBus");
                        busTime = busTimer;
                    }
                }
                else
                {
                    busTime -= Time.deltaTime;
                }
            }
        }
    }

    IEnumerator spawnCar()
    {
        if (hasCar == 0)
        {
            float dist;
            if (lastSpawned == null)
            {
                dist = 5;
            }
            else
            {
                dist = Vector3.Distance(lastSpawned.transform.position, transform.position);
            }
            if (dist >= 2)
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
                lastSpawned = Car;
            }
        }
        yield return new WaitForSeconds(0f);
    }

    IEnumerator spawnBus()
    {
        if (hasCar == 0)
        {
            float dist;
            if (lastSpawned == null)
            {
                dist = 5;
            }
            else
            {
                dist = Vector3.Distance(lastSpawned.transform.position, transform.position);
            }
            if (dist >= 2)
            {
                GameObject Car = Instantiate(bus, transform.position, transform.rotation);
                if (busPaths == 1)
                {
                    Car.GetComponent<bus>().name = "b1";
                    Car.GetComponent<bus>().path = busPath1;
                }
                else if (busPaths == 2)
                {
                    int rnd = UnityEngine.Random.Range(0, 2);
                    if (rnd == 0)
                    {
                        Car.GetComponent<bus>().name = "b1";
                        Car.GetComponent<bus>().path = busPath1;
                    }
                    else
                    {
                        Car.GetComponent<bus>().name = "b2";
                        Car.GetComponent<bus>().path = busPath2;
                    }
                }
            }
        }
        yield return new WaitForSeconds(0f);
    }
}
