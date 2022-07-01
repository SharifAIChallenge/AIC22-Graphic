using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        var temp = transform.eulerAngles;
        temp.y = _cam.transform.eulerAngles.y;
        transform.eulerAngles = temp;

        temp = transform.localEulerAngles;
        temp.x = _cam.transform.localEulerAngles.x;
        transform.localEulerAngles = temp;
    }
}