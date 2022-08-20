using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualBlockControl : MonoBehaviour
{

    MoveByGrid moveByGrid;
    private IEnumerator coroutine;
    private bool isMoving = false;

    private void Awake()
    {
        moveByGrid = GetComponent<MoveByGrid>();
        EventsController.UpgradeGridEvent.AddListener(OnChageGrid);

        // TODO: delete late
        EventsController.CheckForBrakeEvent.AddListener(OnCheckBrake);

        EventsController.GameOverEvent.AddListener(OnGameOver);
    }  

    private void Start()
    {        
        StartCoroutine(StartMoveBlock(Vector2Int.zero));
    }

    private void OnGameOver()
    {
        //Destroy(gameObject);
    }

    public void DestroyCube()
    {
        GridController.DeleteCube(moveByGrid.x, moveByGrid.y);
        Destroy(gameObject);
    }


    // TODO: delete late
    private void OnCheckBrake(Vector2 checkPos)
    {
        Vector2Int pos = new(moveByGrid.x, moveByGrid.y);
        if (pos == checkPos && !SystemStatic.isGameOver)
        {
            DestroyCube();
        }
    }

    private void OnChageGrid()
    {
        if (!isMoving)
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
        isMoving = true;

        if (direction != Vector2Int.zero)
        {
            Vector2Int destination = moveByGrid.GetMoveDestination(direction);
            moveByGrid.x += direction.x;
            moveByGrid.y += direction.y;

            if (coroutine != null) StopCoroutine(coroutine);            
            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));
        }


        if (moveByGrid.y >= 0)
            while (moveByGrid.IsPoleEmpty(Vector2Int.down))
            {
                Vector2Int destination = moveByGrid.GetMoveDestination(Vector2Int.down);
                GridController.UpdateGrid(moveByGrid.x, moveByGrid.y, Vector2Int.down);                       
                moveByGrid.y -= 1;
                coroutine = moveByGrid.NewMovePlayerGrid(destination);
                yield return StartCoroutine(coroutine);
            }

        isMoving = false;

    }



}
