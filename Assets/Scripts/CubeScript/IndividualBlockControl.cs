using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IndividualBlockControl : MonoBehaviour
{

    MoveByGrid moveByGrid;
    private IEnumerator coroutine;
    private bool isMoving = false;
    SpriteRenderer rend;

    private void Awake()
    {
        moveByGrid = GetComponent<MoveByGrid>();
        EventsController.UpgradeGridEvent.AddListener(OnChageGrid);
        EventsController.GameOverEvent.AddListener(OnGameOver);
        EventsController.NextLevelEvent.AddListener(OnLevelUp);
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnLevelUp()
    {
        moveByGrid.y -= 1;
        CheckUnderGround();
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


    private void OnChageGrid()
    {
        // дл€ того что, бы продвинулась дальше в низ, если свободна €чейка
        if (!isMoving && moveByGrid.y > 0)
        {            
            if (moveByGrid.IsPoleEmpty(Vector2Int.down))
                StartCoroutine(StartMoveBlock(Vector2Int.zero));
        }
    }

    // checkToMove
    public void MoveBlock(Vector2Int direction, bool auto = false)
    {
        bool isEmpty = moveByGrid.IsPoleEmpty(direction);
        bool isEmptyUp = moveByGrid.IsPoleEmpty(Vector2Int.up);        

        if (isEmpty && isEmptyUp)
        {
            if (auto)
                EventsController.playerPushAnimationEvent.Invoke();
                        
            GridController.UpdateGrid(moveByGrid.x, moveByGrid.y, direction);
            StartCoroutine(StartMoveBlock(direction));
        }
    }

    private IEnumerator StartMoveBlock(Vector2Int direction)
    {
        //Debug.Log("START MOVE BLOCK");

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
        CheckUnderGround();


        if (moveByGrid.y > 0)
        {
            Vector2Int pos = new(moveByGrid.x, moveByGrid.y - 1);
            GameObject obj = GridController.GetBlockByPos(pos);

            // if down is block
            if (obj.GetComponent<CubeGlobal>() != null)
            {
                Vector2Int moveNow = obj.GetComponent<CubeGlobal>().moveLenta;
                if (moveNow != Vector2Int.zero)
                {
                    MoveBlock(moveNow, false);
                }
            }
        }

    }

    private void CheckUnderGround()
    {
        if (moveByGrid.y<0 && !isMoving)
        {          
            Color tmp = rend.color;
            tmp.a = 0.5f;
            rend.color = tmp;
            
            Destroy(moveByGrid);
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        EventsController.UpgradeGridEvent.RemoveListener(OnChageGrid);
        EventsController.GameOverEvent.RemoveListener(OnGameOver);
        EventsController.NextLevelEvent.RemoveListener(OnLevelUp);
    }
}
