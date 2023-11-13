using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("System")]
    [SerializeField] private AdMobManager adMobManager;

    [Header("UI")]
    [SerializeField] private GameObject PreGameOverUI;

    [SerializeField] private Button showAdsButton;    


    private void Awake()
    {
        EventsController.BeforeGameOverEvent.AddListener(BeforeGameOver);
        PreGameOverUI.SetActive(false);

        //EventsController.revarderFail.AddListener(OnFailAd);
        EventsController.revarderDone.AddListener(OnDoneAd);
        //EventsController.revarderClose.AddListener(OnCloseAd);
    }

    private void OnDoneAd()
    {
        Debug.Log("Restore Player ON DONE AD");

        EventsController.SetPauseGameEvent.Invoke(false);        
        EventsController.playerIdleAnimationEvent.Invoke();
        EventsController.RestorePlayer.Invoke();
    }

    private void BeforeGameOver()
    {        
        EventsController.SetPauseGameEvent.Invoke(true);

        // checkads
        bool checkAds = adMobManager.CheckLoadedAds();
        if (checkAds && MainConfig.playerY <= 7)
        {
            PreGameOverUI.SetActive(true);
            showAdsButton.interactable = true;
        }
        else
        {
            EventsController.SetPauseGameEvent.Invoke(false);
            EventsController.GameOverEvent.Invoke();
            showAdsButton.interactable = false;
        }            
        
    }

    public void ClickOnAdsButton()
    {
        // showAds        
        adMobManager.ShowRewardedAd();
        PreGameOverUI.SetActive(false);

        // когда закончится реклама 
    }

    public void ClickOnNewGameButton()
    {
        PreGameOverUI.SetActive(false);
        EventsController.SetPauseGameEvent.Invoke(false);
        EventsController.GameOverEvent.Invoke();
    }
}
