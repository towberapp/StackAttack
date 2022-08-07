using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private int xPoleConfig = 7;
    [SerializeField] private int yPoleConfig = 6;

    public static int[,] mainGrid;
    public static GameObject[,] blockGrid;


    public static int xPole;
    public static int yPole;

    private void Awake()
    {        
        xPole = xPoleConfig;
        yPole = yPoleConfig;
        mainGrid = new int[xPole, yPole];

        StartGenerateGrid();
    }

    public static bool CheckPole(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= xPole || pos.y >= yPole) return false;
        if (mainGrid[pos.x, pos.y] == 1) return false;
        return true;
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
        if (!IsOutOfRange(blockPos) && IsInArray(blockPos))
        {
            GameObject block = blockGrid[blockPos.x, blockPos.y];

            MoveByGrid moveByGrid = block.GetComponent<MoveByGrid>();
            IndividualBlockControl individualBlockControl = block.GetComponent <IndividualBlockControl>();

            bool isEmpty = moveByGrid.IsPoleEmpty(direction);

            if (isEmpty)
            {
                UpdateGrid(blockPos.x, blockPos.y, direction);
                individualBlockControl.MoveBlock(direction);                           
            }
        }
    }


    public static void UpdateGrid(int x, int y, Vector2Int move)
    {
        if (move.x == 0 && move.y == 0) return;

        blockGrid[x + move.x, y + move.y] = blockGrid[x, y];
        blockGrid[x, y] = null;

        mainGrid[x + move.x, y + move.y] = 1;
        mainGrid[x, y] = 0;
    }


    private void StartGenerateGrid()
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
    }




}
