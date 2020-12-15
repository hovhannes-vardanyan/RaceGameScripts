using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [Header("Boost Parameters")]
    [Range(0, 10)]
    public float BoostTime = 5f;
    [Range(0, 10)]
    public float BoostCount;

    [HideInInspector]
    public PlayerStats playerStats;


    Vehicle Player;
    void Start()
    {
        Player = GameObject.FindObjectOfType<Vehicle>();
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        BoostCount = Player.CurrentSpeed * 0.2f;
    }
    void Update()
    {
        //animate booster object
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime * 5);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !playerStats.IsBoosted)
        {
            Player.CurrentSpeed += BoostCount;
            playerStats.SetIsBoosted(true);
            StartCoroutine(Accelerate());
        }
    }

    IEnumerator Accelerate()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(BoostTime);
        Player.CurrentSpeed -= BoostCount;
        playerStats.SetIsBoosted(false);
        Destroy(gameObject);
    }
}
