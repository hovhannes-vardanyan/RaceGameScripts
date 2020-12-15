using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    PlayerStats PlayerStats;
    Vehicle Player;
    private float TimeCount;




    [Header("Player Stats UI")]
    public Text CoinsCountText;
    public float CountDown;

    [Header("UI Panels")]
    public GameObject VictoryMenu;
    public GameObject RestartMenu;


    [Header("UI Text")]
    public Text CountDownText;
    public Text TimerText;

    public event EventHandler OnCountdownOver;



    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        Player = GameObject.FindObjectOfType<Vehicle>();
        PlayerStats = GameObject.FindObjectOfType<PlayerStats>();
        PlayerStats.OnCoinTaken += UIManager_OnCoinTaken;
        StartCoroutine(CountDownTimer());
    }

    //Change CoinsCount value when player takes Coin
    private void UIManager_OnCoinTaken(object sender, EventArgs e)
    {
        CoinsCountText.text = "Coins:" + PlayerStats.CoinsCount.ToString();
    }

    void Update()
    {
        // Timer
        TimerText.text = $"Time {String.Format("{0:00.0}", (TimeCount += Time.deltaTime))}";
    }

    //Start countdown
    IEnumerator CountDownTimer()
    {
        yield return new WaitForSeconds(CountDown);
        OnCountdownOver.Invoke(this.gameObject, EventArgs.Empty);

    }


    public void SetActiveVictoryMenu()
    {
        //set active restart menu
        VictoryMenu.SetActive(true);
        gameManager.SetPlayerActive(false);
    }
    public void SetActiveRestartMenu()
    {
        //set active restart menu
        RestartMenu.SetActive(true);
        gameManager.SetPlayerActive(false);
    }

    #region Buttons Functions
    public void RestartButton()
    {
        Debug.Log("Restart");
        Time.timeScale = 1f;
        SceneManager.LoadScene($"Scene1");
        Player.joystick.gameObject.SetActive(true);
    }


    #endregion
}
