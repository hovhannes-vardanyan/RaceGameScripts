using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [Header("Road Parameters")]
    public int PathLength;
    public int StraightPartCount;
    public int AnglePartCount;
    public Transform StartPoint;

    [Header("RoadParts")]
    public List<GameObject> StraightRoadParts;
    public List<GameObject> AngleRoadParts;


    public List<GameObject> RoadParts;

    void Start()
    {
        StraightPartCount = (int)(PathLength * 0.75f);
        AnglePartCount = PathLength - StraightPartCount;


        GeneratePath();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //void GeneratePath()
    //{
    //    //int count;
    //    //int straightCount = StraightPartCount / AnglePartCount;
    //    //Vector3 spawnPos = StartPoint.position;



    //    //bool isStraight = true;
    //    //int straightCountChecker = 0;

    //    ////Selected roadpart holder
    //    //GameObject selectedRoadPart = new GameObject();
    //    //for (int i = 0; i < PathLength; i++)
    //    //{

    //    //    if (isStraight)
    //    //    {
    //    //        selectedRoadPart = StraightRoadParts[Random.Range(0, StraightRoadParts.Count)]; 
    //    //    }
    //    //    else 
    //    //    { 
    //    //        selectedRoadPart = AngleRoadParts[Random.Range(0, AngleRoadParts.Count)]; 
    //    //    }

    //    //    var currentRoadPart = Instantiate(selectedRoadPart, spawnPos, selectedRoadPart.transform.rotation);
    //    //    straightCountChecker++;
    //    //    spawnPos = currentRoadPart.GetComponent<RoadPart>().EndPoint.transform.position;
    //    //}

    //}

    void GeneratePath()
    {
        Vector3 spawnPos = StartPoint.position;
        GameObject selectedRoadPart = new GameObject();
        bool isStraight = true;
        int straightCountChecker = 0;
        int straightPathLength = UnityEngine.Random.Range(3, 5);



        for (int i = 0; i < PathLength; i++)
        {

            if (isStraight)
            {
                selectedRoadPart = StraightRoadParts[Random.Range(0, StraightRoadParts.Count)];
                straightCountChecker++;
                if (straightCountChecker > straightPathLength)
                {
                    isStraight = false;
                    straightCountChecker = 0;
                }
            }
            else
            {
                selectedRoadPart = AngleRoadParts[Random.Range(0, AngleRoadParts.Count)];
                isStraight = true;
            }


            var currentRoadPart = Instantiate(selectedRoadPart, spawnPos, selectedRoadPart.transform.rotation);
            straightCountChecker++;
            spawnPos = currentRoadPart.GetComponent<RoadPart>().EndPoint.transform.position;
        }

    }
}
