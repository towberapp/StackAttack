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

    private bool isPause;

    public void RestartGame()
    {
        EventsController.GameOverEvent.Invoke();
        RestartSceneInvoke();
    }

    public void PauseGame()
    {
        SetPauseGame(!isPause);
    }

    //private

    private void Awake()
    {
        restart.onClick.AddListener(RestartGame);
        
        intervalSlider.onValueChanged.AddListener(OnIntervalChange);
        speedSlider.onValueChanged.AddListener(OnSpeedChange);
        
        EventsController.GameOverEvent.AddListener(OnGameOver);
        EventsController.StartEvent.AddListener(OnStart);

        Debug.Log("AWAKE SYSTEM UI");
    }

    private void Start()
    {
        SetPauseGame(true);
    }

    private void OnStart()
    {
        SetPauseGame(false);
    }

    private void SetPauseGame (bool status)
    {
        if (status)
        {
            isPause = true;
            Time.timeScale = 0;
        } else
        {
            isPause = false;
            Time.timeScale = 1;
        }
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
        //Invoke(nameof(RestartSceneInvoke), 1.0f);        
    }


    private void RestartSceneInvoke()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
