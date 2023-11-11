using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BonusController : MonoBehaviour
{
    /*[Header("Read Only")]
    [SerializeField] private BonusTypeSO bonusTypeViewer;
    [SerializeField] private Status bonusStatusViewer = Status.empty;
    [SerializeField] private ActivatedStatus activatedBonusViewer = ActivatedStatus.empty;*/

    [SerializeField] private GameObject boomBoxObj;

    [Header("BonusList")]
    [SerializeField] private List<BonusTypeSO> listOfBonusSerialized;
    

    //
    public enum Status
    {
        empty,
        taken        
    }

    public enum ActivatedStatus
    {
        empty,
        active
    }

    public static Status bonusStatus = Status.empty;
    public static ActivatedStatus activatedBonus = ActivatedStatus.empty;


    // храним СО бонуса
    public static BonusTypeSO bonusType;
    public static BonusTypeSO activatedBonusType;

    // static boombox
    public static GameObject boomBoxStatic;

    public static List<BonusTypeSO> listOfBonus;

    //события
    public static UnityEvent<BonusTypeSO> onTakeBonus = new();
    public static UnityEvent<BonusTypeSO> onActiveBonus = new();
    public static UnityEvent onUseBonus = new();

    //события бонусов
    public static UnityEvent bonusTimeEvent = new();
    public static UnityEvent bonusJumpEvent = new();
    public static UnityEvent bonusMagetEvent = new();
    public static UnityEvent bonusPowerEvent = new();
    public static UnityEvent bonusDeadSpaceEvent = new();


    public static BonusTypeSO GetBounsSO()
    {
        if (listOfBonus.Count > 0)
        {
            int rnd = Random.Range(0, listOfBonus.Count);
            return listOfBonus[rnd];
        }
        else
        {
            Debug.LogWarning("The list of bonuses is empty.");
            return null; // Или другое значение по умолчанию в случае пустого списка
        }
    }


    private void Awake()
    {
        onTakeBonus.AddListener(OnTakeBonus);
        onActiveBonus.AddListener(OnActiveBonus);
        onUseBonus.AddListener(OnUsedBonus);

        EventsController.StartEvent.AddListener(OnStartEvent);

        listOfBonus = listOfBonusSerialized;
        boomBoxStatic = boomBoxObj;
    }

    private void OnStartEvent()
    {
        Debug.Log("ON START EVENT");

        bonusType = null;
        bonusStatus = Status.empty;
        //bonusStatusViewer = Status.empty;
        activatedBonus = ActivatedStatus.empty;
        //activatedBonusViewer = ActivatedStatus.empty;
        //bonusTypeViewer = null;
    }


    public void CheckActivatedBonus(BonusTypeSO bonus)
    {
        if (bonus.name == "Time")
            bonusTimeEvent.Invoke();

    }

    private void OnUsedBonus()
    {
        activatedBonus = ActivatedStatus.empty;
        //activatedBonusViewer = ActivatedStatus.empty;
        activatedBonusType = null;
    }


    private void OnActiveBonus(BonusTypeSO arg0)
    {
        Debug.Log("OnActiveBonus: " + arg0);

        bonusType = null;
        bonusStatus = Status.empty;
        //bonusStatusViewer = Status.empty;
        //bonusTypeViewer = null;

        //
        activatedBonus = ActivatedStatus.active;
        //activatedBonusViewer = ActivatedStatus.active;
        activatedBonusType = arg0;

        CheckActivatedBonus(arg0);
    }

    private void OnTakeBonus(BonusTypeSO arg0)
    {
        Debug.Log("OnTakeBonus: " + arg0);

        bonusType = arg0;
        bonusStatus = Status.taken;
        //bonusStatusViewer = Status.taken;
        //bonusTypeViewer = arg0;
    }

}
