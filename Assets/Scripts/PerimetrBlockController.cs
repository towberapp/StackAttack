using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerimetrBlockController : MonoBehaviour
{
    [SerializeField] private GameObject wallCube;


    private void Awake()
    {
        EventsController.StartEvent.AddListener(OnStartGame);
        EventsController.NextLevelEvent.AddListener(OnNextLevel);
    }

    private void OnNextLevel()
    {
        Instantiate(wallCube, new Vector2(-1, GridController.yPole-1 + SystemStatic.level), Quaternion.identity);
        Instantiate(wallCube, new Vector2(GridController.xPole, GridController.yPole-1 + SystemStatic.level), Quaternion.identity);
    }

    private void OnStartGame()
    {
        for (int i = 0; i < GridController.yPole; i++)
        {
           Instantiate(wallCube, new Vector2(-1, i), Quaternion.identity);
           Instantiate(wallCube, new Vector2(GridController.xPole, i), Quaternion.identity);
        }
    }
}
