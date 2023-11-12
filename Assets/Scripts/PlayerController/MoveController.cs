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

    private bool isMooving = false;

    private void Awake()
    {
        moveByGrid = GetComponent<MoveByGrid>();
        EventsController.UpgradeGridEvent.AddListener(OnChageGrid);
        EventsController.NextLevelEvent.AddListener(OnLevelUp);
        EventsController.StartEvent.AddListener(OnStart);
        EventsController.RestorePlayer.AddListener(OnRestore);
    }

    private void OnRestore()
    {
        Debug.Log("StartRestore");

        moveByGrid.y++;

        GridController.blockGrid[moveByGrid.x, moveByGrid.y] = gameObject;
        GridController.mainGrid[moveByGrid.x, moveByGrid.y] = 2;        

        Vector2Int newPlayerPos = new Vector2Int(moveByGrid.x, moveByGrid.y);
        transform.position = (Vector2)newPlayerPos;
        SystemStatic.isStartGame = true;
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
            //Debug.Log("PLAYER START MOVE");
            StartCoroutine(StartMovePlayer(Vector2Int.zero));
        }
    }

    private void Start()
    {
        //Debug.Log("Start 1: " + playerPos);

        transform.position = (Vector2)playerPos;
        isListenKey = true;

        //Debug.Log("Start 2: " + playerPos);
    }


    //key

    private bool keyLeft = false;
    private bool keyRight = false;
    private bool keyUp = false;
    private bool keyLeftUp = false;
    private bool keyRightUp = false;

    private bool anykeydown = false;
    private bool isCantMoove = false;

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

        if (SystemStatic.isStartGame)
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

    /*        if (Input.anyKey && !anykeydown)
            {
                anykeydown = true;
            }*/


            if ((!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !keyLeft && !keyRight) && anykeydown)
            {
                //Debug.Log("ANY KEY FALSE");
                anykeydown = false;

                if (!isMooving)
                {
                    //Debug.Log("Double controle");
                    EventsController.playerIdleAnimationEvent.Invoke();
                }
                    
            }
                
            
        }

    }

    private void FixedUpdate()
    {
        MainConfig.playerFloatX = transform.position;
    }


    private void OnClickControl(Vector2Int direction)
    {
        if (!anykeydown)
        {
            anykeydown = true;
            //Debug.Log("ANY KEY TRUE");
        }
             

        lastKey = direction;

        if (!isListenKey) return;

        lastDirection = direction;

        StartCoroutine(StartMovePlayer(direction)); 
    }



    private IEnumerator StartMovePlayer(Vector2Int direction)
    {
        isMooving = true;
        //Debug.Log("TRY MOVE DIRECTION: " + direction);

        Vector2Int startPos = new(moveByGrid.x, moveByGrid.y);

        if (SystemStatic.isGameOver) yield break;
        isListenKey = false;
        moveByGrid.isMove = true;

        if (IsCanPlayerMove(direction) && direction != Vector2Int.zero && !SystemStatic.isGameOver)
        {
            isCantMoove = true;

            Vector2Int newDirection = ChangeDirection(direction);
            EventsController.playerDirectionEvent.Invoke(newDirection.x);

            Vector2Int destination = moveByGrid.GetMoveDestination(newDirection);
            
            ChangePos(newDirection);
            EventsController.PlayerMove.Invoke();

            //Debug.Log("Direction move: " + direction);

            if (newDirection == (Vector2Int.up + Vector2Int.right))
                EventsController.playerJumpAnimationEvent.Invoke(1);

            if (newDirection == (Vector2Int.up + Vector2Int.left))
                EventsController.playerJumpAnimationEvent.Invoke(-1);

            if (newDirection == Vector2Int.up || direction == Vector2Int.up*2)
                EventsController.playerJumpAnimationEvent.Invoke(0);

            if (newDirection == Vector2Int.right || direction == Vector2Int.left)
                EventsController.playerRunAnimationEvent.Invoke();


            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));

        } else
        {            
            if (isCantMoove)
            {
                Debug.Log("IS CANT MOOVE");
                EventsController.playerIdleAnimationEvent.Invoke();
                isCantMoove = false;
            }           
        }

        Vector2Int nextDirection = GetNextDirection(direction);

        if (nextDirection != Vector2Int.zero)
        {
            //if (!moveByGrid.IsPoleEmpty(nextDirection + Vector2Int.down))
            {
                if (IsCanPlayerMove(nextDirection))
                {
                    lastDirection = nextDirection;

                   //if (direction == (Vector2Int.up + Vector2Int.right))
                    //    EventsController.playerJumpAnimationEvent.Invoke(1);

                    //if (direction == (Vector2Int.up + Vector2Int.left))
                   //     EventsController.playerJumpAnimationEvent.Invoke(-1);

                    //if (direction == (Vector2Int.up))
                    //    EventsController.playerJumpAnimationEvent.Invoke(0);

                    if (nextDirection == Vector2Int.right || nextDirection == Vector2Int.left)
                        EventsController.playerRunAnimationEvent.Invoke();


                    EventsController.playerDirectionEvent.Invoke(nextDirection.x);
                    Vector2Int destination = moveByGrid.GetMoveDestination(nextDirection);
                    ChangePos(nextDirection);
                    EventsController.PlayerMove.Invoke();
                    yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));
                }
            }
        }

        while (moveByGrid.IsPoleEmpty(Vector2Int.down))
        {
            EventsController.playerIdleAnimationEvent.Invoke();
            Vector2Int destination = moveByGrid.GetMoveDestination(Vector2Int.down);
            ChangePos(Vector2Int.down);
            lastKey = Vector2Int.down;
            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(destination));
        }

        Vector2Int curPos = new(moveByGrid.x, moveByGrid.y);


        // check special cube
        isListenKey = true;
        moveByGrid.isMove = false;


        Vector2Int checkDown = CheckDownCube();
        if (checkDown != Vector2Int.zero)
        {
            if (checkDown.y == 2 && curPos != startPos)
            {
                StartCoroutine(StartMovePlayer(Vector2Int.up * 2));
                lastKey = Vector2Int.up;
            }

            // if moving block
            if (checkDown.x != 0)
            {
                if (lastKey != Vector2Int.up)
                {
                    if (moveByGrid.IsPoleEmpty(checkDown))
                        StartCoroutine(StartMovePlayer(checkDown));
                }
                else
                {
                    StartCoroutine(StartMovePlayer(Vector2Int.up));
                }

            }

        }


        //Debug.Log("STOP ALL MOOVE");        
        EventsController.playerStopMoove.Invoke();

        //Debug.Log($"LastKey: {lastKey}, lastdir: {lastDirection}");        


        if (!anykeydown)
        {
            EventsController.playerIdleAnimationEvent.Invoke();
        } else
        {
            //Debug.Log("ANYKAY DOWN");
        }

        //Debug.Log("STOP MOOVING");
        isMooving = false;
    }


    private Vector2Int CheckDownCube()
    {        
        if (moveByGrid.y > 0)
        {
            Vector2Int pos = new(moveByGrid.x, moveByGrid.y - 1);
            GameObject obj = GridController.GetBlockByPos(pos);

            if (obj == null) return Vector2Int.down;

            // если под ногами динамит - включить его
            if (obj.GetComponent<SpecialTNTControl>() != null)
            {
                obj.GetComponent<SpecialTNTControl>().StartAnim();
            }


            if (obj.GetComponent<CubeGlobal>() != null)
            {
                CubeGlobal cube = obj.GetComponent<CubeGlobal>();

                if (cube.jumpHeight > 1)
                {
                    return Vector2Int.up*2;
                } 

                if (cube.moveLenta != Vector2Int.zero)
                {
                    return cube.moveLenta;
                }

                return Vector2Int.zero;

            } else
            {
                return Vector2Int.zero;
            }

        } else
        {
            return Vector2Int.zero;
        }
    }

    private void ChangePos(Vector2Int dir)
    {


        GridController.UpdateGrid(moveByGrid.x, moveByGrid.y, dir, 2);

        moveByGrid.x += dir.x;
        moveByGrid.y += dir.y;

        MainConfig.playerX = moveByGrid.x;
        MainConfig.playerY = moveByGrid.y;
        
    } 


    private Vector2Int ChangeDirection(Vector2Int direction) 
    {
        Vector2Int checkDown = CheckDownCube();

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

        if (direction == Vector2Int.left)
        {
            
        }

        if (direction == Vector2Int.up)
        {
            if (checkDown.y == 2)
            {
                return Vector2Int.up*2;
            }        

            // проверим бонус двойного прыжка
            if (BonusController.activatedBonus == BonusController.ActivatedStatus.active)
            {
                if (BonusController.activatedBonusType.objectName == "Jump")
                {                    
                    BonusController.onUseBonus.Invoke();
                    BonusController.bonusJumpEvent.Invoke();
                    return Vector2Int.up * 2;
                }
            }

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
