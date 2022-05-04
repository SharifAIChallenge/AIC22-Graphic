using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Graph
{
    [SerializeField, HideInInspector]
    private List<Vector2> nodes;

    [SerializeField, HideInInspector]
    private List<(int firstNode, int secondNode)> edges;

    private int _nodeCount = 1;
    private int _edgeCount = 0;
    public int V => _nodeCount;
    public int E => _edgeCount;

    public Graph()
    {
        Vector2 firstNode = new Vector2(0, 0);
        nodes = new List<Vector2>
        {
            firstNode
        };
        edges = new List<(int firstNode, int secondNode)>();
    }

    public void AddNode(Vector2 position)
    {
        nodes.Add(position);
        _nodeCount++;
    }

    public void SetNodePosition(Vector2 newPosition, int id)
    {
        nodes[id] = newPosition;
    }

    public List<Vector2> GetNodes()
    {
        return nodes;
    }

    public List<(int, int)> GetEdges()
    {
        return edges;
    }

    public void AddEdge(int firstNode, int secondNode)
    {
        edges.Add((firstNode , secondNode));
        _edgeCount++;
    }
}
