using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPoint : MonoBehaviour
{
    public Vehicle Player;
    public LayerMask RoadLayer;

    [Header("Special Effects")]
    public GameObject HitEffect;
    public ParticleSystem DustEffect;




    void Update()
    {
        RaycastHit HitFront;
        bool b = Physics.Raycast(transform.position, transform.forward, out HitFront, maxDistance: 0.1f, RoadLayer);

        // Debug.DrawLine(transform.position, HitFront.point, color: Color.red);
        Debug.DrawRay(transform.position, transform.forward * 0.1f, Color.white);
        if (b)
        {
            Debug.Log("HitFence");
            Player.sphere.AddForce(-Player.vehicleModel.transform.forward * 50, ForceMode.Force);
            Instantiate(HitEffect, transform.position, Quaternion.identity);
        }





    }

}
