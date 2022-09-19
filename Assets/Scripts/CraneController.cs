using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CraneController : MonoBehaviour
{
    [SerializeField] private GameObject cranGroup;
    [SerializeField] private GameObject cranPrefab;     

    private void Awake()
    {
        EventsController.StartEvent.AddListener(OnStartGame);
        EventsController.RunCran.AddListener(NewCube);
        EventsController.NextLevelEvent.AddListener(OnNextLevel);
    }

    private void OnNextLevel()
    {
        
    }

    private void NewCube(GameObject block, int xPos)
    {
        Vector3 pos = cranGroup.transform.position;
        GameObject obj = Instantiate(cranPrefab, pos, Quaternion.identity, cranGroup.transform);
        obj.transform.position = new(-2f, obj.transform.position.y, obj.transform.position.z);

        block.transform.position = obj.transform.position - new Vector3(0,0.75f,0);
        block.transform.SetParent(obj.transform);

        StartCoroutine(MoveObject(new Vector2Int(GridController.xPole + 4, 0), obj, xPos, block));
    }

    private void OnStartGame()
    {
        Vector3 pos = cranGroup.transform.position;
        cranGroup.transform.position = pos + new Vector3(0, GridController.yPole-0.33f, pos.z);
    }


    public IEnumerator MoveObject(Vector2 vector, GameObject obj, int stopPos, GameObject block)
    {
        bool drop = false;
        float elapseTime = 0;
        float realSpeed = MainConfig.speedMove * vector.x / 2;

        Vector2 originPos = obj.transform.position;
        Vector2 targetPos = originPos + vector;               

        while (elapseTime < realSpeed && !SystemStatic.isGameOver)
        {
            if (obj.transform.position.x >= stopPos && !drop)
            {
                drop = true;
                Debug.Log("DROP OBJ");
                EventsController.DropCran.Invoke(block, stopPos);
            }

            obj.transform.position = Vector3.Lerp(originPos, targetPos, (elapseTime / realSpeed));
            elapseTime += Time.deltaTime;
            yield return null;
        }

        if (!SystemStatic.isGameOver)
            obj.transform.position = new Vector2(Mathf.Round(targetPos.x), Mathf.Round(targetPos.y));
    }
}
