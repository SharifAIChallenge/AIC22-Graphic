using System;
using System.Collections;
using System.Collections.Generic;
using GraphCreator;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int id;
    private Graph _graph;
    
    public Node SetId(int id)
    {
        this.id = id; 
        this.GetComponentInChildren<TextMeshPro>().text = id.ToString();
        return this;
    }

    private void OnMouseDown()
    {
        this.GetComponent<SpriteRenderer>().color = Color.cyan;
        _graph.SelectNode(this);
    }

    public void Unselect()
    {
        this.GetComponent<SpriteRenderer>().color = new Color(1.0f,0.3915f,0.3915f);
    }

    public Node SetGraph(Graph graph)
    {
        _graph = graph;
        return this;
    }
    
    
}
