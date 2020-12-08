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


    public bool spawnVoetB = false;

    public float voetTimer = 2f;
    public float voetTime = 0f;

    public GameObject Voet;

    public int voetPaths = 6;
    public List<GameObject> voetPath1 = new List<GameObject>();
    public List<GameObject> voetPath2 = new List<GameObject>();
    public List<GameObject> voetPath3 = new List<GameObject>();
    public List<GameObject> voetPath4 = new List<GameObject>();
    public List<GameObject> voetPath5 = new List<GameObject>();
    public List<GameObject> voetPath6 = new List<GameObject>();

    /// <summary>
    /// wanneer dit object inspawned wordt het visueele deel van het object uit gezet
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
    }

    /// <summary>
    /// als er gespawned mag worden wordt er op een timer gespawned
    /// (dit is voor test purposses)
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //als er gespawned mag worden
        if (maySpawn)
        {
            //als er auto's gespawned mogen worden
            if (spawnCarB)
            {
                //als de timer 0 of minder is
                if (carTime <= 0)
                {
                    //als er geen voertuig in de spawn staat
                    if (hasCar == 0)
                    {
                        //spawn een auto
                        StartCoroutine("spawnCar");
                        carTime = carTimer;
                    }
                }
                else
                {
                    carTime -= Time.deltaTime;
                }
            }
            //als er bussen gespawned mogen worden
            if (spawnBusB)
            {
                //als de timer 0 of minder is
                if (busTime <= 0)
                {
                    //als er geen voertuig in de spawn staat
                    if (hasCar == 0)
                    {
                        //spawn een bus
                        StartCoroutine("spawnBus");
                        busTime = busTimer;
                    }
                }
                else
                {
                    busTime -= Time.deltaTime;
                }
            }
            //als er voetgespawned mogen worden
            if (spawnVoetB)
            {
                //als de timer 0 of minder is
                if (voetTime <= 0)
                {
                    //als er geen voertuig in de spawn staat
                    if (hasCar == 0)
                    {
                        //spawn een voetganger
                        StartCoroutine("spawnVoet");
                        voetTime = voetTimer;
                    }
                }
                else
                {
                    voetTime -= Time.deltaTime;
                }
            }
        }
    }

    /// <summary>
    /// spawned een auto en geeft deze een lijst checkpoints mee als route
    /// </summary>
    /// <returns></returns>
    IEnumerator spawnCar()
    {
        //als er geen voertuig in de spawn staat
        if (hasCar == 0)
        {
            float dist;
            //als er nog geen object in is gespawned
            if (lastSpawned == null)
            {
                dist = 5;
            }
            else
            {
                //dist is de afstand van de spawn naar het laatst gespawde object
                dist = Vector3.Distance(lastSpawned.transform.position, transform.position);
            }
            //als de distance naar het laatst gespawnde object 3 of meer is
            if (dist >= 3)
            {
                //spawn een auto in
                GameObject Car = Instantiate(car, transform.position, transform.rotation);

                spawnPoint spawnpoint = GetComponent<spawnPoint>();
                //als het aantal routes voor auto's 1 is
                if (spawnpoint.numPaths == 1)
                {
                    //geef route 1 aan de auto mee
                    Car.GetComponent<car2>().path = spawnpoint.path1;
                }
                //als het aantal routes voor auto's 2 is
                else if (spawnpoint.numPaths == 2)
                {
                    //pak een random getal (0 of 1)
                    int rnd = UnityEngine.Random.Range(0, 2);
                    //als 0
                    if (rnd == 0)
                    {
                        //geef route 1 aan de auto mee
                        Car.GetComponent<car2>().path = spawnpoint.path1;
                    }
                    else
                    {
                        //geef route 2 aan de auto mee
                        Car.GetComponent<car2>().path = spawnpoint.path2;
                    }
                }
                lastSpawned = Car;
            }
        }
        //wacht 0 seconden (dit is verplicht bij dit type functie)
        yield return new WaitForSeconds(0f);
    }

    /// <summary>
    /// spawned een bus en geeft deze een lijst checkpoints mee als route
    /// </summary>
    /// <returns></returns>
    IEnumerator spawnBus()
    {
        //als er geen voertuig in de spawn staat
        if (hasCar == 0)
        {
            float dist;
            //als er nog geen object in is gespawned
            if (lastSpawned == null)
            {
                dist = 5;
            }
            else
            {
                //dist is de afstand van de spawn naar het laatst gespawde object
                dist = Vector3.Distance(lastSpawned.transform.position, transform.position);
            }
            //als de distance naar het laatst gespawnde object 3 of meer is
            if (dist >= 3)
            {
                //spawn een bus in
                GameObject Car = Instantiate(bus, transform.position, transform.rotation);

                //als het aantal routes voor bussen 1 is
                if (busPaths == 1)
                {
                    //geef de bus de naam b1
                    Car.GetComponent<bus>().name = "b1";
                    //geef route 1 aan de bus mee
                    Car.GetComponent<bus>().path = busPath1;
                }
                //als het aantal routes voor bussen 2 is
                else if (busPaths == 2)
                {
                    //pak een random getal (0 of 1)
                    int rnd = UnityEngine.Random.Range(0, 2);
                    //als 0
                    if (rnd == 0)
                    {
                        //geef de bus de naam b1
                        Car.GetComponent<bus>().name = "b1";
                        //geef route 1 aan de bus mee
                        Car.GetComponent<bus>().path = busPath1;
                    }
                    else
                    {
                        //geef de bus de naam b2
                        Car.GetComponent<bus>().name = "b2";
                        //geef route 2 aan de bus mee
                        Car.GetComponent<bus>().path = busPath2;
                    }
                }
            }
        }
        //wacht 0 seconden (dit is verplicht bij dit type functie)
        yield return new WaitForSeconds(0f);
    }

    /// <summary>
    /// spawned een voetganger en geeft deze een lijst checkpoints mee als route
    /// </summary>
    /// <returns></returns>
    IEnumerator spawnVoet()
    {
        //als er geen voertuig in de spawn staat
        if (hasCar == 0)
        {
            float dist;
            //als er nog geen object in is gespawned
            if (lastSpawned == null)
            {
                dist = 5;
            }
            else
            {
                //dist is de afstand van de spawn naar het laatst gespawde object
                dist = Vector3.Distance(lastSpawned.transform.position, transform.position);
            }
            //als de distance naar het laatst gespawnde object 3 of meer is
            if (dist >= 3)
            {
                //spawn een voetganger in
                GameObject Car = Instantiate(Voet, transform.position, transform.rotation);

                //als het aantal routes voor voetgangers 1 is
                if (voetPaths == 1)
                {
                    //geef route 1 aan de voetganger mee
                    Car.GetComponent<voet>().path = voetPath1;
                }
                else if (voetPaths == 6)
                {
                    //pak random getal (1,2,3,4,5 of 6)
                    int rnd = UnityEngine.Random.Range(0, 6);
                    //als 0
                    if (rnd == 0)
                    {
                        //geef route 1 aan de voetganger mee
                        Car.GetComponent<voet>().path = voetPath1;
                    }
                    //als 1
                    else if (rnd == 1)
                    {
                        //geef route 2 aan de voetganger mee
                        Car.GetComponent<voet>().path = voetPath1;
                    }
                    //als 2
                    else if (rnd == 2)
                    {
                        //geef route 3 aan de voetganger mee
                        Car.GetComponent<voet>().path = voetPath3;
                    }
                    //als 3
                    else if (rnd == 3)
                    {
                        //geef route 4 aan de voetganger mee
                        Car.GetComponent<voet>().path = voetPath4;
                    }
                    //als 4
                    else if (rnd == 4)
                    {
                        //geef route 5 aan de voetganger mee
                        Car.GetComponent<voet>().path = voetPath5;
                    }
                    //als 5
                    else if (rnd == 5)
                    {
                        //geef route 6 aan de voetganger mee
                        Car.GetComponent<voet>().path = voetPath6;
                    }
                }
                lastSpawned = Car;
            }
        }
        //wacht 0 seconden (dit is verplicht bij dit type functie)
        yield return new WaitForSeconds(0f);
    }
}
