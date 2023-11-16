using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsController : MonoBehaviour
{
    // GameStatus
    public static UnityEvent BeforeGameOverEvent = new();
    public static UnityEvent StartBeforeGameOverEvent = new();
    public static UnityEvent GameOverEvent = new();
    public static UnityEvent StartEvent = new();
    public static UnityEvent<int> PreStartEvent = new();
    public static UnityEvent<bool> SetPauseGameEvent = new();
    public static UnityEvent RestorePlayer = new();
    //public static UnityEvent TestEvent = new();


    public static UnityEvent NextLevelEvent = new();
    public static UnityEvent UpgradeGridEvent = new();
    public static UnityEvent<int> updateCoinEvent = new();
    public static UnityEvent<int> updateLevelRecord = new();
    public static UnityEvent<int> countGameEvent = new();

    public static UnityEvent PlayerMove = new();

    public static UnityEvent<int, int, Vector2[]> moveCubeEvent = new();
    public static UnityEvent<Vector2> CheckForBrakeEvent = new();

    public static UnityEvent<GameObject, int> RunCran = new();
    public static UnityEvent<GameObject, int> DropCran = new();

    // player Animation
    public static UnityEvent playerRunAnimationEvent = new();
    public static UnityEvent playerPushAnimationEvent = new();
    public static UnityEvent playerIdleAnimationEvent = new();
    public static UnityEvent<int> playerDirectionEvent = new();
    public static UnityEvent<int> playerJumpAnimationEvent = new();
    public static UnityEvent playerDestroyAnimationEvent = new();
    public static UnityEvent playerBoomAnimationEvent = new();
    public static UnityEvent playerOffAnimationEvent = new();

    public static UnityEvent playerStopMoove = new();
    public static UnityEvent<bool> blockMoove = new();

    public static UnityEvent onTakeChip = new();

    // box
    public static UnityEvent boxDownFloor = new();

    // specislbox
    public static UnityEvent tntBlowUp = new();

    //ads
    public static UnityEvent<bool> loadAdSuccesEvent = new();
    public static UnityEvent revarderDone = new();
    public static UnityEvent revarderClick = new();
    public static UnityEvent revarderFail = new();
    public static UnityEvent revarderClose = new();
    public static UnityEvent revarderShow = new();

    private void Awake()
    {
        StartBeforeGameOverEvent.AddListener(OnBeforeDeth);
    }

    private void OnBeforeDeth()
    {
        Invoke("GameOver", 1.5f);
    }

    private void GameOver() => BeforeGameOverEvent.Invoke();


    private void OnDestroy()
    {
        GameOverEvent.RemoveAllListeners();
        StartEvent.RemoveAllListeners();
        NextLevelEvent.RemoveAllListeners();
        PreStartEvent.RemoveAllListeners();
        UpgradeGridEvent.RemoveAllListeners();
        moveCubeEvent.RemoveAllListeners();
        CheckForBrakeEvent.RemoveAllListeners();
        SetPauseGameEvent.RemoveAllListeners();

        playerRunAnimationEvent.RemoveAllListeners();
        playerPushAnimationEvent.RemoveAllListeners();
        playerIdleAnimationEvent.RemoveAllListeners();
        playerOffAnimationEvent.RemoveAllListeners();
        playerDestroyAnimationEvent.RemoveAllListeners();
        playerJumpAnimationEvent.RemoveAllListeners();

        playerDirectionEvent.RemoveAllListeners();
        PlayerMove.RemoveAllListeners();

    }
}
