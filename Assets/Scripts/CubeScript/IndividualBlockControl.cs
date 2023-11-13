using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class IndividualBlockControl : MonoBehaviour
{

    MoveByGrid moveByGrid;
    private IEnumerator coroutine;
    private bool isMoving = true;
    private bool isStart = false;
    private bool isOnMoveBlock = false;
    SpriteRenderer rend;

    [HideInInspector] public UnityEvent touchGround;

    private void Awake()
    {
        moveByGrid = GetComponent<MoveByGrid>();
        EventsController.UpgradeGridEvent.AddListener(OnChageGrid);
        EventsController.NextLevelEvent.AddListener(OnLevelUp);
        rend = GetComponent<SpriteRenderer>();
        EventsController.PlayerMove.AddListener(PlayerMove);
    }

    private void PlayerMove()
    {
        if (!isStart || isMoving) return; 
        if (!isOnMoveBlock) return;

        StartCoroutine(StartMoveBlock(Vector2Int.zero, 1));
        Debug.Log("UPDATE CUBE");
    }

    private void OnLevelUp()
    {
        moveByGrid.y -= 1;
        CheckUnderGround();
    }

    private void Start()
    {        
        //StartCoroutine(StartMoveBlock(Vector2Int.zero, 1));
    }


    public void DropCran()
    {
        StartCoroutine(StartMoveBlock(Vector2Int.zero, 1));
        isStart = true;
    }


    public void DestroyCube()
    {
        if (moveByGrid.y < 0) return;

        GridController.DeleteCube(moveByGrid.x, moveByGrid.y);
        Destroy(gameObject);
    }


    private void OnChageGrid()
    {
        // для того что, бы продвинулась дальше в низ, если свободна ячейка
        if (!isMoving && moveByGrid.y > 0)
        {            
            if (moveByGrid.IsPoleEmpty(Vector2Int.down))
                StartCoroutine(StartMoveBlock(Vector2Int.zero, 2));
        }

        // обновим слой куба
        if (moveByGrid.y >= 0)
            rend.sortingOrder = moveByGrid.y;
    }

    // checkToMove
    public void MoveBlock(Vector2Int direction, bool auto = true)
    {  
        bool isEmpty = moveByGrid.IsPoleEmpty(direction);
        bool isEmptyUp = moveByGrid.IsPoleEmpty(Vector2Int.up);
        bool isCanMoove = true;

        // bonus blocl msg
        if (GetComponent<BonusBlockScript>() != null && isEmptyUp)
        {
            BonusBlockScript bonusBlockScript = GetComponent<BonusBlockScript>();
            bonusBlockScript.TryTouchBonusBlock();
            DestroyCube();
            return;
        }

        // несдвигаемый блок
        if (GetComponent<NoMooveCube>() != null)
        {
            // проверим дополнительно на бонус сдвигания

            if (BonusController.activatedBonus == BonusController.ActivatedStatus.active && 
                BonusController.activatedBonusType.objectName == "Power")
            {                
                BonusController.onUseBonus.Invoke();
                BonusController.bonusPowerEvent.Invoke();

                NoMooveCube componentToRemove = GetComponent<NoMooveCube>();
                Destroy(componentToRemove);

            } else
            {
                isCanMoove = false;
            }
        }

     


        if (isEmpty && isEmptyUp && isCanMoove)
        {
            EventsController.blockMoove.Invoke(true);
            GridController.UpdateGrid(moveByGrid.x, moveByGrid.y, direction);
            StartCoroutine(StartMoveBlock(direction, 3));

        } else
        {
            // не можем двигать блок, значит взрываем, если есть бонус
            // проверим бонус двойного прыжка
            if (BonusController.activatedBonus == BonusController.ActivatedStatus.active)
            {
                if (BonusController.activatedBonusType.objectName == "DeadSpace")
                {
                    // надо чуть задержать движение игрока, что бы кубы выше взорванного блока начали падать.
                    SystemStatic.isStartGame = false;

                    BonusController.onUseBonus.Invoke();
                    BonusController.bonusDeadSpaceEvent.Invoke();

                    Instantiate(BonusController.boomBoxStatic, transform.position, transform.rotation);

                    DestroyCube();

                    SystemStatic.isStartGame = true;

                    return;
                }
            }

            EventsController.blockMoove.Invoke(false);
        }
    }

    private IEnumerator StartMoveBlock(Vector2Int direction, int fn = 0)
    {
        //Debug.Log("fn: " + fn + ", START MOVE! :" + moveByGrid.id );

        if (SystemStatic.isGameOver) yield break;       

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

                rend.sortingOrder = moveByGrid.y;

                yield return StartCoroutine(coroutine);
            }
        
        isMoving = false;

        touchGround.Invoke();
        EventsController.boxDownFloor.Invoke();

        CheckUnderGround();

        // если блок встречает ленту-блок
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
                    isOnMoveBlock = true;
                    MoveBlock(moveNow, false);
                } else
                {
                    isOnMoveBlock = false;
                }
            } else
            {
                isOnMoveBlock = false;
            }
        } else
        {
            isOnMoveBlock = false;
        }

    }

    private void CheckUnderGround()
    {
        if (moveByGrid.y<0 && !isMoving)
        {          
            Color tmp = rend.color;
            tmp.a = 0.5f;
            rend.color = tmp;

            rend.sortingOrder = -1;
            
            Destroy(moveByGrid);
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        EventsController.UpgradeGridEvent.RemoveListener(OnChageGrid);
        EventsController.NextLevelEvent.RemoveListener(OnLevelUp);
    }
}
