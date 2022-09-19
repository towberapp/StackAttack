using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int row = 9;


    [Header("Game Object Control")]
    [SerializeField] private GameObject leftMenuBtnObj; //+
    [SerializeField] private GameObject mainMenuObj;  //+
    [SerializeField] private GameObject resumeBtnObj; //+
    [SerializeField] private GameObject restartBtnObj; //+
    [SerializeField] private GameObject startGameBtnObj; //+
    [SerializeField] private GameObject gameOverMenuObj; //+
    [SerializeField] private GameObject difficulSelectObj; //+

    [Header("Element Control")]
    [SerializeField] private Button leftMenuBtn; //+
    [SerializeField] private Button newGameBtn;   // +
    [SerializeField] private Button reStartGameBtn;   // +
    [SerializeField] private Button resumeGameBtn; //+
    [SerializeField] private Button exitGameBtn; //+

    [Header("Text UI Element")]
    [SerializeField] private TMP_Text levelTextTopMenu; //+    
    [SerializeField] private TMP_Text levelRecordTextMainMenu;


    private void Awake()
    {
        EventsController.NextLevelEvent.AddListener(OnLevelChange);
        EventsController.GameOverEvent.AddListener(OnGameOver);
        EventsController.StartEvent.AddListener(OnStartGame);

        newGameBtn.onClick.AddListener(OnNewGame);
        leftMenuBtn.onClick.AddListener(OnLeftMenuClick);
        resumeGameBtn.onClick.AddListener(OnResumeClick);
        exitGameBtn.onClick.AddListener(OnExitClick);
        reStartGameBtn.onClick.AddListener(RestartGame);

        Debug.Log("AWAKE GAME");

        SystemStatic.isStartGame = false;
        SystemStatic.isGameOver = false;
        SystemStatic.isGamePaused = false;
        SystemStatic.level = 0;
    }

    private void Start()
    {        
        SetPauseGame(true);
        mainMenuObj.SetActive(true);
        leftMenuBtnObj.SetActive(false);        

        restartBtnObj.SetActive(false);
        resumeBtnObj.SetActive(false);
        startGameBtnObj.SetActive(true);
        //difficulSelectObj.SetActive(true);

        gameOverMenuObj.SetActive(false);
        LoadLevelRecord();
    }

    private void LoadLevelRecord()
    {
        levelRecordTextMainMenu.text = SystemStatic.levelRecord.ToString();
    }

    private void OnNewGame()
    {
        EventsController.PreStartEvent.Invoke(row);
    }

    private void OnStartGame()
    {
        SystemStatic.isStartGame = true;
        SetPauseGame(false);
        mainMenuObj.SetActive(false);
        leftMenuBtnObj.SetActive(true);
    }


    private void OnResumeClick()
    {
        leftMenuBtnObj.SetActive(true);
        mainMenuObj.SetActive(false);
        SetPauseGame(false);
    }

    private void OnLeftMenuClick()
    {
        mainMenuObj.SetActive(true);
        leftMenuBtnObj.SetActive(false);
        resumeBtnObj.SetActive(true);
        restartBtnObj.SetActive(true);
        startGameBtnObj.SetActive(false);
        gameOverMenuObj.SetActive(false);
        //difficulSelectObj.SetActive(false);
        SetPauseGame(true);
    }

    private void OnGameOver()
    {
        SystemStatic.isGameOver = true;

        SetPauseGame(true);
        mainMenuObj.SetActive(true);
        leftMenuBtnObj.SetActive(false);

        restartBtnObj.SetActive(true);
        startGameBtnObj.SetActive(false);
        resumeBtnObj.SetActive(false);
        gameOverMenuObj.SetActive(true);
        //difficulSelectObj.SetActive(false);
    }


    private void OnLevelChange()
    {
        levelTextTopMenu.text = SystemStatic.level.ToString();
    }

    private void OnExitClick()
    {
        Application.Quit();        
    }

    public void RestartGame()
    {
        RestartScene();
    }

    private void SetPauseGame(bool status)
    {
        if (status)
        {
            Time.timeScale = 0;
            SystemStatic.isGamePaused = true;
        }
        else
        {
            Time.timeScale = 1;
            SystemStatic.isGamePaused = false;
        }
    }


    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        newGameBtn.onClick.RemoveListener(OnNewGame);
        leftMenuBtn.onClick.RemoveListener(OnLeftMenuClick);
        resumeGameBtn.onClick.RemoveListener(OnResumeClick);
        exitGameBtn.onClick.RemoveListener(OnExitClick);
        reStartGameBtn.onClick.RemoveListener(RestartGame);
    }
}
