using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainConfig : MonoBehaviour
{
    [SerializeField] private float _speedMove = 1f;
    [SerializeField] private float _interval = 4f;

    public static float speedMove;
    public static float intervalCube;

    public static int playerX;
    public static int playerY;

    public static int countCubeSet;

    private void Awake()
    {
        speedMove = _speedMove;
        intervalCube = _interval;

        countCubeSet = 0;

        playerX = 0;
        playerY = 0;

        SystemStatic.level = 0;
        SystemStatic.isGameOver = false;
        SystemStatic.isGamePaused = false;
    }
}


public class SystemStatic
{
    public static bool isGameOver = false;
    public static bool isGamePaused = false;
    public static int level = 0;
}
