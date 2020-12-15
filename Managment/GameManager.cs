using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System;

public class GameManager : MonoBehaviour
{
    PathManager pathManager;
    List<Vector3> PathPoints;


    [Header("Respawn Bot")]
    public Bot Bot;

    private List<Bot> Bots;
    private Vehicle Player;
    private UIManager uIManager;

    float waitTime = 5;

    [Header("Extra GameObject")]
    public GameObject Tramplin;
    public GameObject Booster;
    public GameObject Coin;


    [Header("Extra's Count")]
    public int BoosterCount;
    public int EnemyCount;                    
    public int TramplinCount;


    private void Start()
    {
        //Set FrameRate 60
        Application.targetFrameRate = 300;

        uIManager = GameObject.FindObjectOfType<UIManager>();
        //event when countdown is Over
        uIManager.OnCountdownOver += UIManager_OnCountdownOver;
        Player = GameObject.FindObjectOfType<Vehicle>();
        pathManager = GameObject.FindObjectOfType<PathManager>();
        PathPoints = pathManager.WayPoints;

        //Find bots set their path and activate when countdown is over
        GetAllBots();
        SetRandomSpeedForAllBots();
        SetPlayerActive(false);
        SetAllBotsActive(false);
        SetRandomTurnDistForAllBots();



        SpawnBoosters();
        //  SpawnTramplins();
        SpawnCoins();
    }

    private void Update()
    {
        //Teleport Enemys near to player
        if (Player.IsLanded)
        {
            Debug.Log("Teleport");
            if (Player.nextWaypoint > 4 && Player.nextWaypoint < pathManager.WayPoints.Count - 4)
            {
                foreach (var bot in Bots)
                {
                    int randNumber = UnityEngine.Random.Range(1, 4);
                    Vector3 newPos = pathManager.WayPoints[Player.nextWaypoint - randNumber] + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0.2f, 0);
                    StartCoroutine(TeleportBot(bot, newPos, randNumber));

                }
            }
            Player.IsLanded = false;
        }
    }

    IEnumerator TeleportBot(Bot bot, Vector3 pos, int randNumber)
    {
        yield return new WaitForSeconds(3f);
        if (bot.sphere != null && bot.currentIndex < Player.nextWaypoint)
        {
            bot.sphere.transform.position = pos;
            bot.currentIndex = Player.nextWaypoint - randNumber;
            bot.NextWayPoint = pathManager.WayPoints[Player.nextWaypoint - randNumber];
        }
    }



    // event when countdown is over
    private void UIManager_OnCountdownOver(object sender, EventArgs e)
    {
        SetPlayerActive(true);
        SetAllBotsActive(true);
    }

    public void SetPlayerActive(bool status)
    {
        Player.SetPlayerActive(status);
    }

    private void GetAllBots()
    {
        //Find All Bots and add to BotsList
        Bots = GameObject.FindObjectsOfType<Bot>().ToList();
    }

    public void SetAllBotsActive(bool status)
    {
        foreach (var bot in Bots)
        {
            bot.SetBotActive(status);
        }
    }

    //Set Random Speed to all bots at the start
    public void SetRandomSpeedForAllBots()
    {
        foreach (var bot in Bots)
        {
            //Set Random Speed
            bot.SetCurrentSpeed(UnityEngine.Random.Range(9, 12));
        }

    }
    public void SetRandomTurnDistForAllBots()
    {
        foreach (var bot in Bots)
        {
            //Set Random Speed
            bot.TurnDistance = (UnityEngine.Random.value * 1.5f) + 1f;
        }

    }

    //Change Bot Speed On Road might become usefull
    public void SetBotSpeed(Bot bot, float speed)
    {
        bot.SetCurrentSpeed(speed);
    }

    IEnumerator SetRandomSpeed_C()
    {
        yield return new WaitForSeconds(waitTime);
        SetRandomSpeedForAllBots();
        waitTime = UnityEngine.Random.Range(8, 15);
    }

    #region SpawnExtras
    public void SpawnCoins()
    {
        int prevIndex = 0;
        for (int i = 0; i < TramplinCount; i++)
        {
            int maxCombo = UnityEngine.Random.Range(3, 7);
            int currentIndex = UnityEngine.Random.Range(0, PathPoints.Count);
            while (currentIndex == prevIndex)
            {
                currentIndex = UnityEngine.Random.Range(0, PathPoints.Count);
            }

            for (int j = 0; j < maxCombo; j++)
            {
                Instantiate(Coin, PathPoints[currentIndex] + new Vector3(UnityEngine.Random.Range(0, 0.5f), 0.1f, UnityEngine.Random.Range(0, 0.5f)), transform.rotation);
            }
        }
    }
    public void SpawnTramplins()
    {
        int prevIndex = 0;
        for (int i = 0; i < TramplinCount; i++)
        {
            int currentIndex = UnityEngine.Random.Range(0, PathPoints.Count);
            while (currentIndex == prevIndex)
            {
                currentIndex = UnityEngine.Random.Range(0, PathPoints.Count);
            }

            Instantiate(Tramplin, PathPoints[currentIndex], Tramplin.transform.rotation);
        }
    }

    public void SpawnBoosters()
    {
        int prevIndex = 0;
        for (int i = 0; i < BoosterCount; i++)
        {
            int currentIndex = UnityEngine.Random.Range(0, PathPoints.Count);
            while (currentIndex == prevIndex)
            {
                currentIndex = UnityEngine.Random.Range(0, PathPoints.Count);
            }

            Instantiate(Booster, PathPoints[currentIndex] + new Vector3(0, 0.2f, 0), Quaternion.identity);
        }
    }
    #endregion

}
