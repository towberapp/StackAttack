using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text level;

    private void Awake()
    {
        EventsController.NextLevelEvent.AddListener(OnLevelChange);
    }

    private void OnLevelChange()
    {
        level.text = SystemStatic.level.ToString();
    }

}
