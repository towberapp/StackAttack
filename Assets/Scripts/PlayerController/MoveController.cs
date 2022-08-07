using System.Collections;
using UnityEngine;

public class MoveController : MonoBehaviour
{

    private bool isListenKey;
    private Vector2Int lastKey;

    MoveByGrid moveByGrid;

    private void Awake()
    {
        moveByGrid = GetComponent<MoveByGrid>();
    }

    private void Start()
    {
        transform.position = new Vector2(0f, 1f);
        isListenKey = true;
    }


    public void ControlUiButtonDown(int key)
    {
        switch (key)
        {
            case 1:
                OnClickControl(Vector2Int.left);
                break;
            case 2:
                OnClickControl(Vector2Int.right);
                break;
            case 3:
                OnClickControl(Vector2Int.left + Vector2Int.up);
                break;
            case 4:
                OnClickControl(Vector2Int.right + Vector2Int.up);
                break;
            case 5:
                OnClickControl(Vector2Int.up);
                break;
            default:
                break;
        }
    }


    void Update()
    {
        // left
        if (Input.GetKey(KeyCode.A)) OnClickControl(Vector2Int.left);
        
        // right
        if (Input.GetKey(KeyCode.D)) OnClickControl(Vector2Int.right);
        
        // upleft
        if (Input.GetKey(KeyCode.Q)) OnClickControl(Vector2Int.left + Vector2Int.up);
        
        // upright
        if (Input.GetKey(KeyCode.E)) OnClickControl(Vector2Int.right + Vector2Int.up);

        // up
        if (Input.GetKey(KeyCode.W)) OnClickControl(Vector2Int.up);
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

        if (IsCanPlayerMove(direction))
        {
            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(direction));                     
        }


        Vector2Int nextDirection = GetNextDirection(direction);

        if (IsCanPlayerMove(nextDirection) && !moveByGrid.IsPoleEmpty(nextDirection + Vector2Int.down) && nextDirection != Vector2Int.zero)
        {
            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(nextDirection));
        }


        while (moveByGrid.IsPoleEmpty(Vector2Int.down))
        {
            yield return StartCoroutine(moveByGrid.NewMovePlayerGrid(Vector2Int.down));
        }

        isListenKey = true;
    }


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
            Vector2Int moveCoord = moveByGrid.GetMoveCoord(direction);
            GridController.MoveBlock(moveCoord, direction);

            return moveByGrid.IsPoleEmpty(Vector2Int.left);
        }

        if (direction == Vector2Int.right)
        {
            Vector2Int moveCoord = moveByGrid.GetMoveCoord(direction);
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
