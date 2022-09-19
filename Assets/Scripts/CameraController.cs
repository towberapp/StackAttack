using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Camera cam;
    private float scaleheight;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        EventsController.PreStartEvent.AddListener(OnPreStartGame);
    }

    private void OnPreStartGame(int row)
    {    
        float sdvig = (row / 2.0f) - 0.5f;
        cam.orthographicSize = (row / scaleheight)* 1.085f;

        Debug.Log("row: " + row);
        Debug.Log("scaleheight" + scaleheight);
        Debug.Log("scaleheight 2: " + cam.orthographicSize);

        // scale: 0,88888, row 9. cam: 10,12 - norm 11   \\ 1.088
        // scale: 1, row 9, cam 9 - norm 9.75

        transform.position = new Vector3(sdvig, transform.position.y, transform.position.z);
    }


    void Start()
    {
        float targetaspect = 9.0f / 16.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        scaleheight = windowaspect / targetaspect;
        cam.orthographicSize /= scaleheight;

        Debug.Log("scaleheight: " + scaleheight);

        //transform.position = new Vector3(transform.position.x, transform.position.y / scaleheight, transform.position.z);
    }

}
