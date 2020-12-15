using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingClouds : MonoBehaviour
{
    [Header("Cloud Movement Speed")]
    public float RotationSpeed;

    private Vector3 rotationVector;

    void Start()
    {
        rotationVector = new Vector3(0,15,0);
    }

    void Update()
    {
        transform.Rotate(rotationVector * RotationSpeed * Time.deltaTime);
    }
}
