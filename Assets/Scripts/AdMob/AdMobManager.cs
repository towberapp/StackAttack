using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobManager : MonoBehaviour
{    
    private RewardedAd _rewardedAd;

    //private string _adUnitId = "ca-app-pub-1859015684244712/1673516659";
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917"; // DEMO

    private void Start()
    {
        MobileAds.Initialize(initStatus => {
            LoadRewardedAd();
        });
               
    }

    public void LoadRewardedAd()
    {
        

        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");       

        var adRequest = new AdRequest();        

        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {                

                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    //EventsController.loadAdSuccesEvent.Invoke(false);

                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;

                //EventsController.loadAdSuccesEvent.Invoke(true);

                //return;

                RegisterEventHandlers(_rewardedAd);

            });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {            
            _rewardedAd.Show((Reward reward) =>
            {
                EventsController.revarderDone.Invoke();
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
        else
        {
            Debug.Log("Реклама не загружена и не готова к показу");
        }
    }


    public bool CheckLoadedAds()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
            return true;
        else

            return false;
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {        

        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        ad.OnAdImpressionRecorded += () =>
        {

            Debug.Log("Rewarded ad recorded an impression.");
        };

        ad.OnAdClicked += () =>
        {
            EventsController.revarderClick.Invoke();
            Debug.Log("Rewarded ad was clicked.");
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            EventsController.revarderShow.Invoke();
            Debug.Log("Rewarded ad full screen content opened.");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            EventsController.revarderClose.Invoke();
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            EventsController.revarderFail.Invoke();
            LoadRewardedAd();
        };
    }


    private void OnDestroy()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }

}
