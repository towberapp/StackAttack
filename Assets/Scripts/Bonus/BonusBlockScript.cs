using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class BonusBlockScript : MonoBehaviour
{
    [SerializeField] private GameObject fxBonus;

    public BonusTypeSO bonusType;

    private IndividualBlockControl individualBlockControl;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        individualBlockControl = GetComponent<IndividualBlockControl>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // взять рандомно bonus
        bonusType = BonusController.GetBounsSO();

        // если по какой-то причине список будет пустой
        if (bonusType == null)
        {
            if (individualBlockControl != null)
                individualBlockControl.DestroyCube();
            return;
        }
        //

        spriteRenderer.sprite = bonusType.mainSprite;

    }

    public void TryTouchBonusBlock()
    {
        BonusController.onTakeBonus.Invoke(bonusType);
        Instantiate(fxBonus, transform.position, transform.rotation);        
    }

}
