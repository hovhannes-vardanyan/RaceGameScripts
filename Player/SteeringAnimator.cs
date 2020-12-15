using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAnimator : MonoBehaviour
{
    private Vehicle Player;
    public Animator VehicleAnimator;
    public GameObject[] Flames;
    private float rotateTarget;
    private float rotate;

    private void Start()
    {
        Player = GameObject.FindObjectOfType<Vehicle>();
    }

    // Update is called once per frame
    void Update()
    {
        SteerAnimator();
    }


    void SteerAnimator()
    {
        //Lerp rotate 
        rotate = Player.HorizontalInput;
        rotateTarget = Mathf.Lerp(rotateTarget, rotate, Time.deltaTime * 10); rotate = 0;
        VehicleAnimator.SetFloat("HorizontalInput", rotateTarget);
    }


    //Set Flames Active When Player Picks Boost
    public void SetFlamesActive(bool flameStatus)
    {
        if (flameStatus)
        {
            foreach (var flame in Flames)
            {
                flame.SetActive(true);
            }
        }
        else
        {
            foreach (var flame in Flames)
            {
                flame.SetActive(false);
            }
        }
    }



}
