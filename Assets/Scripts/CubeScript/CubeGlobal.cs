using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGlobal : MonoBehaviour 
{
    [SerializeField] private SpriteRenderer sprite;

    public int jumpHeight;
    public Vector2Int moveLenta = Vector2Int.zero;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Flip(int x)
    {
        if (x > 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }
}
