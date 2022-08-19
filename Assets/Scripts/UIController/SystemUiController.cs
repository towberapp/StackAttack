using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class SystemUiController : MonoBehaviour
{

    [SerializeField] private TMP_Text textSystem;
    [SerializeField] private Button restart;
    [SerializeField] private Slider intervalSlider;
    [SerializeField] private Slider speedSlider;

    private void Awake()
    {
        restart.onClick.AddListener(RestartGame);
        
        intervalSlider.onValueChanged.AddListener(OnIntervalChange);
        speedSlider.onValueChanged.AddListener(OnSpeedChange);

        
        EventsController.GameOverEvent.AddListener(OnGameOver);

        Debug.Log("AWAKE SYSTEM UI");

    }

    private void OnDestroy()
    {
        Debug.Log("DESTROY");
    }

    private void OnSpeedChange(float sliderSpeed)
    {        
        MainConfig.speedMove = 0.75f - (sliderSpeed/2);
    }

    private void OnIntervalChange(float sliderData)
    {
        float koefInterval = 5.0f - (sliderData + 1) * 2; // from 1 to 2;
        MainConfig.intervalCube = koefInterval;
    }

    private void OnGameOver()
    {
        textSystem.text = "GameOver!";
        Invoke(nameof(RestartSceneInvoke), 1.0f);        
    }


    public void RestartGame()
    {
        EventsController.GameOverEvent.Invoke();
    }

    private void RestartSceneInvoke()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
