using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    [SerializeField] private GameObject moveFolder;

    private void Awake()
    {
        EventsController.NextLevelEvent.AddListener(OnLevelUp);
    }
 

    private void OnLevelUp()
    {
        StartCoroutine(MoveObject(Camera.main.gameObject));       

        for (int y = 0; y < GridController.yPole-1; y++)
           for (int x = 0; x < GridController.xPole; x++)
           {
                GridController.blockGrid[x, y] = GridController.blockGrid[x, y + 1];
                GridController.mainGrid[x, y] = GridController.mainGrid[x, y + 1];
           }

        for (int x = 0; x < GridController.xPole; x++)
        {
            GridController.blockGrid[x, GridController.yPole - 1] = null;
            GridController.mainGrid[x, GridController.yPole - 1] = 0;
        }

        print("--- ON LEVEL UP ---");
        ShowGrid();
        print("---  ---");
    }

    private void ShowGrid()
    {
        for (int y = 0; y < GridController.yPole - 1; y++)
            for (int x = 0; x < GridController.xPole; x++)
            {
                if (GridController.mainGrid[x, y] == 1)
                    Debug.LogFormat("x: {0}, y: {1} , val: {2}", x, y, GridController.mainGrid[x, y]);
            }
    }


    private IEnumerator MoveObject(GameObject obj)
    {
        float elapseTime = 0;

        Vector3 originPos = obj.transform.position;
        Vector3 targetPos = new(originPos.x, originPos.y + 1, originPos.z);

        while (elapseTime < MainConfig.speedMove)
        {
            obj.transform.position = Vector3.Lerp(originPos, targetPos, (elapseTime / MainConfig.speedMove));
            elapseTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = new Vector3(Mathf.Round(targetPos.x), Mathf.Round(targetPos.y), originPos.z);
    }


}
