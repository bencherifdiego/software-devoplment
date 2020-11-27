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

    // Start is called before the first frame update
    void Start()
    {
        agent.SetDestination(path[currentTarget].transform.position);
        mayMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0f, 0.125f, 0f), transform.TransformDirection(Vector3.forward), Color.red);
        //agent.SetDestination(transform.position);
        agent.isStopped = true;

        int layerMask = 1 << 11;

        if (mayMove)
        {
            if (!Physics.Raycast(transform.position + new Vector3(0f, 0.125f, 0f), transform.TransformDirection(Vector3.forward), 1f, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (timer <= 0)
                {
                    agent.isStopped = false;
                    //agent.SetDestination(path[currentTarget].transform.position);
                    if (Vector3.Distance(transform.position, path[currentTarget].transform.position) <= 0.25f)
                    {
                        if (path.Count > currentTarget + 1)
                        {
                            currentTarget++;
                            agent.SetDestination(path[currentTarget].transform.position);
                        }
                        else
                        {
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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "stop")
        {
            int status = other.GetComponentInParent<stoplicht>().statuss;

            if (status == 0)
            {
                mayMove = false;
            }
            else if (status == 1)
            {
                mayMove = true;
            }
        }
        else if (other.tag == "busStop")
        {
            if (name == "b1")
            {
                int status = other.GetComponentInParent<stoplichtBus>().b1;

                if (status == 0)
                {
                    mayMove = false;
                }
                else if (status == 1)
                {
                    mayMove = true;
                }
            }
            else if (name == "b2")
            {
                int status = other.GetComponentInParent<stoplichtBus>().b2;

                if (status == 0)
                {
                    mayMove = false;
                }
                else if (status == 1)
                {
                    mayMove = true;
                }
            }
        }
        else if (other.tag == "detection")
        {
            other.GetComponentInParent<stoplicht>().hasCar = 1;
            //other.GetComponentInParent<stoplicht>().hasCarTimer = other.GetComponentInParent<stoplicht>().hasCarMax;
        }
        else if (other.tag == "detectionBus")
        {
            if (name == "b1")
            {
                other.GetComponentInParent<stoplichtBus>().hasb1 = 1;
            }
            else if (name == "b2")
            {
                other.GetComponentInParent<stoplichtBus>().hasb2 = 1;
            }
            //other.GetComponentInParent<stoplicht>().hasCarTimer = other.GetComponentInParent<stoplicht>().hasCarMax;
        }
        else if (other.tag == "spawnDetection")
        {
            other.GetComponentInParent<carSpawner3>().hasCar = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "detection")
        {
            other.GetComponentInParent<stoplicht>().hasCar = 0;
        }
        else if (other.tag == "detectionBus")
        {
            if (name == "b1")
            {
                other.GetComponentInParent<stoplichtBus>().hasb1 = 0;
            }
            else if (name == "b2")
            {
                other.GetComponentInParent<stoplichtBus>().hasb2 = 0;
            }
        }
        else if (other.tag == "spawnDetection")
        {
            other.GetComponentInParent<carSpawner3>().hasCar = 0;
        }
    }
}
