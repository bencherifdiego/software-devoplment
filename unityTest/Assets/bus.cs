using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bus : MonoBehaviour
{
    public List<GameObject> path;
    private int currentTarget = 0;

    public NavMeshAgent agent;

    public bool mayMove;

    public int timer = 0;
    public int maxTimer = 10;

    public string name;

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
    /// kijkt elke frame of de bus mag bewegen, zo nee dan stopt de bus totdat deze weer mag bewegen
    /// wanneer de bus mag bewegen wordt er gekeken of deze bij het volgende checkpoint is en of dit het laatste checkpoint is
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //tekent een debug lijn om de distance waarin de bus een object voor zich detecteerd zien
        Debug.DrawRay(transform.position + new Vector3(0f, 0.125f, 0f), transform.TransformDirection(Vector3.forward), Color.red);

        //stopt de bus
        agent.isStopped = true;

        timeExisted += Time.deltaTime;

        int layerMask = 1 << 11;

        //als de bus niet bij een rood stoplicht staat
        if (mayMove)
        {
            //als er geen object voor de bus staat
            if (!Physics.Raycast(transform.position + new Vector3(0f, 0.125f, 0f), transform.TransformDirection(Vector3.forward), 1f, layerMask, QueryTriggerInteraction.Ignore))
            {
                //als de timer voor detectie 0 of minder is
                if (timer <= 0)
                {
                    //bus mag bewegen
                    agent.isStopped = false;

                    //als de distance naar het volgende checkpoint in path minder dan 0.25 is
                    if (Vector3.Distance(transform.position, path[currentTarget].transform.position) <= 0.25f)
                    {
                        //als er een volgend checkpoint in path zit
                        if (path.Count > currentTarget + 1)
                        {
                            currentTarget++;
                            //verrander het doel om naartoe te bewegen naar het volgende checkpoint
                            agent.SetDestination(path[currentTarget].transform.position);
                        }
                        else
                        {
                            //zet de tijd die deze bus heeft bestaan in de lijst met tijden
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

            //als het stoplicht rood is mag de bus niet meer bewegen
            if (status == 0)
            {
                mayMove = false;
            }
            //als het stoplicht groen is mag de bus wel bewegen
            else if (status == 1)
            {
                mayMove = true;
            }
        }
        //als de tag van het trigger object "busStop" is
        else if (other.tag == "busStop")
        {
            //als deze bus vooraan staat bij dit stoplicht
            if (other.GetComponentInParent<stoplichtBus>().currentBus == this.gameObject)
            {
                //als dit bus b1 is
                if (name == "b1")
                {
                    //er word gekeken of het stoplicht groen of rood is
                    int status = other.GetComponentInParent<stoplichtBus>().b1;

                    //als het stoplicht rood is mag de bus niet meer bewegen
                    if (status == 0)
                    {
                        mayMove = false;
                    }
                    //als het stoplicht groen is mag de bus wel bewegen
                    else if (status == 1)
                    {
                        mayMove = true;
                    }
                }
                //als dit bus b2 is
                else if (name == "b2")
                {
                    //er word gekeken of het stoplicht groen of rood is
                    int status = other.GetComponentInParent<stoplichtBus>().b2;

                    //als het stoplicht rood is mag de bus niet meer bewegen
                    if (status == 0)
                    {
                        mayMove = false;
                    }
                    //als het stoplicht groen is mag de bus wel bewegen
                    else if (status == 1)
                    {
                        mayMove = true;
                    }
                }
            }
            else
            {
                //als er nog geen bus voor het stoplicht staat
                if (other.GetComponentInParent<stoplichtBus>().currentBus == null)
                {
                    //deze bus staat nu vooraan bij het stoplicht
                    other.GetComponentInParent<stoplichtBus>().currentBus = this.gameObject;
                }
            }
        }
        //als de tag van het trigger object "detection" is
        else if (other.tag == "detection")
        {
            //er staat een voertuig bij het stoplicht
            other.GetComponentInParent<stoplicht>().hasCar = 1;
        }
        //als de tag van het trigger object "detectionBus" is
        else if (other.tag == "detectionBus")
        {
            //als dit bus b1 is
            if (name == "b1")
            {
                //bus b1 staat bij het stoplicht
                other.GetComponentInParent<stoplichtBus>().hasb1 = 1;
            }
            //als dit bus b2 is
            else if (name == "b2")
            {
                //bus b2 staat bij het stoplicht
                other.GetComponentInParent<stoplichtBus>().hasb2 = 1;
            }
        }
        //als de tag van het trigger object "spawnDetection" is
        else if (other.tag == "spawnDetection")
        {
            //de bus zit nog in de plek waar deze word ingespawned
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
        //als de tag van het trigger object "detectionBus" is
        else if (other.tag == "detectionBus")
        {
            //als dit bus b1 is
            if (name == "b1")
            {
                //bus b1 staat niet meer bij het stoplicht
                other.GetComponentInParent<stoplichtBus>().hasb1 = 0;
            }
            //als dit bus b2 is
            else if (name == "b2")
            {
                //bus b2 staat niet meer bij het stoplicht
                other.GetComponentInParent<stoplichtBus>().hasb2 = 0;
            }
            other.GetComponentInParent<stoplichtBus>().currentBus = null;
        }
        //als de tag van het trigger object "spawnDetection" is
        else if (other.tag == "spawnDetection")
        {
            //er zit geen voertuig meer in de spawn
            other.GetComponentInParent<carSpawner3>().hasCar = 0;
        }
    }
}
