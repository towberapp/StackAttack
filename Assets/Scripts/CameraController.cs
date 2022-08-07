using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        float targetaspect = 9.0f / 16.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;

        Camera camera = GetComponent<Camera>(); 

        camera.orthographicSize /= scaleheight;
        transform.position = new Vector3(transform.position.x, transform.position.y / scaleheight, transform.position.z);
    }

}
