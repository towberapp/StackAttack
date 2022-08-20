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
        cam.orthographicSize = row / scaleheight;
        transform.position = new Vector3(sdvig, transform.position.y, transform.position.z);
    }


    void Start()
    {
        float targetaspect = 9.0f / 16.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        scaleheight = windowaspect / targetaspect;

        cam.orthographicSize /= scaleheight;
        //transform.position = new Vector3(transform.position.x, transform.position.y / scaleheight, transform.position.z);
    }

}
