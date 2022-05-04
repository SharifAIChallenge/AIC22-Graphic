using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCreator : MonoBehaviour
{
    [HideInInspector]
    public Graph graph;

    [SerializeField]
    public bool isEdgeMode = false;
}
