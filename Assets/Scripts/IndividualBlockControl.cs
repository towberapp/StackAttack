using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualBlockControl : MonoBehaviour
{

    MoveByGrid moveByGrid;
    private IEnumerator coroutine;

    private void Awake()
    {
        moveByGrid = GetComponent<MoveByGrid>();
    }

    private void Start()
    {        
        StartCoroutine(StartMoveBlock(Vector2Int.zero));
    }

    // checkToMove
    public void MoveBlock(Vector2Int direction)
    {
        bool isEmpty = moveByGrid.IsPoleEmpty(direction);
        bool isEmptyUp = moveByGrid.IsPoleEmpty(Vector2Int.up);        

        if (isEmpty && isEmptyUp)
        {
            EventsController.playerPushAnimationEvent.Invoke();
            GridController.UpdateGrid(moveByGrid.x, moveByGrid.y, direction);
            StartCoroutine(StartMoveBlock(direction));
        }
    }

    private IEnumerator StartMoveBlock(Vector2Int direction)
    {        

        if (direction != Vector2Int.zero)
        {
            Vector2Int destination = moveByGrid.GetMoveDestination(direction);
            moveByGrid.x += direction.x;
            moveByGrid.y += direction.y;

            if (coroutine != null) StopCoroutine(coroutine);            
            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));
        }


        while (moveByGrid.IsPoleEmpty(Vector2Int.down))
        {
            Vector2Int destination = moveByGrid.GetMoveDestination(Vector2Int.down);
            GridController.UpdateGrid(moveByGrid.x, moveByGrid.y, Vector2Int.down);                       
            moveByGrid.y -= 1;

            CheckPlayerHead();

            coroutine = moveByGrid.NewMovePlayerGrid(destination);
            yield return StartCoroutine(coroutine);
        }

    }

    private void CheckPlayerHead()
    {
        //print("CHECK DEATH : " + MainConfig.playerX + " - " + MainConfig.playerY);
        //print("CHECK DEATH moveByGrid : " + moveByGrid.x + " - " + moveByGrid.y);

        if (MainConfig.playerX == moveByGrid.x && MainConfig.playerY == moveByGrid.y)
        {
            //gameover
            //EventsController.GameOverEvent.Invoke();
        }
    }


}
