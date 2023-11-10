using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsController : MonoBehaviour
{
    // GameStatus
    public static UnityEvent GameOverEvent = new();
    public static UnityEvent StartEvent = new();
    public static UnityEvent<int> PreStartEvent = new();


    public static UnityEvent NextLevelEvent = new();
    public static UnityEvent UpgradeGridEvent = new();

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

    private void OnDestroy()
    {
        GameOverEvent.RemoveAllListeners();
        StartEvent.RemoveAllListeners();
        NextLevelEvent.RemoveAllListeners();
        PreStartEvent.RemoveAllListeners();
        UpgradeGridEvent.RemoveAllListeners();
        moveCubeEvent.RemoveAllListeners();
        CheckForBrakeEvent.RemoveAllListeners();

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
