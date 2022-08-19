using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveController : MonoBehaviour
{

    private bool isListenKey;
    private Vector2Int lastKey;
    MoveByGrid moveByGrid;

   

    private void Awake()
    {
        moveByGrid = GetComponent<MoveByGrid>();
        EventsController.UpgradeGridEvent.AddListener(OnChageGrid);
    }

    private void OnChageGrid()
    {
       if (isListenKey)
           StartCoroutine(StartMovePlayer(Vector2Int.zero));
    }

    private void Start()
    {
        transform.position = new Vector2(0f, 0f);
        isListenKey = true;
    }


    //key

    private bool keyLeft = false;
    private bool keyRight = false;
    private bool keyUp = false;
    private bool keyLeftUp = false;
    private bool keyRightUp = false;


    public void ControlUiButtonUp(int key)
    {
        switch (key)
        {
            case 1:
                keyLeft = false;
                break;
            case 2:
                keyRight = false;
                break;
            case 3:
                keyLeftUp = false;
                break;
            case 4:
                keyRightUp = false;
                break;
            case 5:
                keyUp = false;
                break;
            default:
                break;
        }
    }

    public void ControlUiButtonDown(int key)
    {
        switch (key)
        {
            case 1:
                keyLeft = true;
                break;
            case 2:
                keyRight = true;
                break;
            case 3:
                keyLeftUp = true;
                break;
            case 4:
                keyRightUp = true;
                break;
            case 5:
                keyUp = true;
                break;
            default:
                break;
        }
    }


    void Update()
    {


        // left
        if (Input.GetKey(KeyCode.A) || keyLeft) OnClickControl(Vector2Int.left);
        
        // right
        if (Input.GetKey(KeyCode.D) || keyRight) OnClickControl(Vector2Int.right);
        
        // upleft
        if (Input.GetKey(KeyCode.Q) || keyLeftUp) OnClickControl(Vector2Int.left + Vector2Int.up);
        
        // upright
        if (Input.GetKey(KeyCode.E) || keyRightUp) OnClickControl(Vector2Int.right + Vector2Int.up);

        // up
        if (Input.GetKey(KeyCode.W) || keyUp) OnClickControl(Vector2Int.up);
    }


    private void OnClickControl(Vector2Int direction)
    {
        lastKey = direction;

        if (!isListenKey) return;       

        StartCoroutine(StartMovePlayer(direction));        
    }


    private IEnumerator StartMovePlayer(Vector2Int direction)
    {        

        isListenKey = false;

        if (IsCanPlayerMove(direction) && direction != Vector2Int.zero)
        {
            EventsController.playerDirectionEvent.Invoke(direction.x);
            Vector2Int destination = moveByGrid.GetMoveDestination(direction);
            moveByGrid.x += direction.x;
            moveByGrid.y += direction.y;

            MainConfig.playerX = moveByGrid.x;
            MainConfig.playerY = moveByGrid.y;

            //print("DESTINATION: " + destination);

            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));                     
        }
        

        Vector2Int nextDirection = GetNextDirection(direction);

        //Debug.Log("DIRECTION: " + direction);
        //Debug.Log("NEXT DIRECTION: " + nextDirection);

        // check for CUBE to brake
        if (nextDirection == Vector2Int.zero && direction == Vector2Int.up)
        {
            //Debug.Log("CHECK FOR BRAKE: " + moveByGrid.x + "," + (moveByGrid.y + 1));
            EventsController.CheckForBrakeEvent.Invoke(new Vector2Int(moveByGrid.x, (moveByGrid.y + 1)));
        }

        if (nextDirection != Vector2Int.zero)
            if (!moveByGrid.IsPoleEmpty(nextDirection + Vector2Int.down)) 
                if (IsCanPlayerMove(nextDirection))
                {
                    EventsController.playerDirectionEvent.Invoke(nextDirection.x);

                    Vector2Int destination = moveByGrid.GetMoveDestination(nextDirection);
                    moveByGrid.x += nextDirection.x;
                    moveByGrid.y += nextDirection.y;

                    MainConfig.playerX = moveByGrid.x;
                    MainConfig.playerY = moveByGrid.y;
                    yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));
                }

        while (moveByGrid.IsPoleEmpty(Vector2Int.down))
        {
            Vector2Int destination = moveByGrid.GetMoveDestination(Vector2Int.down);
            moveByGrid.y -= 1;            
            MainConfig.playerY = moveByGrid.y;
            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));
        }

        EventsController.playerIdleAnimationEvent.Invoke();
        isListenKey = true;
    }


/*    private IEnumerator MovePlayerDown()
    {
        while (moveByGrid.IsPoleEmpty(Vector2Int.down))
        {
            Vector2Int destination = moveByGrid.GetMoveDestination(Vector2Int.down);
            moveByGrid.y -= 1;
            MainConfig.playerY = moveByGrid.y;
            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));
        }
    }*/


    private bool IsCanPlayerMove(Vector2Int direction)
    {
        if (direction == (Vector2Int.left + Vector2Int.up))
        {

            return moveByGrid.IsPoleEmpty(Vector2Int.left);
        }
           
        if (direction == (Vector2Int.right + Vector2Int.up))
        {
            return moveByGrid.IsPoleEmpty(Vector2Int.right);
        }
        
        if (direction == Vector2Int.left)
        {
            Vector2Int moveCoord = new Vector2Int(moveByGrid.x, moveByGrid.y) + direction;
            GridController.MoveBlock(moveCoord, direction);

            return moveByGrid.IsPoleEmpty(Vector2Int.left);
        }

        if (direction == Vector2Int.right)
        {
            Vector2Int moveCoord = new Vector2Int(moveByGrid.x, moveByGrid.y) + direction;
            GridController.MoveBlock(moveCoord, direction);

            return moveByGrid.IsPoleEmpty(Vector2Int.right);
        }

        return true;

    }


    private Vector2Int GetNextDirection(Vector2Int direction)
    {
        Vector2Int nextDirection;

        if (direction == (Vector2Int.left + Vector2Int.up)) {

            return Vector2Int.left;
        }

        if (direction == (Vector2Int.right + Vector2Int.up))
        {
            return Vector2Int.right;
        }

        if (direction == Vector2Int.up && lastKey != Vector2Int.up && (lastKey == Vector2Int.left || lastKey == Vector2Int.right))
        { 
            return lastKey;
        }

        nextDirection = Vector2Int.zero;
        return nextDirection;
    }




}
