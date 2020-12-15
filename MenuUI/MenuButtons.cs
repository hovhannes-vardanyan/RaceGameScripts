using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    //Loading Bar
    public Slider LoadingSlider;

    private void Start()
    {
        StartButton();
    }

    public void StartButton()
    {
        StartCoroutine(LoadLevelAsync(1));
    }

    //Level loading
    IEnumerator LoadLevelAsync(int index)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("Scene1");
        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress/0.9f);
            LoadingSlider.value = progress;
            yield return null;
        }
    }

    //Set quality level
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //Quit The Game
    public void QuitButton()
    {
        Application.Quit();
    }
}
