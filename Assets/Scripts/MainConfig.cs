using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainConfig : MonoBehaviour
{
    [SerializeField] private float _speedMove = 1f;

    public static float speedMove;

    private void Awake()
    {
        speedMove = _speedMove;
    }
}
