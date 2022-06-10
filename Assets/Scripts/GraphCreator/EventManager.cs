using System.Collections;
using System.Collections.Generic;
using GraphCreator;
using UnityEngine;
using UnityEngine.Serialization;

public class EventManager : MonoBehaviour
{
    public Graph theGraph;
    public Camera mainCamera;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            theGraph.AddNode();
        else if (Input.GetKeyDown(KeyCode.E))
            theGraph.ToggleEdgeMode();
    }

    /*public void ZoomOut()
    {
        mainCamera.orthographicSize += 0.1f;
        Vector3 newPos = mainCamera.transform.position  -(mainCamera.transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition)) / 10;
        newPos.z = -10;
        mainCamera.transform.position = newPos;


    }
    
    public void ZoomIn()
    {
        mainCamera.orthographicSize -= 0.1f;
        Vector3 newPos = mainCamera.transform.position  -(mainCamera.transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition)) / 10;
        newPos.z = -10;
        mainCamera.transform.position = newPos;
    }*/
}
