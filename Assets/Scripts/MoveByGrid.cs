using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByGrid : MonoBehaviour
{

    public IEnumerator NewMovePlayerGrid(Vector2 direction)
    {
        float elapseTime = 0;

        Vector2 originPos = transform.position;
        Vector2 targetPos = originPos + direction;

        while (elapseTime < MainConfig.speedMove)
        {
            transform.position = Vector3.Lerp(originPos, targetPos, (elapseTime / MainConfig.speedMove));
            elapseTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector2(Mathf.Round(targetPos.x), Mathf.Round(targetPos.y));
    }


    public bool IsPoleEmpty(Vector2Int direction)
    {
        Vector2Int poleForCheck = Vector2Int.RoundToInt(transform.position) + direction;
        return GridController.CheckPole(poleForCheck);
    }

    public Vector2Int GetMoveCoord(Vector2Int direction)
    {
        return Vector2Int.RoundToInt(transform.position) + direction;
    }

}
