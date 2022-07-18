using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GraphCreator
{
    public class Graph : MonoBehaviour
    {
        [SerializeField] public bool editMode;

        public Node nodePrefab;
        public bool edgeMode = false;
        public EdgeType edgeEditType = EdgeType.ROAD;
        public Camera mainCamera;
        [SerializeField] private NodeDictionary _nodes = new();
        private int _nextNodeIndex = 1;
        private Node _selectedNode;
        [SerializeField] private EdgeDictionary _edges = new();

        [SerializeField] private Edge edgePrefab;
        public float roadCost = 0, busCost = 10, trainCost = 100;

        [Space]
        [SerializeField] private string yamlFileAddress;
        [SerializeField] private string jsonFileAddress;

        public void AddNode()
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.y = 0;
            
            AddNode(_nextNodeIndex, worldPosition);
            
            _nextNodeIndex++;
        }

        public void AddNode(int nodeId, Vector3 position)
        {
            var newNode = Instantiate(nodePrefab, position, Quaternion.identity, this.transform);
            newNode.transform.localRotation = Quaternion.identity;
            _nodes.Add(nodeId, newNode);
            var nodeComponent = newNode.GetComponent<Node>();
            nodeComponent.SetId(nodeId).SetGraph(this);
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


        private MyTuple GetEdgeTuple(int firstNodeID, int secondNodeID)
        {
            var bigger = (firstNodeID < secondNodeID) ? secondNodeID : firstNodeID;
            var smaller = (firstNodeID < secondNodeID) ? firstNodeID : secondNodeID;
            var edgeTuple = new MyTuple(smaller, bigger);
            return edgeTuple;
        }

        public void AddEdge(int selectedNodeID, int nodeID)
        {
            AddEdge(selectedNodeID, nodeID, edgeEditType);
        }
        
        public void AddEdge(int firstNode, int secondNode, EdgeType edgeType)
        {
            Edge e;
            var edgeTuple = GetEdgeTuple(firstNode, secondNode);
            if (_edges.Keys.Contains(edgeTuple))
            {
                e = _edges[edgeTuple];
                e.SetTransportationMethod(edgeType);
                Debug.Log($"{edgeType} Set to existing edge Between {firstNode} AND {secondNode}");
                return;
            }

            Debug.Log($"Edge Added Between {firstNode} AND {secondNode} OF TYPE {edgeType}");
            e = Instantiate(edgePrefab, transform);
            e.Setup(_nodes[edgeTuple.Item1].transform, _nodes[edgeTuple.Item2].transform, edgeType);
            _edges[edgeTuple] = e;
        }

        public void ToggleEdgeMode()
        {
            edgeMode = !edgeMode;
        }

        public Vector3 GetNodePositionById(int id)
        {
            if (_nodes.ContainsKey(id))
                return _nodes[id].transform.position;
            else
                throw new Exception($"NO NODE WITH ID {id} EXISTS!");
        }

        public Vector3[] GetPathPoint(int firstNodeId, int secondNodeId)
        {
            var tuple = GetEdgeTuple(firstNodeId, secondNodeId);
            if (_edges.ContainsKey(tuple))
            {
                var edge = _edges[tuple];
                var waypoints = edge.spline.Select(c => c.position);
                if (firstNodeId > secondNodeId)
                {
                    waypoints = waypoints.Reverse();
                }

                return waypoints.ToArray();
            }
            else
            {
                throw new Exception($"Edge doesnt exist between {firstNodeId} AND {secondNodeId}");
            }
        }

        public Edge GetEdge(int firstNodeId, int secondNodeId)
        {
            var edgeTuple = GetEdgeTuple(firstNodeId, secondNodeId);
            if (_edges.Keys.Contains(edgeTuple))
                return _edges[edgeTuple];
            return null;
        }

        public void BuildYaml()
        {
            string yamlString = "graph:\n" + "  paths:\n";
            int i = 1;
            foreach (var edgesKey in _edges.Keys)
            {
                Edge edge = _edges[edgesKey];

                Tuple<string,int> infoTuple = edge.GetPathsInformation(i , edgesKey, roadCost, busCost, trainCost);
                yamlString += infoTuple.Item1;
                i = infoTuple.Item2;
            }

            yamlString += "  nodes:\n";
            foreach (KeyValuePair<int, Node> node in _nodes)
            {
                int index = node.Key;
                yamlString += "    - id: " + index + "\n";
            }
            
            Debug.Log(yamlString);
            
            using var writer = new StreamWriter(yamlFileAddress);
            writer.WriteLine(yamlString);
        }

        public void BuildMapJson()
        {
            var map = new GraphJsonData();
            
            foreach (var pair in _nodes)
            {
                var node = new NodeJsonData{
                    id = pair.Key,
                    position = pair.Value.transform.position
                };
                map.nodes.Add(node);
            }
            
            foreach (var pair in _edges)
            {
                var edge = new EdgeJsonData
                {
                    node1Id = pair.Key.Item1,
                    node2Id = pair.Key.Item2,
                    edgeType = pair.Value.edgeType
                };
                map.edges.Add(edge);
            }
            
            /*Debug.Log(JsonConvert.SerializeObject(map,new JsonSerializerSettings() {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));*/
            var json = JsonUtility.ToJson(map);
            Debug.Log(json + "\n");

            using var writer = new StreamWriter(jsonFileAddress);
            writer.WriteLine(json);
        }

        public Node GetNodeById(int id)
        {
            if (_nodes.ContainsKey(id))
                return _nodes[id];

            return null;
        }
        
        public void LoadMapJson()
        {
            string json = File.ReadAllText(jsonFileAddress);
            var parsed = JsonUtility.FromJson<GraphJsonData>(json);

            foreach (var node in parsed.nodes)
            {
                AddNode(node.id, node.position);
            }

            foreach (var edge in parsed.edges)
            {
                AddEdge(edge.node1Id, edge.node2Id, edge.edgeType);
            }
        }
    }

    [Serializable]
    public class NodeDictionary : SerializableDictionary<int, Node>
    {
    }

    [Serializable]
    public class EdgeDictionary : SerializableDictionary<MyTuple, Edge>
    {
    }

    [Serializable]
    public struct MyTuple
    {
        public int Item1;
        public int Item2;
        
        public MyTuple(int item1, int item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}