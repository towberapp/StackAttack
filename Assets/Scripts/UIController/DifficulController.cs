using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficulClass
{
    public float speed;
    public float interval;

    public DifficulClass(float _speed, float _interval)
    {
        speed = _speed;
        interval = _interval;
    }
}


public class DifficulController : MonoBehaviour
{    
    private DifficulClass easy = new(0.75f, 5f);
    private DifficulClass medium = new(0.5f, 3.5f);
    private DifficulClass hard = new(0.3f, 2f);

    private List<DifficulClass> diff = new();

    [SerializeField] private List<Button> btn;

    private void Awake()
    {
        diff.Add(easy);
        diff.Add(medium);
        diff.Add(hard);
    }

    private void Start()
    {
        SelectDiff(1);
    }

    public void SelectDiff(int index)
    {
        for (int i = 0; i < btn.Count; i++)
        {
            if (i ==index)
            {
                ButtonOn(btn[i]);
                SetDifficul(i);
            } else
            {
                ButtonOff(btn[i]);
            }
        }
    }

    private void SetDifficul(int i)
    {
        MainConfig.speedMove = diff[i].speed;
        MainConfig.intervalCube = diff[i].interval;
    }

    private void ButtonOn(Button btn)
    {
        btn.GetComponent<Image>().color = Color.green;
    }

    private void ButtonOff(Button btn)
    {
        btn.GetComponent<Image>().color = Color.gray;
    }

}
