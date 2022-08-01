using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private float timeMove = .5f;    

    private bool isMoving = false;

    private bool leftKey = false;
    private bool rightKey = false;
    private bool upLeftKey = false;
    private bool upRightKey = false;

    private Vector2 originPos, targetPos;

    private Vector2Int gridPlayer = new Vector2Int(12, 5);
    private int[,] poleArray;

    private int xPole = 12;
    private int yPole = 6;

    private void Awake()
    {
        poleArray = new int[xPole, yPole];

        poleArray[0, 0] = 1;
        poleArray[1, 0] = 1;
        poleArray[2, 0] = 1;
        poleArray[4, 0] = 1;
        poleArray[4, 1] = 1;
        poleArray[5, 0] = 1;
        poleArray[7, 0] = 1;
        poleArray[8, 0] = 1;
        poleArray[8, 1] = 1;
        poleArray[10, 0] = 1;

    }

    private void Start()
    {
        //setBlock
        SetBlockFromArray(poleArray);
        transform.position = new Vector2(0f, 1f);
    }

    public void ControlUiButtonDown(int key)
    {
        switch (key)
        {
            case 1:
                leftKey = true;
                break;
            case 2:
                rightKey = true;
                break;
            case 3:
                upLeftKey = true;
                break;
            case 4:
                upRightKey = true;
                break;
            default:
                break;
        }
    
    }

    public void ControlUiButtonUp(int key)
    {
        switch (key)
        {
            case 1:
                leftKey = false;
                break;
            case 2:
                rightKey = false;
                break;
            case 3:
                upLeftKey = false;
                break;
            case 4:
                upRightKey = false;
                break;
            default:
                break;
        }
    }




    void Update()
    {

        // left
        if ((Input.GetKey(KeyCode.A) || leftKey) && !isMoving)
        {
            MovePlayer(-1);
        }

        // right
        if ((Input.GetKey(KeyCode.D) || rightKey) && !isMoving)
        {
            MovePlayer(1);
        }

        // upleft
        if ((Input.GetKey(KeyCode.Q) || upLeftKey) && !isMoving)
        {
            JumpPlayer(-1);
        }

        // upright
        if ((Input.GetKey(KeyCode.E) || upRightKey) && !isMoving)
        {
            JumpPlayer(1);
        }

    }



    private void MovePlayer(int x)
    {
        isMoving = true;
        int posX = (int)transform.position.x;
        int posY = (int)transform.position.y;

        bool a = CheckPole(posX + x, posY);

        if (!a) {
            isMoving = false;
            return;
        }
        StartCoroutine(MovePlayerGrid(new Vector2(x, 0)));
    }


    private void JumpPlayer(int x)
    {
        isMoving = true;
        
        int posX = (int)transform.position.x;
        int posY = (int)transform.position.y;

        bool a = CheckPole(posX + x, posY);
        bool b = CheckPole(posX + x, posY + 1);
        bool c = CheckPole(posX + (x*2), posY + 1);
        bool d = CheckPole(posX + (x*2), posY);

        if (!a && b) {
            //small
            StartCoroutine(MovePlayerGrid(new Vector2(x, 1)));            
            return;
        }

        if (a && c && !d)
        {
            //big
            StartCoroutine(MovePlayerGrid(new Vector2(x*2, 1)));
            return;
        }

        isMoving = false;

    }


    private bool CheckPole(int x, int y)
    {
        if (x < 0 || y < 0 || x >= xPole || y >= yPole) return false;        
        if (poleArray[x,y] == 1) return false;        
        return true;
    } 



    private IEnumerator MovePlayerGrid (Vector2 direction)
    {
        isMoving = true;
        float elapseTime = 0;

        originPos = transform.position;
        targetPos = originPos + direction;

        while (elapseTime < timeMove)
        {
            transform.position = Vector3.Lerp(originPos, targetPos, (elapseTime / timeMove));
            elapseTime += Time.deltaTime;
            yield return null; 
        }

        transform.position = new Vector2 (Mathf.Round(targetPos.x), Mathf.Round(targetPos.y));
        
        isMoving = false;
        
        //check dno
        CheckDnoPlayer(transform.position);

        
    }


    private void CheckDnoPlayer(Vector2 pos)
    {               
        int playerX = (int)transform.position.x;
        int playerY = (int)transform.position.y;       
        if (playerY == 0) return;
        
        int chechTarget = poleArray[playerX, playerY - 1];

        if (chechTarget == 1) return;
        
        StartCoroutine(MovePlayerGrid(new Vector2(0, -1)));
  }


    private void SetBlockFromArray(int[,] arr)
    {        
        for (int y = 0; y < yPole; y++)
        {
            for (int x = 0; x < xPole; x++)
            {
                if (arr[x, y] == 1)
                {
                    Instantiate(cube, new Vector2(x, y), Quaternion.identity);
                }
            }
        }
    }



}
