using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusUIButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private GameObject buttonGO;

    BonusTypeSO bonusType;

    private void Awake()
    {
        BonusController.onTakeBonus.AddListener(OnTakeBonus);
        button.interactable = false;
        buttonGO.SetActive(false);
    }

    private void OnTakeBonus(BonusTypeSO bonus)
    {
        buttonGO.SetActive(true);
        button.interactable = true;

        image.enabled = true;
        image.sprite = bonus.iconSprite;
        bonusType = bonus;
    }

    public void ClickOnBonus()
    {
        if (BonusController.bonusStatus != BonusController.Status.taken)
        {
            Debug.Log("Бонуса нет, кликать не нужно");
            return;
        }

 /*       if (BonusController.activatedBonus != BonusController.ActivatedStatus.empty)
        {
            Debug.Log("Старый бонус ещё не использован: " + BonusController.activatedBonus);
            return;
        }*/

        BonusController.onActiveBonus.Invoke(bonusType);
        image.enabled = false;
        image.sprite = null;
        bonusType = null;
        button.interactable = false;
        buttonGO.SetActive(false);
    }
}
