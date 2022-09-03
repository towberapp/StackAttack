using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SystemUiController : MonoBehaviour
{


    private void OnSpeedChange(float sliderSpeed)
    {        
        MainConfig.speedMove = 0.75f - (sliderSpeed/2);
    }

    private void OnIntervalChange(float sliderData)
    {
        float koefInterval = 5.0f - (sliderData + 1) * 2; // from 1 to 2;
        MainConfig.intervalCube = koefInterval;
    }

}
