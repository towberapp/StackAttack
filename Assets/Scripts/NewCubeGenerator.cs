using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCubeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject cubeFolder;

    [SerializeField] private Sprite[] spriteBlock; 

    private float time = 0.0f;


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

        int y = GridController.yPole-1;

        GameObject obj = Instantiate(cube, new Vector2(x, y + SystemStatic.level), Quaternion.identity, cubeFolder.transform);

        MainConfig.countCubeSet++;

        int spriteInt = Random.Range(0, spriteBlock.Length);
        obj.GetComponent<SpriteRenderer>().sprite = spriteBlock[spriteInt];
        obj.GetComponent<SpriteRenderer>().sortingOrder = MainConfig.countCubeSet + 10;

        MoveByGrid moveByGrid = obj.GetComponent<MoveByGrid>();
        moveByGrid.x = x;
        moveByGrid.y = y;

        GridController.blockGrid[x, y] = obj;
        GridController.mainGrid[x, y] = 1;

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

        //print("xPole: " + (GridController.xPole - 1));
        //print("count: " + count);

        // if net mest
        if (count == GridController.xPole)
        {           
            return -1;
        }

        int[] terms = termsList.ToArray();
        int randItem = Random.Range(0, terms.Length);

        return terms[randItem];
    }


}
