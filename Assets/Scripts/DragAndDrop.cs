using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 _dragOffset;
    private void OnMouseDrag()
    {

        transform.position = GetMousePosition() + _dragOffset;
    }
    private void OnMouseDown()
    {
        _dragOffset = transform.position - GetMousePosition();
    }

    private Vector3 GetMousePosition()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
