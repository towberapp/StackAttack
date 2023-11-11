using UnityEngine;

public class BonusPlayerIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer icon;

    private void Awake()
    {
        icon.enabled = false;
        
        BonusController.onActiveBonus.AddListener(OnActiveBonus);
        BonusController.onUseBonus.AddListener(OnUseBonus);
    }

    private void OnActiveBonus(BonusTypeSO arg0)
    {
        icon.enabled = true;
        icon.sprite = arg0.mainSprite;
    }

    private void OnUseBonus()
    {
        icon.enabled = false;
        icon.sprite = null;
    }
}
