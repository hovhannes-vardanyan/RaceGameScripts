using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;

public class Vehicle : MonoBehaviour
{
    #region Default Paramteres
    [Header("Components")]
    public Transform vehicleModel;
    public Rigidbody sphere;



    [Header("Parameters")]
    [Range(0.0f, 20.0f)] public float Speed = 10;
    [Range(0f, 200.0f)] public float steering = 100f;
    [Range(0.0f, 300.0f)] public float gravity = 120f;
    [Range(0.0f, 20.0f)] public float jumpForce = 10f;


    // Vehicle components

    Transform container, steering_wheel, front_wheel;
    Transform body;




    private float currentSteering;
    [HideInInspector]
    public float rotate, rotateTarget;
    [HideInInspector]
    public bool nearGround, onGround;
    Vector3 containerBase;

    [HideInInspector]
    public float HorizontalInput;
    public FloatingJoystick joystick;
    Collider sphereCollider;

    public LayerMask RoadLayer;


    public Text JumpText;
    public bool IsLanded;


    #endregion

    #region Player Teleport
    List<Vector3> LastPositions;
    [HideInInspector]
    public bool HadLost = false;
    #endregion

    #region Drfiting
    [HideInInspector]
    public bool IsDrifting;
    public float CurrentSpeed;
    #endregion

    #region Moblie input
    float hInput;
    Touch touch;
    float screenWidth;
    #endregion

    #region JumpParameters
    [HideInInspector]
    public bool HasJumped;
    float defaultDrag;
    float defaultAngDrag;
    #endregion

    #region WayPointSystem
    [Header("Waypoints")]


    PathManager pathManager;
    List<Vector3> PathPoints;
    int actualWaypoint = 0;
    public int nextWaypoint;
    Vector3 posOnPath;



    #endregion

    #region VFX

    [Header("Visual Effects")]
    public TrailRenderer trailLeft, trailRight;
    public GameObject LeftSmoke, RightSmoke;
    bool enableSmoke;
    #endregion

    public CameraController Camera;


    void Awake()
    {
        //Get container
        container = vehicleModel.GetChild(0);
        containerBase = container.localPosition;
    }

    private void Start()
    {
        screenWidth = Screen.width;

        transform.position = sphere.transform.position + new Vector3(0, 0.35f, 0f);
        sphereCollider = sphere.GetComponent<Collider>();
        currentSteering = steering;



        //PathManagher
        pathManager = GameObject.FindObjectOfType<PathManager>();
        PathPoints = pathManager.WayPoints;
        nextWaypoint = actualWaypoint;

    }



