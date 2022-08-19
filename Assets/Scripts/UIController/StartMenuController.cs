using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartMenuController : MonoBehaviour
{

    [SerializeField] private GameObject startMenu;
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_Dropdown selectRow;


    private void Awake()
    {
        startButton.onClick.AddListener(OnStart);
        startMenu.SetActive(true);
    }

    private void OnStart()
    {            
        startMenu.SetActive(false);

        int row = Convert.ToInt16(selectRow.options[selectRow.value].text);
        EventsController.PreStartEvent.Invoke(row);
    }

}
 
