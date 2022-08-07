using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{

    [SerializeField] private GameObject cube;    
    [SerializeField] private Sprite[] spriteBlock;
    

    private void Start()
    {
        GridController.blockGrid = new GameObject[GridController.xPole, GridController.yPole];
        SetBlockFromArray(GridController.mainGrid);      
    }

    private void SetBlockFromArray(int[,] arr)
    {
        for (int y = 0; y < GridController.yPole; y++)
        {
            for (int x = 0; x < GridController.xPole; x++)
            {
                if (arr[x, y] == 1)
                {
                    GameObject obj = Instantiate(cube, new Vector2(x, y), Quaternion.identity);

                    // set sprite
                    int spriteInt = Random.Range(0, spriteBlock.Length);
                    obj.GetComponent<SpriteRenderer>().sprite = spriteBlock[spriteInt];
                    GridController.blockGrid[x, y] = obj;
                }
            }
        }
    }

}
