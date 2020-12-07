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
            //als de timer 0 of minder is
            if (time <= 0)
            {
                //als er geen voertuig in de spawn staat
                if (hasCar == 0)
                {
                    //spawn een bus
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

    /// <summary>
    /// spawned een bus en geeft deze een lijst checkpoints mee als route
    /// </summary>
    /// <returns></returns>
    IEnumerator spawnCar()
    {
        //als er geen voertuig in de spawn staat
        if (hasCar == 0)
        {
            //spawn een bus in
            GameObject Car = Instantiate(car, transform.position, transform.rotation);

            spawnPoint spawnpoint = GetComponent<spawnPoint>();
            //als het aantal routes 1 is
            if (spawnpoint.numPaths == 1)
            {
                //geef de route door aan de bus
                Car.GetComponent<car2>().path = spawnpoint.path1;
            }
            //als het aantal routes 2 is
            else if (spawnpoint.numPaths == 2)
            {
                //pak een random getal (0 of 1)
                int rnd = UnityEngine.Random.Range(0, 2);
                //als 0
                if (rnd == 0)
                {
                    //geef route 1 door aan de bus
                    Car.GetComponent<car2>().path = spawnpoint.path1;
                }
                //als 1
                else
                {
                    //geef route 2 door aan de bus
                    Car.GetComponent<car2>().path = spawnpoint.path2;
                }
            }
        }
        //wacht 0 seconden (dit is verplicht bij dit type functie)
        yield return new WaitForSeconds(0f);
    }
}
