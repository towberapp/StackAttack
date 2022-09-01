using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveController : MonoBehaviour
{
    [SerializeField] Vector2Int playerPos = Vector2Int.zero;

    private bool isListenKey;
    private Vector2Int lastKey , lastDirection;
    MoveByGrid moveByGrid;
   

    private void Awake()
    {
        moveByGrid = GetComponent<MoveByGrid>();
        EventsController.UpgradeGridEvent.AddListener(OnChageGrid);
        EventsController.NextLevelEvent.AddListener(OnLevelUp);
        EventsController.StartEvent.AddListener(OnStart);
    }

    private void OnStart()
    {
        // set user to array;
        GridController.blockGrid[playerPos.x, playerPos.y] = gameObject;
        GridController.mainGrid[playerPos.x, playerPos.y] = 2;
    }

    private void OnLevelUp()
    {
        moveByGrid.y -= 1;

        if (moveByGrid.y < 0)
        {
            EventsController.GameOverEvent.Invoke();
        }
    }

    private void OnChageGrid()
    {
        if (isListenKey && moveByGrid.IsPoleEmpty(Vector2Int.down))
        {
            Debug.Log("PLAYER START MOVE");
            StartCoroutine(StartMovePlayer(Vector2Int.zero));
        }
    }

    private void Start()
    {
        transform.position = (Vector2)playerPos;
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

       /* if (isListenKey && lastDirection != lastKey && lastKey != Vector2Int.zero)
        {
            Debug.Log("MOVE!!! : " + lastKey);
        }*/
    }


    private void OnClickControl(Vector2Int direction)
    {
        lastKey = direction;        
        if (!isListenKey) return;

        lastDirection = direction;
        StartCoroutine(StartMovePlayer(direction));        
    }

    private IEnumerator StartMovePlayer(Vector2Int direction)
    {

        if (SystemStatic.isGameOver) yield break;
        isListenKey = false;
        moveByGrid.isMove = true;

        if (IsCanPlayerMove(direction) && direction != Vector2Int.zero && !SystemStatic.isGameOver)
        {
            Vector2Int newDirection = ChangeDirection(direction);
            EventsController.playerDirectionEvent.Invoke(newDirection.x);
            Vector2Int destination = moveByGrid.GetMoveDestination(newDirection);

            ChangePos(newDirection);

            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));
        }

        Vector2Int nextDirection = GetNextDirection(direction);

        if (nextDirection != Vector2Int.zero)
        {
            //if (!moveByGrid.IsPoleEmpty(nextDirection + Vector2Int.down))
            {
                if (IsCanPlayerMove(nextDirection))
                {
                    lastDirection = nextDirection;

                    EventsController.playerDirectionEvent.Invoke(nextDirection.x);
                    Vector2Int destination = moveByGrid.GetMoveDestination(nextDirection);
                    ChangePos(nextDirection);
                    yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));
                }
            }
        }

        while (moveByGrid.IsPoleEmpty(Vector2Int.down))
        {
            Vector2Int destination = moveByGrid.GetMoveDestination(Vector2Int.down);
            ChangePos(Vector2Int.down);
            lastKey = Vector2Int.down;
            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));
        }

        EventsController.playerIdleAnimationEvent.Invoke();


      /*  Debug.Log("direction:" + direction);
        Debug.Log("next direction:" + nextDirection);
        Debug.Log("lastkey:" + lastKey);*/

        // check special cube
        isListenKey = true;
        moveByGrid.isMove = false;

        // move in special cube 
        if (moveByGrid.y > 0)// && lastKey == nextDirection || moveByGrid.y>0 && nextDirection == Vector2Int.zero)
        {
            Vector2Int pos = new(moveByGrid.x, moveByGrid.y - 1);
            GameObject obj = GridController.GetBlockByPos(pos);            
            // if down is block
            if (obj.GetComponent<CubeGlobal>() != null)
            {
                CubeGlobal cube = obj.GetComponent<CubeGlobal>();
                
                // lentaCube
                Vector2Int moveNow = cube.moveLenta;
                int jumpHeight = cube.jumpHeight;

                //funblock 
                if (jumpHeight > 1)
                {
                    StartCoroutine(StartMovePlayer(Vector2Int.up*2));
                    lastKey = Vector2Int.up;
                }

                // if moving block
                if (moveNow != Vector2Int.zero)
                {
                    if (lastKey != Vector2Int.up)
                    {
                        if (moveByGrid.IsPoleEmpty(moveNow))
                            StartCoroutine(StartMovePlayer(moveNow));
                    } else
                    {
                        StartCoroutine(StartMovePlayer(Vector2Int.up));
                    }

                }
            }
        }

    }

    private void ChangePos(Vector2Int dir)
    {
        //Debug.Log("CHANGE POS: " + dir);

        GridController.UpdateGrid(moveByGrid.x, moveByGrid.y, dir, 2);

        moveByGrid.x += dir.x;
        moveByGrid.y += dir.y;

        MainConfig.playerX = moveByGrid.x;
        MainConfig.playerY = moveByGrid.y;
        
    } 


    private Vector2Int ChangeDirection(Vector2Int direction) 
    {
        if (direction == (Vector2Int.left + Vector2Int.up))
        {
            if (moveByGrid.IsPoleEmpty(Vector2Int.left)) return direction;

            return Vector2Int.up;
        }

        if (direction == (Vector2Int.right + Vector2Int.up))
        {
            if (moveByGrid.IsPoleEmpty(Vector2Int.right)) return direction;

            return Vector2Int.up;
        }

        if (direction == Vector2Int.up)
        {
            if (moveByGrid.IsPoleEmpty(Vector2Int.down))
            {
                return Vector2Int.up;
            }
            
        }

        return direction;
    }



    private bool IsCanPlayerMove(Vector2Int direction)
    {

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

        if (direction == Vector2Int.up*2 && lastKey != Vector2Int.up && (lastKey == Vector2Int.left || lastKey == Vector2Int.right))
        {
            return lastKey;
        }

        nextDirection = Vector2Int.zero;
        return nextDirection;
    }




}
