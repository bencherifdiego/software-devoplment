using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class car2 : MonoBehaviour
{
    public List<GameObject> path;
    private int currentTarget = 0;

    public NavMeshAgent agent;

    public bool mayMove;
    public bool mayMove2 = true;

    public int timer = 0;
    public int maxTimer = 10;

    public float timeExisted = 0;

    averageTime aT = averageTime.instance;

    /// <summary>
    /// zet het huidige doel voor de pathing naar het eerste checkpoint wat in de lijst path zit
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        agent.SetDestination(path[currentTarget].transform.position);
        mayMove = true;
    }

    /// <summary>
    /// kijkt elke frame of de auto mag bewegen, zo nee dan stopt de auto totdat deze weer mag bewegen
    /// wanneer de auto mag bewegen wordt er gekeken of deze bij het volgende checkpoint is en of dit het laatste checkpoint is
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //tekent een debug lijn om de distance waarin de auto een object voor zich detecteerd zien
        Debug.DrawRay(transform.position + new Vector3(0f, 0.125f, 0f), transform.TransformDirection(Vector3.forward), Color.red);

        //stopt de auto
        agent.isStopped = true;

        timeExisted += Time.deltaTime;

        int layerMask = 1 << 11;

        //als de auto niet bij een rood stoplicht staat
        if (mayMove)
        {
            //als er geen object voor de auto staat
            if (!Physics.Raycast(transform.position + new Vector3(0f, 0.125f, 0f), transform.TransformDirection(Vector3.forward), 1f, layerMask, QueryTriggerInteraction.Ignore))
            {
                //als de timer voor detectie 0 of minder is
                if (timer <= 0)
                {
                    //auto mag bewegen
                    agent.isStopped = false;

                    //als de distance naar het volgende checkpoint in path minder dan 0.25 is
                    if (Vector3.Distance(transform.position, path[currentTarget].transform.position) <= 0.25f)
                    {
                        //als er een volgend checkpoint in path zit
                        if (path.Count > currentTarget + 1)
                        {
                            currentTarget++;
                            //verrander het doel om naartoe te bewegen naar het volende checkpoint
                            agent.SetDestination(path[currentTarget].transform.position);
                        }
                        else
                        {
                            //zet de tijd die deze auto heeft bestaan in de lijst met tijden
                            aT.addTime(timeExisted);

                            //verwijder dit object uit de simulator
                            Destroy(this.gameObject);
                        }
                    }
                }
                else
                {
                    timer--;
                }
            }
            else
            {
                timer = maxTimer;
            }
        }
    }

    /// <summary>
    /// wanneer een bus in een object dat gemarkeerd staat als trigger staat draait de volgende code
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //als de tag van het trigger object "stop" is
        if (other.tag == "stop")
        {
            //er word gekeken of het stoplicht groen of rood is
            int status = other.GetComponentInParent<stoplicht>().statuss;

            //als het stoplicht rood is mag de auto niet meer bewegen
            if (status == 0)
            {
                mayMove = false;
            }
            //als het stoplicht groen is mag de auto wel bewegen
            else if (status == 1)
            {
                mayMove = true;
            }
        }
        //als de tag van het trigger object "detection" is
        else if (other.tag == "detection")
        {
            //er staat een voertuig bij het stoplicht
            other.GetComponentInParent<stoplicht>().hasCar = 1;
        }
        //als de tag van het trigger object "spawnDetection" is
        else if (other.tag == "spawnDetection")
        {
            //de auto zit nog in de plek waar deze word ingespawned
            other.GetComponentInParent<carSpawner3>().hasCar = 1;
        }
    }

    /// <summary>
    /// wanneer een bus een object dat gemarkeerd staat als trigger verlaat draait de volgende code
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        //als de tag van het trigger object "detection" is
        if (other.tag == "detection")
        {
            //het stoplicht heeft geen voertuig meer
            other.GetComponentInParent<stoplicht>().hasCar = 0;
        }
        //als de tag van het trigger object "spawnDetection" is
        else if (other.tag == "spawnDetection")
        {
            //er zit geen voertuig meer in de spawn
            other.GetComponentInParent<carSpawner3>().hasCar = 0;
        }
    }
}
