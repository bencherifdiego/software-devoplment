using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carSpawnManager : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> busSpawns = new List<GameObject>();
    public float spawnDelay = 2f;
    public float busSpawnDelay = 15f;
    public bool carMaySpawn = false;
    public bool busMaySpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("spawn");
        StartCoroutine("busSpawn");
    }

    IEnumerator spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (carMaySpawn)
            {
                int rnd = UnityEngine.Random.Range(0, spawnPoints.Count);
                spawnPoints[rnd].GetComponent<carSpawner3>().StartCoroutine("spawnCar");
            }
        }
    }

    IEnumerator busSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(busSpawnDelay);

            if (busMaySpawn)
            {
                int rnd = UnityEngine.Random.Range(0, busSpawns.Count);
                busSpawns[rnd].GetComponent<carSpawner3>().StartCoroutine("spawnBus");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
