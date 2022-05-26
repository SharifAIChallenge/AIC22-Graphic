using System.Collections;
using System.Collections.Generic;
using GraphCreator;
using UnityEngine;
using UnityEngine.Serialization;

public class EventManager : MonoBehaviour
{
    public Graph theGraph;
    private static Node _selectedNode;
    public Camera mainCamera;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            theGraph.AddNode();
        else if (Input.GetKeyDown(KeyCode.O))
            ZoomOut();
        else if (Input.GetKeyDown(KeyCode.I))
            ZoomIn();
    }

    public static void SelectNode(Node node)
    {
        if (_selectedNode is not null)
            _selectedNode.Unselect();
        _selectedNode = node;
    }

    public void ZoomOut()
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
    }
    
}
