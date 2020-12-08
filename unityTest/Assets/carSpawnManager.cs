using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carSpawnManager : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> busSpawns = new List<GameObject>();
    public List<GameObject> voetSpawns = new List<GameObject>();
    public float spawnDelay = 2f;
    public float busSpawnDelay = 15f;
    public float voetSpawnDelay = 5;
    public bool carMaySpawn = false;
    public bool busMaySpawn = false;
    public bool voetMaySpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        //spawn auto
        StartCoroutine("spawn");

        //spawn bus
        StartCoroutine("busSpawn");

        //spawn voetganger
        StartCoroutine("voetSpawn");
    }

    /// <summary>
    /// spawned auto's op een timer
    /// </summary>
    /// <returns></returns>
    IEnumerator spawn()
    {
        while (true)
        {
            //wacht een aantal seconden
            yield return new WaitForSeconds(spawnDelay);

            //als er auto's mogen spawnen
            if (carMaySpawn)
            {
                //pak een random spawnpunt uit de lijst met spawnpunten voor auto's en spawn er een auto
                int rnd = UnityEngine.Random.Range(0, spawnPoints.Count);
                spawnPoints[rnd].GetComponent<carSpawner3>().StartCoroutine("spawnCar");
            }
        }
    }

    /// <summary>
    /// spawned bussen op een timer
    /// </summary>
    /// <returns></returns>
    IEnumerator busSpawn()
    {
        while (true)
        {
            //wacht een aantal seconden
            yield return new WaitForSeconds(busSpawnDelay);

            //als er bussen mogen spawnen
            if (busMaySpawn)
            {
                //pak een random spawnpunt uit de lijst met spawnpunten voor bussen en spawn er een bus
                int rnd = UnityEngine.Random.Range(0, busSpawns.Count);
                busSpawns[rnd].GetComponent<carSpawner3>().StartCoroutine("spawnBus");
            }
        }
    }

    /// <summary>
    /// spawned voetgangers op een timer
    /// </summary>
    /// <returns></returns>
    IEnumerator voetSpawn()
    {
        while (true)
        {
            //wacht een aantal seconden
            yield return new WaitForSeconds(voetSpawnDelay);

            //als er voetgangers mogen spawnen
            if (voetMaySpawn)
            {
                //pak een random spawnpunt uit de lijst met spawnpunten voor voetgangers en spawn er een voetganger
                int rnd = UnityEngine.Random.Range(0, voetSpawns.Count);
                voetSpawns[rnd].GetComponent<carSpawner3>().StartCoroutine("spawnVoet");
            }
        }
    }
}
