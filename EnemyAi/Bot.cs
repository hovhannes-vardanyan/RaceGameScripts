using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Bot : MonoBehaviour
{
    #region DefParameters
    [Header("Components")]
    public Transform vehicleModel;
    public Rigidbody sphere;


    [Header("Parameters")]

    [Range(0.0f, 50.0f)] public float Speed = 35f;
    [Range(0f, 100.0f)] public float steering = 80f;
    [Range(0.0f, 300.0f)] public float gravity = 120f;


    float CurrentSpeed = 35f;



    // Vehicle components
    Transform container, steering_wheel, front_wheel;
    Transform body;
    TrailRenderer trailLeft, trailRight;

    // Private
    float rotate, rotateTarget;
    bool nearGround, onGround;
    Vector3 containerBase;
    #endregion

    #region PathFinding
    //PathFinding
    public Vector3[] WayPoints;
    public int currentIndex = 0;
    public Vector3 NextWayPoint;
    public bool IsKicked;
    public float TurnDistance;
    #endregion



    private void Awake()
    {

        foreach (Transform t in GetComponentsInChildren<Transform>())
        {

            switch (t.name)
            {
                // Vehicle components
                case "steering_wheel":
                    {
                        steering_wheel = t;
                        front_wheel = steering_wheel.GetChild(0).transform;
                    }

                    break;
                case "body": body = t; break;
            }

        }

        container = vehicleModel.GetChild(0);
        containerBase = container.localPosition;

    }

    private void Start()
    {
        //Set First waypoint
        NextWayPoint = WayPoints[0];
    }


    void Update()
    {
        #region Steering
        rotateTarget = Mathf.Lerp(rotateTarget, rotate, Time.deltaTime * 10f); rotate = 0;

        // Vehicle tilt
        float tilt = 0.0f;
        container.localPosition = containerBase + new Vector3(0, Mathf.Abs(tilt) / 2000, 0);
        container.localRotation = Quaternion.Slerp(container.localRotation, Quaternion.Euler(0, rotateTarget / 8, tilt), Time.deltaTime * 10.0f);
        #endregion

        #region FollowPath
        //Rotate Towards nextwaypoint
        var lookPos = NextWayPoint - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20);


        //Distance between bot and next waypoint
        float dist = Vector3.SqrMagnitude(sphere.transform.position - NextWayPoint);
        if (dist < TurnDistance)
        {
            //Change waypoint When it gets near
            NextWayPoint = WayPoints[++currentIndex];
        }

        transform.position = (sphere.transform.position + new Vector3(0, 0.5f, 0));

        Debug.DrawLine(vehicleModel.position, NextWayPoint, Color.green);
        #endregion

    }



    public void SetWayPoints(Vector3[] WayPoints)
    {
        if (WayPoints != null)
        {
            this.WayPoints = WayPoints;
        }

    }

    //Physics
    private void FixedUpdate()
    {
        RaycastHit hitOn;
        RaycastHit hitNear;

        onGround = Physics.Raycast(transform.position, Vector3.down, out hitOn, 0.2f);
        nearGround = Physics.Raycast(transform.position, Vector3.down, out hitNear, 2.0f);

        // rotate Vehcile model according normal
        vehicleModel.up = Vector3.Lerp(vehicleModel.up, hitNear.normal, Time.deltaTime * 2.0f);
        vehicleModel.Rotate(0, transform.eulerAngles.y, 0);

        // Movement
        if (!IsKicked)
        {
            if (nearGround)
            {
                sphere.AddForce(vehicleModel.forward * CurrentSpeed * 100 * Time.deltaTime, ForceMode.Acceleration);
            }
            else
            {
                // Simulated gravity
                sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
            }
        }

        #region drag
        // Simulated drag on ground 
        Vector3 localVelocity = transform.InverseTransformVector(sphere.velocity);
        localVelocity.x *= 0.9f;
        if (nearGround)
        {
            sphere.velocity = transform.TransformVector(localVelocity);
        }
        //Rotate Front Wheel
        front_wheel.Rotate(new Vector3(CurrentSpeed, 0, 0));
        #endregion


    }

    #region Kicked
    //call when player hits enemy
    public void KickOut()
    {
        Debug.Log("Hit");
        IsKicked = true;

        sphere.AddForce(50 * Vector3.up, ForceMode.Impulse);
        //Start coroutine to destroy kicked enemy
        StartCoroutine(WaitToDestroy());
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(transform.parent.gameObject);
    }
    #endregion

    #region Managment
    // This Is should be called from game manager to activate and deactivate bots
    public void SetBotActive(bool activeStatus)
    {
        if (activeStatus)
            CurrentSpeed = Speed;
        else
            CurrentSpeed = 0;
    }

    //Use this to Random Speed for Bots In GameManager
    public void SetCurrentSpeed(float speedCount)
    {
        this.Speed = speedCount;
        CurrentSpeed = Speed;
    }
    #endregion
}