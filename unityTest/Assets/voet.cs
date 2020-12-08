using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voet : MonoBehaviour
{
    public bool noordWaard;
    public bool oostWaard;
    public bool zuidWaard;
    public bool westWaard;

    public float speed = 5f;
    public float sped = 0f;

    public List<GameObject> path;
    private int currentTarget = 0;

    public bool mayMove = true;

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
        transform.LookAt(path[currentTarget].transform.position);
    }

    /// <summary>
    /// kijkt elke frame of de voetganger mag bewegen, zo nee dan stopt de voetganger totdat deze weer mag bewegen
    /// wanneer de voetganger mag bewegen wordt er gekeken of deze bij het volgende checkpoint is en of dit het laatste checkpoint is
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        timeExisted += Time.deltaTime;

        //als de voetganger mag bewegen
        if (mayMove)
        {
            //als de distancenaar het volgende checkpoint in path minder dan 0.25 is
            if (Vector3.Distance(transform.position, path[currentTarget].transform.position) <= 0.1f)
            {
                //als er een volgende checkpoint in path zit
                if (path.Count > currentTarget + 1)
                {
                    currentTarget++;
                    //kijk naar het volgende checkpoint
                    transform.LookAt(path[currentTarget].transform.position);
                }
                else
                {
                    //zet de tijd die deze voetganger heeft bestaan in de lijst met tijden
                    aT.addTime(timeExisted);

                    //verwijder dit object uit de simulator
                    Destroy(this.gameObject);
                }
            }
            else
            {
                //beweeg de voetganger naar voren
                transform.position += transform.forward * Time.deltaTime;
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

            //als het stoplicht rood is mag de voetganger niet meer bewegen
            if (status == 0)
            {
                mayMove = false;
            }
            //als het stoplicht groen is mag de voetganger wel bewegen
            else if (status == 1)
            {
                mayMove = true;
            }
        }
        //als de tag van het trigger object "detection" is
        else if (other.tag == "detection")
        {
            //er staat een voetganger voor het stoplicht
            other.GetComponentInParent<stoplicht>().hasCar = 1;
        }
        //als de tag van het trigger object "spawnDetection" is
        else if (other.tag == "spawnDetection")
        {
            //de voetganger staat nog in de spawn
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
            //er staat geen voetganger meer voor het stoplicht
            other.GetComponentInParent<stoplicht>().hasCar = 0;
        }
        //als de tag van het trigger object "spawnDetection" is
        else if (other.tag == "spawnDetection")
        {
            //er zit geen voetganger meer in de spawn
            other.GetComponentInParent<carSpawner3>().hasCar = 0;
        }
    }
}
