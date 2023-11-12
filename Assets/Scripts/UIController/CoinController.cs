using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private TMP_Text coinTextTopMenu;

    readonly PlayerPrefsController playerpref = new();

    private void Awake()
    {
        EventsController.NextLevelEvent.AddListener(OnLevelUp);
        EventsController.onTakeChip.AddListener(OnTakeChip);
        SystemStatic.coin = playerpref.GetCoin();
    }
    private void Start()
    {
        coinTextTopMenu.text = SystemStatic.coin.ToString();        
    }

    private void OnLevelUp()
    {
        UpdateCoin(1);
    }

    private void OnTakeChip()
    {
        UpdateCoin(5);
    }

    private void UpdateCoin (int coin)
    {
        SystemStatic.coin += coin;
        playerpref.UpdateCoin();
        StartCoroutine(CointVisual(coin));
        EventsController.updateCoinEvent.Invoke(SystemStatic.coin);
    }


    IEnumerator CointVisual(int coin)
    {
        for (int i = 0; i <= coin; i++)
        {
            yield return new WaitForSeconds(0.3f + i/5);
            coinTextTopMenu.text = (SystemStatic.coin-coin + i).ToString();
        }
        coinTextTopMenu.text = SystemStatic.coin.ToString();
    }


    private void OnDestroy()
    {
        EventsController.NextLevelEvent.RemoveListener(OnLevelUp);
        EventsController.onTakeChip.RemoveListener(OnTakeChip);
    }

}
