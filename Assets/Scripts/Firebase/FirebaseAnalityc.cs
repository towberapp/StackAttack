using System;
using UnityEngine;
using Firebase.Analytics;

public class FirebaseAnalityc : MonoBehaviour
{


    private void Awake()
    {
        EventsController.GameOverEvent.AddListener(OnGameOver);        
        EventsController.StartEvent.AddListener(OnStart);

        EventsController.NextLevelEvent.AddListener(OnNextLevel);
        EventsController.updateCoinEvent.AddListener(OnUpdateCoin);
        EventsController.updateLevelRecord.AddListener(OnLevelRecord);
        EventsController.countGameEvent.AddListener(OnCountGame);

        // ads
        EventsController.loadAdSuccesEvent.AddListener(OnLoadAd);
        EventsController.revarderDone.AddListener(OnRevardedDone);
        EventsController.revarderClick.AddListener(OnrevarderClick);
        EventsController.revarderFail.AddListener(OnrevarderFail);
        EventsController.revarderClose.AddListener(OnrevarderClose);
        EventsController.revarderShow.AddListener(OnShow);

        //interstisial
        EventsController.interstisialDone.AddListener(OnDoneInterstisial);
        EventsController.interstisialClick.AddListener(OnClickInterstisial);        
    }


    private void OnClickInterstisial()
    {
        FirebaseAnalytics.LogEvent(
            "AdMob",
            "Interstisial",
            "Click"
        );
    }

    private void OnDoneInterstisial()
    {
        FirebaseAnalytics.LogEvent(
            "AdMob",
            "Interstisial",
            "Done"
        );
    }

    private void OnShow()
    {
        FirebaseAnalytics.LogEvent(
            "AdMob",
            "Revarded",
            "Show"
        );
    }

    private void OnrevarderClose()
    {
        FirebaseAnalytics.LogEvent(
            "AdMob",
            "Revarded",
            "Close"
        );
    }

    private void OnrevarderFail()
    {
        FirebaseAnalytics.LogEvent(
            "AdMob",
            "Revarded",
            "Fail"
        );
    }

    private void OnrevarderClick()
    {
        FirebaseAnalytics.LogEvent(
            "AdMob",
            "Revarded",
            "Click"
        );
    }

    private void OnRevardedDone()
    {
        FirebaseAnalytics.LogEvent(
            "AdMob",
            "Revarded",
            "Done"
        );
    }

    private void OnLoadAd(bool arg0)
    {
        if (arg0)
            FirebaseAnalytics.LogEvent(
                "AdMob",
                "Revarded",
                "LoadSuccess"
            );
        else
            FirebaseAnalytics.LogEvent(
                 "AdMob",
                 "Revarded",
                 "LoadError"
             );
    }

    private void OnCountGame(int count)
    {
        //Debug.Log("Analytic -> OnCountGame: " + count);

        FirebaseAnalytics.LogEvent(
            "CountGame",
            "Count",
            count
        );
    }

    private void OnLevelRecord(int levelRecord)
    {
        Debug.Log("Analytic -> OnLevelRecord: " + levelRecord);

        FirebaseAnalytics.LogEvent(
            "LevelRecord",
            "Level",
            levelRecord
        );
    }

    private void OnUpdateCoin(int coin)
    {
        //Debug.Log("Analytic -> OnUpdateCoin: " + coin);

        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventPostScore,
            FirebaseAnalytics.ParameterScore,
            coin
        );
    }

    private void OnNextLevel()
    {
        //Debug.Log("Analytic -> OnNextLevel: " + SystemStatic.level);

        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventLevelUp,
            FirebaseAnalytics.ParameterLevel,
            SystemStatic.level
        );
    }

    private void OnStart()
    {
        //Debug.Log("Analytic -> OnStart");
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);
    }

    private void OnGameOver()
    {
        //Debug.Log("Analytic -> OnGameOver");
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd);
    }

    private void OnDestroy()
    {
        EventsController.GameOverEvent.RemoveAllListeners();
        EventsController.StartEvent.RemoveAllListeners();

        EventsController.NextLevelEvent.RemoveAllListeners();
        EventsController.updateCoinEvent.RemoveAllListeners();
        EventsController.updateLevelRecord.RemoveAllListeners();
        EventsController.countGameEvent.RemoveAllListeners();

        // ads
        EventsController.loadAdSuccesEvent.RemoveAllListeners();
        EventsController.revarderDone.RemoveAllListeners();
        EventsController.revarderClick.RemoveAllListeners();
        EventsController.revarderFail.RemoveAllListeners();
        EventsController.revarderClose.RemoveAllListeners();
    }
}
