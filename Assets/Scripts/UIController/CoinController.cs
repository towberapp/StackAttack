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
        SystemStatic.coin = playerpref.GetCoin();
    }
    private void Start()
    {
        coinTextTopMenu.text = SystemStatic.coin.ToString();        
    }

    private void OnLevelUp()
    {        
        SystemStatic.coin += 5;
        playerpref.UpdateCoin();
        StartCoroutine(CointVisual(5));
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


}
