using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishZone : MonoBehaviour
{
    Vehicle Player;
    UIManager uIManager;
    GameManager gameManager;
    void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        uIManager = GameObject.FindObjectOfType<UIManager>();
        Player = GameObject.FindObjectOfType<Vehicle>();
    }
    void OnTriggerEnter(Collider other)
    {
        //Stop player
        if (other.gameObject.CompareTag("Player"))
        {
            uIManager.SetActiveVictoryMenu();
            gameManager.SetPlayerActive(false);
        }

        //Stop Bots
        if (other.gameObject.CompareTag("Enemy"))
        {
            Bot bot = other.transform.parent.GetChild(0).GetComponent<Bot>();
            bot.SetBotActive(false);
        }
    }
}
