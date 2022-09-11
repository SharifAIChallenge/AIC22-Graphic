using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    private Camera _cam;

    private float defaultDistance = 150;
    
    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        var p = Vector3.Project(_cam.transform.position - transform.position, transform.forward);
        var a = p.magnitude / defaultDistance;
        a = Mathf.Clamp(a, 0.8f, 10);
        transform.localScale = Vector3.one * a;

        var temp = transform.eulerAngles;
        temp.y = _cam.transform.eulerAngles.y;
        transform.eulerAngles = temp;

        temp = transform.localEulerAngles;
        temp.x = _cam.transform.localEulerAngles.x / 1.5f;
        transform.localEulerAngles = temp;
    }
}