using System;
using UnityEngine;

public class GridController : MonoBehaviour
{
    //[SerializeField] private int xPoleConfig = 7;
    //[SerializeField] private int yPoleConfig = 6;

    [SerializeField] private  int yPoleSetup = 9;

    public static int[,] mainGrid;
    public static GameObject[,] blockGrid;

    public static int xPole;
    public static int yPole;

    private void Awake()
    {
        yPole = yPoleSetup;
        EventsController.PreStartEvent.AddListener(OnPreStartGame);
    }
/*
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
    }*/

    private void OnPreStartGame(int row)
    {
        xPole = row;
        //yPole = Convert.ToInt16(xPole * 0.9f);
        //yPole = 9;

        mainGrid = new int[xPole, yPole];
        blockGrid = new GameObject[xPole, yPole];

        Debug.Log("START");

        EventsController.StartEvent.Invoke();
        SystemStatic.isStartGame = true;
    }


    public static GameObject GetBlockByPos(Vector2Int pos)
    {
        if (IsOutOfRange(pos)) return null;        
        return blockGrid[pos.x, pos.y];
    }


    public static bool CheckPole(Vector2Int pos)
    {
        if (IsOutOfRange(pos)) return false;
        if (IsInArray(pos)) return false;

        return true;
    }


    public static bool IsOutOfRange(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= xPole || pos.y >= yPole) return true;
        return false;
    }

    public static bool IsInArray(Vector2Int pos)
    {
        if (mainGrid[pos.x, pos.y] == 1 || mainGrid[pos.x, pos.y] == 2) return true;
        return false;
    }

    public static bool CheckCubeInArray(Vector2Int pos)
    {
        if (mainGrid[pos.x, pos.y] == 1) return true;
        return false;
    }


    public static void MoveBlock(Vector2Int blockPos, Vector2Int direction)
    {       
        Vector2Int blockTop = blockPos + Vector2Int.up;

        if (!IsOutOfRange(blockPos) && !IsInArray(blockPos) && !IsInArray(blockTop))
        {
            //Debug.Log("MOVE--- NO---- BLOCK");
            EventsController.blockMoove.Invoke(false);
        }
      
        if (!IsOutOfRange(blockPos) && IsInArray(blockPos))
        {
            // если есть бонус мощности
            if (BonusController.activatedBonus == BonusController.ActivatedStatus.active &&
                BonusController.activatedBonusType.objectName == "Power")
                {
                    BonusPowerMoveBlock(blockPos, direction);
                    return;
                }


            GameObject block = blockGrid[blockPos.x, blockPos.y];
            IndividualBlockControl 
                individualBlockControl = block.GetComponent<IndividualBlockControl>();
                individualBlockControl.MoveBlock(direction);
        }

    }

    public static void BonusPowerMoveBlock(Vector2Int blockPos, Vector2Int direction)
    {
        Vector2Int blockTop = blockPos + Vector2Int.up;
        Vector2Int blockTopNext = blockPos + Vector2Int.up;

        // если за блоком нет второго блока, то не запускаем бонус
        if (!IsOutOfRange(blockPos) && IsInArray(blockPos) && !IsOutOfRange(blockPos + direction) && !IsInArray(blockPos + direction))
        {
            //Debug.Log("Одиночный, не применяем бонус, пробуем толкать");

            GameObject blockBasic = blockGrid[blockPos.x, blockPos.y];
            IndividualBlockControl
                individualBlockControlBasic = blockBasic.GetComponent<IndividualBlockControl>();
                individualBlockControlBasic.MoveBlock(direction);
            return;
        }

        //Debug.Log("ПРобуем толкать двойной");

        // если за вторым блоком выход за границы - ничего не делаем
        if (IsOutOfRange(blockPos + direction + direction)) return;

        //Debug.Log("тест 1");

        // если за двумя блоками есть третий, ничего не делаем
        if (IsInArray(blockPos + direction + direction)) return;

        //Debug.Log("тест 2");

        // если на одном из двух блоков что-то стоит - ничего не делаем
        if (IsInArray(blockTop) || IsInArray(blockTopNext)) return;

        //Debug.Log("тест 3");

        // вроде всё ок, можно толкать.

        // вначале толкаем дальний
        GameObject blockNext = blockGrid[blockPos.x + direction.x, blockPos.y];
            IndividualBlockControl
            individualBlockControlNext = blockNext.GetComponent<IndividualBlockControl>();
            individualBlockControlNext.MoveBlock(direction);

        // потом ближний
        GameObject block = blockGrid[blockPos.x, blockPos.y];
        IndividualBlockControl
            individualBlockControl = block.GetComponent<IndividualBlockControl>();
            individualBlockControl.MoveBlock(direction);

        // активируем бонус
        BonusController.onUseBonus.Invoke();
        BonusController.bonusPowerEvent.Invoke();

    }


    public static void UpdateGrid(int x, int y, Vector2Int move, int type = 1)
    {
        //Debug.Log("UPDATE GRID");

        if (move == Vector2Int.zero) return;

        //print("UPDATE GRID: " + x + "," + y + " -> " + move);

        blockGrid[x + move.x, y + move.y] = blockGrid[x, y];
        blockGrid[x, y] = null;

        mainGrid[x + move.x, y + move.y] = type;
        mainGrid[x, y] = 0;

        ShowGrid();

        CheckForNexLevel();
        CheckForGameOver();

        EventsController.UpgradeGridEvent.Invoke();
    }

    public static void DeleteCube(int x, int y)
    {
        Debug.Log($"Delete Cube -> {x},{y}");

        blockGrid[x, y] = null;
        mainGrid[x, y] = 0;

        EventsController.UpgradeGridEvent.Invoke();
    }


    private static void ShowGrid()
    {
        for (int y = 0; y < yPole; y++)
            for (int x = 0; x < xPole; x++)
                if (mainGrid[x, y] > 0)
                    Debug.LogFormat("In GRID: {0}, {1} - {2}", x, y, mainGrid[x, y]);

        Debug.Log("-------------------");
    }

   public static void StartDethPlayer()
    {
        SystemStatic.isStartGame = false;
        DeletePlayer();
        EventsController.StartBeforeGameOverEvent.Invoke();
        EventsController.playerDestroyAnimationEvent.Invoke();
    }


    public static void DeletePlayer()
    {
        Vector2Int posPlayer = new (MainConfig.playerX, MainConfig.playerY);
        GameObject player = blockGrid[posPlayer.x, posPlayer.y];

        if (player == null) return;

        if (player.CompareTag("Player"))
        {
            Debug.Log($"Try Del player -> pos: {posPlayer}, obj: {player}");
            DeleteCube(posPlayer.x, posPlayer.y);
        }
        else
        {
            Debug.Log($"Try Del player -> pos: {posPlayer}, obj: {player}");
            Debug.LogWarning("Ошибка удаления игрока");
        }
    }


    private static void CheckForGameOver()
    {        
    }

    private static void CheckForNexLevel()
    {
        int count = 0;
        for (int x = 0; x < xPole; x++)
        {

            if (mainGrid[x, 0] == 1 && blockGrid[x, 0] != null && blockGrid[x,0].CompareTag("Cube"))
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
    


}
