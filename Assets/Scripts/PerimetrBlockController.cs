using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerimetrBlockController : MonoBehaviour
{
    [SerializeField] private GameObject wallCube;
    [SerializeField] private GameObject wallCubeTop;
    [SerializeField] private GameObject cornerCube;

    [SerializeField] private GameObject folderTop;
    [SerializeField] private GameObject folderFix;


    private void Awake()
    {
        EventsController.StartEvent.AddListener(OnStartGame);
        EventsController.NextLevelEvent.AddListener(OnNextLevel);
    }

    private void OnNextLevel()
    {
        Instantiate(wallCube, new Vector2(-1, GridController.yPole-1 + SystemStatic.level), Quaternion.identity, folderFix.transform);
        Instantiate(wallCube, new Vector2(GridController.xPole, GridController.yPole-1 + SystemStatic.level), Quaternion.identity, folderFix.transform);        
    }

    private void OnStartGame()
    {

        for (int i = 0; i < GridController.yPole; i++)
        {
           Instantiate(wallCube, new Vector2(-1, i), Quaternion.identity, folderFix.transform);
           Instantiate(wallCube, new Vector2(GridController.xPole, i), Quaternion.identity, folderFix.transform);
        }

        // folderTop 
        Instantiate(cornerCube, new Vector2(-2, GridController.yPole), Quaternion.identity, folderTop.transform);
        Instantiate(cornerCube, new Vector2(GridController.xPole-1, GridController.yPole), Quaternion.identity, folderTop.transform);

        for (int i = -1; i < GridController.xPole-1; i++)
        {
            Instantiate(wallCubeTop, new Vector2(i, GridController.yPole), Quaternion.identity, folderTop.transform);
        }
    }
}
