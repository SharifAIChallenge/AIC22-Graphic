using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(GraphCreator))]
public class GraphEditor : Editor
{
    GraphCreator _creator;
    Graph _graph;
    private bool _isEdgeMode = false;
    private bool _isSelected = false;
    private int _selectedNode = -1;

    void OnEnable()
    {
        _creator = (GraphCreator) target;
        if (_creator.graph == null)
            _creator.graph = new Graph();
        _graph = _creator.graph;
        _isEdgeMode = _creator.isEdgeMode;
    }
    

    void OnSceneGUI()
    {
        _creator = (GraphCreator) target;
        _isEdgeMode = _creator.isEdgeMode;
        RecognizeSelectedNode();
        HandleKeyboardShortcut();
        Draw();
    }

    private void RecognizeSelectedNode()
    {
        int hashCode = target.name.GetHashCode();
        UnityEngine.Event current = UnityEngine.Event.current;
        switch (current.GetTypeForControl(hashCode))
        {
            case UnityEngine.EventType.MouseUp:
                List<Vector2> nodes = _graph.GetNodes();
                Vector2 mousePosition = current.mousePosition;
                for (int i = 0; i < _graph.V; i++)
                {
                    if ((GetWorldPositionOfMouse(mousePosition) - nodes[i]).magnitude < 0.2f)
                    {
                        if (_isEdgeMode && _selectedNode != -1 && _selectedNode != i)
                        {
                            Debug.Log("Edge between " + i + " and " + _selectedNode);
                            _graph.AddEdge(i, _selectedNode);
                        }
                        _selectedNode = i;
                        Debug.Log(i);
                        current.Use();
                        _isSelected = true;
                        break;
                    }
                }
                break;
        }
    }

    void Draw()
        {
            Handles.color = Color.cyan;
            List<Vector2> nodes = _graph.GetNodes();
            for (int i = 0; i < _graph.V; i++)
            {
                var newpos = (Vector2) Handles.FreeMoveHandle(nodes[i], Quaternion.identity, .1f, Vector2.zero,
                    Handles.CylinderHandleCap);
                if (!newpos.Equals(nodes[i]))
                    _graph.SetNodePosition(newpos, i);
            }
            List<(int, int)> edges = _graph.GetEdges();
            for (int i = 0; i < _graph.E; i++)
            {
                (int, int) edge = edges[i];
                Handles.DrawLine(nodes[edge.Item1], nodes[edge.Item2], 2);
            }
        }

    void HandleKeyboardShortcut()
    {
        Event ev = Event.current;
        switch (ev.type)
        {
            case EventType.KeyDown:
                KeyCode key = ev.keyCode;
                switch (key)
                {
                    case KeyCode.N:
                        _selectedNode = -1;
                        _graph.AddNode(GetWorldPositionOfMouse(ev.mousePosition));
                        break;
                    case KeyCode.D:
                        _selectedNode = -1;
                        break;
                }
                ev.Use();
                break;
        }
    }

    private Vector2 GetWorldPositionOfMouse(Vector2 mousePosition)
    {
        mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
        return mousePosition;
    }

    }

