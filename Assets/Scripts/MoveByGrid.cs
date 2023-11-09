using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByGrid : MonoBehaviour
{
    public int x;
    public int y;
    public bool isMove = false;
    public int id;

    private void Awake()
    {
        id = Random.Range(0, 500);
    }

    public IEnumerator NewMovePlayerGrid(Vector2Int destination)
    {     
        float elapseTime = 0;        
        Vector2 originPos = transform.position; 
        Vector2 targetPos = destination;

        //Debug.LogFormat("originPos: {0}, targetpos {1} ", originPos, targetPos);
        while (elapseTime < MainConfig.speedMove && !SystemStatic.isGameOver)
        {
            transform.position = Vector3.Lerp(originPos, targetPos, (elapseTime / MainConfig.speedMove));
            elapseTime += Time.deltaTime;
            yield return null;
        }

        if (!SystemStatic.isGameOver)
            transform.position = new Vector2(Mathf.Round(targetPos.x), Mathf.Round(targetPos.y));
    }


    public bool IsPoleEmpty(Vector2Int direction)
    {
        Vector2Int poleForCheck = new Vector2Int(x, y) + direction;
        return GridController.CheckPole(poleForCheck);
    }

    public Vector2Int GetMoveDestination(Vector2Int direction)
    {
        //Debug.Log("GetMoveDestination: " + direction + " , x:" + x + ", y:" + y);  
        return new Vector2Int(x + direction.x, y + direction.y + SystemStatic.level);
    }

}
