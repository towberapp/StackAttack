using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerate : MonoBehaviour
{
    [SerializeField] private GameObject folderBackground;
    [SerializeField] private GameObject backCube;


    private void Start()
    {
        for (int y = 0; y < GridController.yPole; y++)
        {
            for (int x = 0; x < GridController.xPole; x++)
            {
               GameObject cube  = Instantiate(backCube, new Vector2(x, y), Quaternion.identity, folderBackground.transform);

                Color col = cube.GetComponent<SpriteRenderer>().color;
                col.a = Random.Range(0.3f, 0.8f);   
                cube.GetComponent<SpriteRenderer>().color = col;
            }
        }
    }
}
