using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    PlayerStats PlayerStats;
    private Vector3 RotationVector;
    public float RotationSpeed = 1f;




    void Start()
    {
        PlayerStats = GameObject.FindObjectOfType<PlayerStats>();
        RotationVector = new Vector3(0, 15, 0);
    }

    void Update()
    {
        //Rotate coin
        transform.Rotate(RotationVector * RotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats.AddCoin();
            Destroy(gameObject);
        }
    }
}
