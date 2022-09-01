using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePersonalMoveController : MonoBehaviour
{

    CubeGlobal cg;

    private void Awake()
    {
        cg = GetComponent<CubeGlobal>();
    }

    private void Start()
    {
        int rnd = Random.Range(0, 2) * 2 - 1;
        cg.moveLenta = new Vector2Int(rnd, 0);
        cg.Flip(rnd);
    }
}
