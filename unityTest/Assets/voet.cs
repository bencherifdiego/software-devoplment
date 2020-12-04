using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voet : MonoBehaviour
{
    public float speed = 5f;
    public float sped = 0f;

    public List<GameObject> path;
    private int currentTarget = 0;

    public bool mayMove = true;

    public int timer = 0;
    public int maxTimer = 10;

    public float timeExisted = 0;

    averageTime aT = averageTime.instance;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(path[currentTarget].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0f, 0.125f, 0f), transform.TransformDirection(Vector3.forward * 0.25f), Color.red);

        timeExisted += Time.deltaTime;

        int layerMask = 1 << 11;

        if (mayMove)
        {
            //if (!Physics.Raycast(transform.position + new Vector3(0f, 0.125f, 0f), transform.TransformDirection(Vector3.forward), 0.25f, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (timer <= 0)
                {
                    if (Vector3.Distance(transform.position, path[currentTarget].transform.position) <= 0.1f)
                    {
                        if (path.Count > currentTarget + 1)
                        {
                            currentTarget++;

                            transform.LookAt(path[currentTarget].transform.position);
                        }
                        else
                        {
                            aT.addTime(timeExisted);

                            Destroy(this.gameObject);
                        }
                    }
                    else
                    {
                        //transform.position = Vector3.Lerp(transform.position, path[currentTarget].transform.position, speed);
                        //transform.Translate(new Vector3(path[currentTarget].transform.position.x, transform.position.y, path[currentTarget].transform.position.z) * Time.deltaTime, Space.World);
                        //transform.position = transform.position + new Vector3(path[currentTarget].transform.position.x, 0, path[currentTarget].transform.position.y) * Time.deltaTime * 0.01f;
                        transform.position += transform.forward * Time.deltaTime;
                    }
                }
                else
                {
                    timer--;
                }
            }
            //else
            {
                //timer = maxTimer;
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
        else if (other.tag == "detection")
        {
            other.GetComponentInParent<stoplicht>().hasCar = 1;
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
        else if (other.tag == "spawnDetection")
        {
            other.GetComponentInParent<carSpawner3>().hasCar = 0;
        }
    }
}
