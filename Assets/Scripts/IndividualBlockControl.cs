using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualBlockControl : MonoBehaviour
{

    MoveByGrid moveByGrid;

    private void Awake()
    {
        moveByGrid = GetComponent<MoveByGrid>();
    }

    public void MoveBlock(Vector2Int direction)
    {
        StartCoroutine(StartMoveBlock(direction));
    }

    private IEnumerator StartMoveBlock(Vector2Int direction)
    {

        yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(direction));


        while (moveByGrid.IsPoleEmpty(Vector2Int.down))
        {
            Vector2Int pos = Vector2Int.RoundToInt(transform.position);
            GridController.UpdateGrid(pos.x, pos.y, Vector2Int.down);
            
            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(Vector2Int.down));
        }

    }


}
