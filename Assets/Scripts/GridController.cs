using System;
using UnityEngine;

public class GridController : MonoBehaviour
{
    //[SerializeField] private int xPoleConfig = 7;
    [SerializeField] private int yPoleConfig = 6;

    public static int[,] mainGrid;
    public static GameObject[,] blockGrid;

    public static int xPole;
    public static int yPole;

    private void Awake()
    {
        EventsController.PreStartEvent.AddListener(OnPreStartGame);
    }

    private void OnPreStartGame(int row)
    {
        //creat array
        //print("ROW: " + row);

        xPole = row;
        yPole = yPoleConfig;
        mainGrid = new int[xPole, yPole];
        blockGrid = new GameObject[xPole, yPole];

        Debug.Log("START");

        EventsController.StartEvent.Invoke();
        SystemStatic.isStartGame = true;
    }

    public static bool CheckPole(Vector2Int pos)
    {
        //print("CHECK POLE: " + pos);
        //ShowGrid();

        if (pos.x < 0 || pos.y < 0 || pos.x >= xPole || pos.y >= yPole) return false;
        if (mainGrid[pos.x, pos.y] == 1) return false;
        return true;
    }

    private static void ShowGrid()
    {
        for (int y = 0; y < yPole; y++)
            for (int x = 0; x < xPole; x++)
                if (mainGrid[x, y] == 1)
                    Debug.LogFormat("In GRID: {0}, {1}", x, y);

        Debug.Log("-------------------");
    }

    public static bool IsOutOfRange(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= xPole || pos.y >= yPole) return true;
        return false;
    }

    public static bool IsInArray(Vector2Int pos)
    {
        if (mainGrid[pos.x, pos.y] == 1) return true;
        return false;
    }


    public static void MoveBlock(Vector2Int blockPos, Vector2Int direction)
    {
        if (!IsOutOfRange(blockPos) && !IsInArray(blockPos))
        {
            EventsController.playerRunAnimationEvent.Invoke();
        }
      
        if (!IsOutOfRange(blockPos) && IsInArray(blockPos))
        {
            //print("MOVE BLOCK IN ARRAR: " + blockPos);
            GameObject block = blockGrid[blockPos.x, blockPos.y];
            IndividualBlockControl 
                individualBlockControl = block.GetComponent<IndividualBlockControl>();
                individualBlockControl.MoveBlock(direction);
        } 
    }


    public static void UpdateGrid(int x, int y, Vector2Int move)
    {       
        if (move == Vector2Int.zero) return;

        //print("UPDATE GRID: " + x + "," + y + " -> " + move);

        blockGrid[x + move.x, y + move.y] = blockGrid[x, y];
        blockGrid[x, y] = null;

        mainGrid[x + move.x, y + move.y] = 1;
        mainGrid[x, y] = 0;

        //ShowGrid();

        CheckForNexLevel();
        CheckForGameOver();

        EventsController.UpgradeGridEvent.Invoke();
    }

    public static void DeleteCube(int x, int y)
    {
        blockGrid[x, y] = null;
        mainGrid[x, y] = 0;
    }



    private static void CheckForGameOver()
    {        
    }

    private static void CheckForNexLevel()
    {
        int count = 0;
        for (int x = 0; x < xPole; x++)
        {
            if (mainGrid[x, 0] == 1)
            {
                count++;
            }
        }

        if (count == xPole)
        {
            SystemStatic.level++;
            EventsController.NextLevelEvent.Invoke();
        }
    }
    



  /*  private void StartGenerateGrid()
    {
        mainGrid[0, 0] = 1;
        mainGrid[1, 0] = 1;
        mainGrid[2, 1] = 1;
        mainGrid[2, 2] = 1;
        mainGrid[2, 0] = 1;
        mainGrid[4, 0] = 1;
        mainGrid[4, 1] = 1;
        mainGrid[5, 0] = 1;
        mainGrid[5, 1] = 1;
        mainGrid[5, 2] = 1;
        mainGrid[6, 0] = 1;
    }*/
}
