using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCubeGenerator : MonoBehaviour
{

    [Header("Settings")]

    [SerializeField] [Range(0, 10)] private int specialCubePercent;

    [Header("Cubes")]

    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject cubeFolder;

    [SerializeField] private Sprite[] spriteBlock;
    [SerializeField] private GameObject[] specialBlock;

    private float time = 0.0f;
    private int lastRand = -1;

     
    private void Update()
    {
        if (!SystemStatic.isGameOver && SystemStatic.isStartGame) 
        { 
            time += Time.deltaTime;
            if (time >= MainConfig.intervalCube)
            {
                time = 0.0f;
                NewBlock();
            }
        }
    }


    private void NewBlock()
    {
        int x = GetEmptyCoord();        

        if (x >= 0)
        {
            GetNewBlock(x);
        } else 
        {
            EventsController.GameOverEvent.Invoke();
            SystemStatic.isGameOver = true;
            SystemStatic.isStartGame = false;
            Debug.LogWarning("GAME OVER");
        }
    }

    private void GetNewBlock(int x)
    {
        int y = GridController.yPole - 1;

        Vector2Int pos = new Vector2Int(x, y + SystemStatic.level);

        //random simple special
        int getRandom = Random.Range(1,11);
        
        MainConfig.countCubeSet++;

        GameObject obj;
        if (getRandom > specialCubePercent)
        {
            obj = GeneratSimpleBlock(pos);
        } else
        {
            obj = GenerateSpecialObj(pos);
        }

        obj.GetComponent<SpriteRenderer>().sortingOrder = MainConfig.countCubeSet + 10;

        MoveByGrid moveByGrid = obj.GetComponent<MoveByGrid>();
        moveByGrid.x = x;
        moveByGrid.y = y;

        GridController.blockGrid[x, y] = obj;
        GridController.mainGrid[x, y] = 1;
    }

    private GameObject GenerateSpecialObj(Vector2Int pos)
    {
        int blockInt = Random.Range(0, specialBlock.Length);        
        GameObject obj = Instantiate(specialBlock[blockInt], ((Vector3Int)pos), Quaternion.identity, cubeFolder.transform);

        return obj;
    }

    private GameObject GeneratSimpleBlock (Vector2Int pos)
    {
        GameObject obj = Instantiate(cube, ((Vector3Int)pos), Quaternion.identity, cubeFolder.transform);
        int spriteInt = Random.Range(0, spriteBlock.Length);
        obj.GetComponent<SpriteRenderer>().sprite = spriteBlock[spriteInt];

        return obj;
    }
    




    private int GetEmptyCoord()
    {
        int count = 0;
        List<int> termsList = new List<int>();

        for (int x = 0; x < GridController.xPole; x++)
        {
            if (GridController.mainGrid[x, GridController.yPole - 1] == 1)
            {
                count++;
            } else
            {
                termsList.Add(x);
            }                
        }


        if (count == GridController.xPole)
        {           
            return -1;
        }
        int[] terms = termsList.ToArray();
        
        int randItem = Random.Range(0, terms.Length);

        while (randItem == lastRand)
        {
            randItem = Random.Range(0, terms.Length);
        }

        lastRand = randItem;

        return terms[randItem];
    }


}