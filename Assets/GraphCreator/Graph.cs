using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

namespace GraphCreator
{
    public class Graph : MonoBehaviour
    {
        public GameObject nodePrefab;
        public bool edgeMode = false;
        public Camera mainCamera;
        private readonly Dictionary<int, GameObject> _nodes = new Dictionary<int, GameObject>();
        private int _nextNodeIndex = 1;
        private Node _selectedNode;
        private readonly Dictionary<int, List<int>> _edges = new Dictionary<int, List<int>>();

        public void AddNode()
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;
            GameObject newNode = Instantiate(nodePrefab, worldPosition, Quaternion.identity, this.transform);
            _nodes.Add(_nextNodeIndex, newNode);
            Node nodeComponent = newNode.GetComponent<Node>();
            nodeComponent.SetId(_nextNodeIndex).SetGraph(this);
            _edges.Add(_nextNodeIndex, new List<int>());
            _nextNodeIndex++;
        }

        public void SelectNode(Node node)
        {
            if (_selectedNode is not null)
            {
                _selectedNode.Unselect();
                if (edgeMode && _selectedNode.id != node.id)
                {
                    AddEdge(_selectedNode.id, node.id);
                    node.Unselect();
                    _selectedNode = null;
                }
                else
                    _selectedNode = node;
            }
            else
            {
                _selectedNode = node;
            }

            
        }

        private void AddEdge(int selectedNodeID, int nodeID)
        {
            _edges[nodeID].Add(selectedNodeID);
            _edges[selectedNodeID].Add(nodeID);
            Debug.Log($"Node Added Between {selectedNodeID} AND {nodeID}");
        }


        public void ToggleEdgeMode()
        {
            edgeMode = !edgeMode;
        }
        public Vector2 GetNodePositionById(int id)
        {
            if (_nodes.ContainsKey(id))
                return _nodes[id].transform.position;
            else
                throw new Exception($"NO NODE WITH ID {id} EXISTS!");
        }

        /*public Vector2 GetPathPoint(int firstNodeId, int secondNodeId, float t)
        {
            Vector2 firstNodePosition;
            Vector2 secondNodePosition;
            if (_nodes.ContainsKey(firstNodeId))
                firstNodePosition = _nodes[firstNodeId].transform.position;
            else
                throw new Exception($"NO NODE WITH ID {firstNodeId} EXISTS!");
            if (_nodes.ContainsKey(secondNodeId))
                secondNodePosition = _nodes[secondNodeId].transform.position;
            else
                throw new Exception($"NO NODE WITH ID {secondNodeId} EXISTS!");
            if (t > 1)
                t = 1;
            else if (t < 0)
                t = 0;
            return Vector2.Lerp(firstNodePosition, secondNodePosition, t);
        }*/

        public Vector3[] GetPathPoint(int firstNodeId, int secondNodeId)
        {
            Vector2 firstNodePosition;
            Vector2 secondNodePosition;
            if (_nodes.ContainsKey(firstNodeId))
                firstNodePosition = _nodes[firstNodeId].transform.position;
            else
                throw new Exception($"NO NODE WITH ID {firstNodeId} EXISTS!");
            if (_nodes.ContainsKey(secondNodeId))
                secondNodePosition = _nodes[secondNodeId].transform.position;
            else
                throw new Exception($"NO NODE WITH ID {secondNodeId} EXISTS!");

            List<Vector3> waypoints = new List<Vector3>();
            waypoints.Add(secondNodePosition);
            waypoints.Add((firstNodePosition+secondNodePosition)/2);
            waypoints.Add((firstNodePosition+secondNodePosition)/2);
            
            return waypoints.ToArray();
        }
    }
}