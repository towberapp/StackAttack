using UnityEngine;


[CreateAssetMenu(fileName = "BonusTypeSO", menuName = "Custom/BonusTypeSO")]
public class BonusTypeSO : ScriptableObject
{
    // Добавьте поля данных, которые вы хотите хранить в скриптаблобджекте
    public string objectName;
    public Sprite mainSprite;
    public Sprite iconSprite;

}