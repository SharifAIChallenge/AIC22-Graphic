using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GraphCreator
{
    public class Graph : MonoBehaviour
    {
        public GameObject nodePrefab;
        public Camera mainCamera;
        private List<GameObject> _nodes = new List<GameObject>();
        private int _nextNodeIndex = 1;
        public void AddNode()
        {   Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;
            GameObject newNode = Instantiate(nodePrefab, worldPosition, Quaternion.identity,this.transform);
            _nodes.Add(newNode);
            newNode.GetComponent<Node>().SetId(_nextNodeIndex);
            _nextNodeIndex++;
        }
    }
}
