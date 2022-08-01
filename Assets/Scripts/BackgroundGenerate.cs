using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerate : MonoBehaviour
{
    [SerializeField] private GameObject backCube;

    private void Start()
    {
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 11; x++)
            {
               GameObject cube  = Instantiate(backCube, new Vector2(x, y), Quaternion.identity);

                Color col = cube.GetComponent<SpriteRenderer>().color;
                col.a = Random.Range(0.1f, 0.3f);   
                cube.GetComponent<SpriteRenderer>().color = col;
            }
        }
    }
}
