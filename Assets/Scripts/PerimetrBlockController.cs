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


    private void Awake()
    {
        EventsController.StartEvent.AddListener(OnStartGame);
        EventsController.NextLevelEvent.AddListener(OnNextLevel);
    }

    private void OnNextLevel()
    {
        Instantiate(wallCube, new Vector2(-1, GridController.yPole-1 + SystemStatic.level), Quaternion.identity);
        Instantiate(wallCube, new Vector2(GridController.xPole, GridController.yPole-1 + SystemStatic.level), Quaternion.identity);
        folderTop.transform.position = folderTop.transform.position + Vector3.up;
    }

    private void OnStartGame()
    {
        for (int i = 0; i < GridController.yPole; i++)
        {
           Instantiate(wallCube, new Vector2(-1, i), Quaternion.identity);
           Instantiate(wallCube, new Vector2(GridController.xPole, i), Quaternion.identity);
        }

        Instantiate(cornerCube, new Vector2(-1, GridController.yPole), Quaternion.identity, folderTop.transform);
        Instantiate(cornerCube, new Vector2(GridController.xPole, GridController.yPole), Quaternion.identity, folderTop.transform);


        for (int i = 0; i < GridController.xPole; i++)
        {
            Instantiate(wallCubeTop, new Vector2(i, GridController.yPole), Quaternion.identity, folderTop.transform);
        }
    }
}
