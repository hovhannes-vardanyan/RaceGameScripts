using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PathManager : MonoBehaviour
{

    List<Bot> Bots;
    public Vehicle Player;

    //Waypoints
    public List<Vector3> WayPoints;
    public List<GameObject> RoadParts;
    public GameObject RoadHolder;





    void Awake()
    {
        //Get Waypoints
        WayPoints = GetWayPoints();

        //Find all bots
        Bots = GameObject.FindObjectsOfType<Bot>().ToList();
        foreach (var bot in Bots)
        {
            if (bot != null)
            {
                bot.SetWayPoints(this.WayPoints.ToArray());
            }
        }


    }
    private void Start()
    {
        Player = GameObject.FindObjectOfType<Vehicle>();
    }
    
    

    public List<Vector3> GetWayPoints()
    {
        List<Vector3> tempList = new List<Vector3>();

        //Get Road Parts
        for (int i = 0; i < RoadHolder.transform.childCount; i++)
        {
            RoadParts.Add(RoadHolder.transform.GetChild(i).gameObject);
        }


        //Get Waypoints from every RoadPart
        for (int i = 0; i < RoadParts.Count; i++)
        {
            for (int j = 0; j < RoadParts[i].transform.childCount; j++)
            {
                tempList.Add(RoadParts[i].transform.GetChild(j).position);
            }
        }

        return tempList;
    }



}