    void Update()
    {
        transform.position = sphere.transform.position;

        #region Mobile Control
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                HorizontalInput += touch.deltaPosition.x / (screenWidth / 2);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                HorizontalInput = 0;
            }
        }
        #endregion
  
        #region Steering

        //Steering
        ControlSteer(HorizontalInput);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y + rotate, 0)), Time.deltaTime);
        rotateTarget = Mathf.Lerp(rotateTarget, rotate, Time.deltaTime * 10f); rotate = 0;

        //vehicle tilt
        float tilt = 0.0f;
        container.localPosition = containerBase + new Vector3(0, Mathf.Abs(tilt) / 2000, 0);
        container.localRotation = Quaternion.Slerp(container.localRotation, Quaternion.Euler(0, rotateTarget / 8, tilt), Time.deltaTime * 10.0f);

        #endregion

        if (onGround)
        {
            float dist = Vector3.Distance(transform.position, PathPoints[nextWaypoint]);
            if (dist < 2f)
            {
                //Change waypoint 
                if (nextWaypoint < PathPoints.Count - 1)
                {
                    actualWaypoint = nextWaypoint;
                    nextWaypoint++;
                }

            }

            //if angle between vehiclemodel.forward an nextwaypoint  >  150 turn player to right direction
            float angle = Vector3.Angle(PathPoints[nextWaypoint] - transform.position, vehicleModel.forward);
            if (angle > 150)
            {
                StartCoroutine(TurnPlayer());
            }
        }

        //Draw Line to Next Waypoint
        Debug.DrawLine(transform.position, PathPoints[nextWaypoint], Color.green);

    }

    void FixedUpdate()
    {
        #region  Normal Physics 
        //Rotate Front Wheel
        front_wheel.Rotate(new Vector3(CurrentSpeed, 0, 0));


        RaycastHit hitOn;
        RaycastHit hitNear;
        RaycastHit hitNorm;

        // ground check
        onGround = Physics.Raycast(transform.position, Vector3.down, out hitOn, 0.3f, layerMask: RoadLayer);
        nearGround = Physics.Raycast(transform.position, Vector3.down, out hitNear, 2.0f);
        Physics.Raycast(transform.position, Vector3.down, out hitNorm, 3f);

        //Draw Lines
        Debug.DrawRay(transform.position, Vector3.down * 0.2f, Color.blue);
        Debug.DrawRay(vehicleModel.position, vehicleModel.forward * 0.1f, Color.yellow);


        //Rotate according ground normal
        // Normal
        vehicleModel.up = Vector3.Lerp(vehicleModel.up, hitNear.normal, Time.deltaTime * 1.0f);
        vehicleModel.Rotate(0, transform.eulerAngles.y, 0);

        #endregion

        #region OnGround Physics

        if (onGround)
        {
            //Move PLayer
            sphere.AddForce(vehicleModel.forward * CurrentSpeed * 100 * Time.deltaTime, ForceMode.Acceleration);
            #region SafePositions
            //Add Safe last positions in array when player is onground
            if (!HadLost)
            {
                //when array has more than 150 elements  delete from start
                if (LastPositions.Count > 150)
                {
                    LastPositions.RemoveAt(0);
                }
                LastPositions.Add(transform.position);
            }
            #endregion

            #region Drifting 

            if ((HorizontalInput > 0.5f || HorizontalInput < -0.5f) && CurrentSpeed > 0)
            {
                IsDrifting = true;
                currentSteering = steering + 10f;
            }
            else
            {
                IsDrifting = false;
                currentSteering = steering;
            }

            //enable skid marks when drifitng
            trailLeft.emitting = IsDrifting;
            trailRight.emitting = IsDrifting;

            #region  smoke
            enableSmoke = (HorizontalInput > 0.9f || HorizontalInput < -0.9f) && CurrentSpeed > 0;

            LeftSmoke.SetActive(enableSmoke);
            RightSmoke.SetActive(enableSmoke);
            #endregion
            #endregion
        }
        else
        {
            sphere.AddForce(vehicleModel.forward * CurrentSpeed * 150 * Time.deltaTime, ForceMode.Acceleration);
            sphere.AddForce(Vector3.down * gravity * Time.deltaTime * 10, ForceMode.Acceleration);


            //Disabele effects in air
            trailLeft.emitting = false;
            trailRight.emitting = false;

            LeftSmoke.SetActive(false);
            RightSmoke.SetActive(false);
        }

        #endregion

        //Player Landed
        if (HasJumped && onGround)
        {
            HasJumped = false;
            Landed();
        }
    }

    #region Player Teleport To Road
    //Call When player hits Ground
    public void Lose()
    {
        //When player hits ground 
        HadLost = true;
        SetPlayerActive(false);
        //teleport back to road
        StartCoroutine(TeleportPlayerSafeLocation());

    }


    IEnumerator TeleportPlayerSafeLocation()
    {
        yield return new WaitForSeconds(0.1f);
        //teleport back and lerp speed
        sphere.transform.position = LastPositions[0];
        SetPlayerActive(true);


        CurrentSpeed = 0;
        HadLost = false;
        Camera.CamTeleportBehaviour();

        //Find nearest waypoint and set to  nextWaypoint
        FindNearestWaypoint();
        //lerp player speed from 0
        StartCoroutine(LerpPlayerSpeed());
    }

    // Call After Hitting Ground
    IEnumerator LerpPlayerSpeed()
    {
        while (CurrentSpeed < Speed)
        {
            CurrentSpeed += 1f;
            yield return null;
        }
        //Make sure CurrentSpeed = Speed
        CurrentSpeed = Speed;
        yield return null;
    }
    #endregion

    #region Controls
    public void ControlSteer(float direction)
    {
        rotate = currentSteering * direction;
    }
    public void ControlJump()
    {
        gravity /= 3;
        Debug.Log("jump");
        sphere.AddForce((vehicleModel.up) * (jumpForce * 5), ForceMode.Impulse);
    }

    void MobileControl()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                hInput += touch.deltaPosition.x / (screenWidth / 2);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                hInput = 0;
            }
        }

        HorizontalInput = hInput;
    }


    #endregion

    #region Landing
    void Landed()
    {
        gravity *= 3;
        Debug.Log("Landed");
        FindNearestWaypoint();

    }
    void FindNearestWaypoint()
    {
        var sortedArr = PathPoints.OrderBy(p => Vector3.Distance(transform.position, p)).ToArray();
        var relativePos = transform.InverseTransformPoint(sortedArr[0]);


        //Find Nearest Waypoint and chek realtive z
        if (relativePos.z > 0)
        {
            nextWaypoint = PathPoints.IndexOf(sortedArr[0]);
        }
        else
        {
            nextWaypoint = (PathPoints.IndexOf(sortedArr[0]) + 1);
        }


        Debug.DrawLine(transform.position, PathPoints[nextWaypoint], Color.yellow, 200);

        IsLanded = true;
    }

    #endregion

    public void SetPlayerActive(bool activeStatus)
    {
        if (activeStatus)
            CurrentSpeed = Speed;
        else
            CurrentSpeed = 0;

    }

    IEnumerator TurnPlayer()
    {
        //turn player right direction 
        var lookPos = PathPoints[nextWaypoint] - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos).normalized;
        float angle = Quaternion.Angle(transform.rotation, rotation);

        while (angle > 10)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
            angle = Quaternion.Angle(transform.rotation, rotation);
            yield return null;
        }


        yield return null;
    }
}