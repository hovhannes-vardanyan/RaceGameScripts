using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTriggerZone : MonoBehaviour
{

    Vehicle Player;
    void Start()
    {
        Player = GameObject.FindObjectOfType<Vehicle>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.ControlJump();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.HasJumped = true;
        }
    }




}
