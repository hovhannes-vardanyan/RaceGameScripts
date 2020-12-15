using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharHead : MonoBehaviour
{
    Vector3 StartDirection;
    Quaternion StartRotation;

    //Look Target
    public Transform Target;
    void Awake()
    {
        if (Target == null)
            return;

        StartDirection = Target.position - transform.position;
        StartRotation = transform.rotation;
    }


    void Update()
    {
        //Make players head look at target
        if (Target == null)
            return;


        var lookPos = Target.position - transform.position;
        var rotation = Quaternion.LookRotation(lookPos).normalized * StartRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
    }
}
