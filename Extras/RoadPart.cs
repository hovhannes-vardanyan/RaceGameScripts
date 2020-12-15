using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoadType
{ 
    StraightPart,
    AnglePart
}

public class RoadPart : MonoBehaviour
{
    public Transform EndPoint;
    public RoadType Type;
}
