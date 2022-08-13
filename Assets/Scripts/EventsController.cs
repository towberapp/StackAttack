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
    public static UnityEvent NextLevelEvent = new();

    public static UnityEvent<int, int, Vector2[]> moveCubeEvent = new();

    // player Animation
    public static UnityEvent playerRunAnimationEvent = new();
    public static UnityEvent playerPushAnimationEvent = new();
    public static UnityEvent playerIdleAnimationEvent = new();
    public static UnityEvent<int> playerDirectionEvent = new();

    private void Awake()
    {
        Time.timeScale = 1;
        GameOverEvent.AddListener(OnGameOver);
    }

    private void OnGameOver()
    {
        print("GAME OVER EVENTS");
        SystemStatic.isGameOver = true;
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        GameOverEvent.RemoveAllListeners();
        StartEvent.RemoveAllListeners();
        NextLevelEvent.RemoveAllListeners();
    }
}
