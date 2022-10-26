using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Transform wayPoints;
    public List<Transform>  slimeWayPoint;
    public List<Transform> chestWayPoint;
    public List<Transform> golemWayPoint;



    // Start is called before the first frame update
    void Start()
    {
        wayPoints = transform.Find("WayPoints");

        foreach (Transform g in wayPoints.transform.GetComponentsInChildren<Transform>())
        {
            if (g.tag == "SlimeWayPoint")
            {
                slimeWayPoint.Add(g);
            }else if (g.tag == "ChestWayPoint")
            {
                chestWayPoint.Add(g);
            }
            else if (g.tag == "GolemWayPoint")
            {
                golemWayPoint.Add(g);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
