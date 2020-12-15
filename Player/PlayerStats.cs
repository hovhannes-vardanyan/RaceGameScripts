using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CameraController cameraController;

    [Header("Player Stats")]
    public int CoinsCount = 0;


    [HideInInspector]
    public bool IsBoosted;
    public SteeringAnimator steeringAnimator;


    UIManager uiManager;
    public event EventHandler OnCoinTaken;



    PathManager pathManager;

    void Start()
    {
        uiManager = GameObject.FindObjectOfType<UIManager>();
        steeringAnimator = GameObject.FindObjectOfType<SteeringAnimator>();
        cameraController = GameObject.FindObjectOfType<CameraController>();
    }


    public void AddCoin()
    {
        //Raise an ivent to change Coins count on Ui mangment
        CoinsCount++;
        OnCoinTaken?.Invoke(this, EventArgs.Empty);
    }

    public void SetIsBoosted(bool boostStatus)
    {
        IsBoosted = boostStatus;
        steeringAnimator.SetFlamesActive(boostStatus);
    }



}
