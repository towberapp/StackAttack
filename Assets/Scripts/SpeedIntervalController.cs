using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIntervalController : MonoBehaviour
{
    private void Awake()
    {
        EventsController.NextLevelEvent.AddListener(OnLevelUp);
    }

    private void OnLevelUp()
    {
        //speed up
        if (MainConfig.speedMove >= 0.25)
            MainConfig.speedMove -= 0.01f;

        //interval up
        if (MainConfig.intervalCube >= 1)
            MainConfig.intervalCube -= 0.1f;        
    }
}
