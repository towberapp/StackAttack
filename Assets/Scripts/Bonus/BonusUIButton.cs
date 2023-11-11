using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusUIButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    BonusTypeSO bonusType;

    private void Awake()
    {
        BonusController.onTakeBonus.AddListener(OnTakeBonus);
        button.interactable = false;
    }

    private void OnTakeBonus(BonusTypeSO bonus)
    {
        button.interactable = true;

        image.enabled = true;
        image.sprite = bonus.mainSprite;
        bonusType = bonus;
    }

    public void ClickOnBonus()
    {
        if (BonusController.bonusStatus != BonusController.Status.taken)
        {
            Debug.Log("������ ���, ������� �� �����");
            return;
        }

 /*       if (BonusController.activatedBonus != BonusController.ActivatedStatus.empty)
        {
            Debug.Log("������ ����� ��� �� �����������: " + BonusController.activatedBonus);
            return;
        }*/

        BonusController.onActiveBonus.Invoke(bonusType);
        image.enabled = false;
        image.sprite = null;
        bonusType = null;
        button.interactable = false;
    }
}