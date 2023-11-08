using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundParalax : MonoBehaviour
{
    [SerializeField] private float xKoef = 1;
    [SerializeField] private float yKoef = 1;

    private Vector2 pos;

    private void Awake()
    {
        pos = transform.position;
    }

    private void FixedUpdate()
    {
       //Debug.Log($"Pos: {MainConfig.playerFloatX}, {SystemStatic.level}");

        transform.position = pos - new Vector2(MainConfig.playerFloatX.x * xKoef, MainConfig.playerFloatX.y * yKoef);
    }

}
