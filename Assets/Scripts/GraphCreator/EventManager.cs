using System.Collections;
using System.Collections.Generic;
using GraphCreator;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    public void ChangeEdgeEditMode(Toggle toggle)
    {
        if (toggle.gameObject.transform.name.Contains("Road"))
        {
            if (toggle.isOn)
                theGraph.edgeEditType = EdgeType.ROAD;
        }
        else if (toggle.gameObject.transform.name.Contains("Bus"))
        {
            if (toggle.isOn)
                theGraph.edgeEditType = EdgeType.BUS;
        }
        else if (toggle.gameObject.transform.name.Contains("Train"))
                    if (toggle.isOn)
                        theGraph.edgeEditType = EdgeType.TRAIN;

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
