using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdMobInterstitial : MonoBehaviour
{
    [Header("Межстраничное")]
    [SerializeField] private string _basicPage = "ca-app-pub-1859015684244712/1542999610"; // межстраничное

    [Header("Межстраничное Демо кей")]
    [SerializeField] private string _basicPageDemo = "ca-app-pub-3940256099942544/1033173712"; // межстраничное

    private InterstitialAd _interstitialAd;

    private void Awake()
    {
        EventsController.NextLevelEvent.AddListener(OnChangeLevel);
    }

    private void OnChangeLevel()
    {
        // Проверяем кратность числа 5
        if (IsMultipleOf5(SystemStatic.level) && SystemStatic.level > 3)
        {
            ShowInterstitialAd();
        }
   
    }

    bool IsMultipleOf5(int number)
    {
        // Используем оператор деления по модулю (%)
        return number % 5 == 0;
    }

    private void Start()
    {
        MobileAds.Initialize(initStatus => {
            LoadLoadInterstitialAd();
        });

    }

    public void LoadLoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        string adsInterstisialId;
        if (Application.isEditor)
            adsInterstisialId = _basicPageDemo;
        else
            adsInterstisialId = _basicPage;

        // send the request to load the ad.
        InterstitialAd.Load(adsInterstisialId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;

                RegisterEventHandlers(_interstitialAd);
            });
    }


    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();

            EventsController.interstisialDone.Invoke();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }



    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {            
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            EventsController.interstisialClick.Invoke();
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            LoadLoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadLoadInterstitialAd();
        };
    }
}
