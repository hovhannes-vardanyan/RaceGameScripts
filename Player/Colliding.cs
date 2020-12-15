using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliding : MonoBehaviour
{
    Vehicle Player;

    void Start()
    {
        Player = GetComponent<Vehicle>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Get parent and find its first child object which  contains Bot Component
            Bot bot = other.transform.parent.GetChild(0).GetComponent<Bot>();
            bot.KickOut();
            Debug.Log("Enemy Founded");
        }

        //collision with ground
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!Player.HadLost)
            {
                Player.Lose();
                Debug.Log("Touched ground");
            }
        }
    }
}
