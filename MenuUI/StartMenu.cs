using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject StartPanel;

    
    void Start()
    {
        Time.timeScale = 0.0f;
    }

  
    public void QuitButton()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        StartPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
