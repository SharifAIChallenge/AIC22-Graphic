using System;
using System.Collections;
using System.Collections.Generic;
using GraphCreator;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int id;
    [HideInInspector][SerializeField] private Graph graph;

    private void Start()
    {
        if (graph.editMode)
        {
            gameObject.AddComponent<DragAndDrop>();
        }
    }

    public Node SetId(int id)
    {
        this.id = id; 
        this.GetComponentInChildren<TMP_Text>().text = id.ToString();
        return this;
    }

    public Node SetGraph(Graph graph)
    {
        this.graph = graph;
        return this;
    }
    
    private void OnMouseDown()
    {
        if(!graph.editMode) return;
        
        this.GetComponent<SpriteRenderer>().color = Color.cyan;
        graph.SelectNode(this);
    }

    public void Unselect()
    {
        //this.GetComponent<SpriteRenderer>().color = new Color(1.0f,0.3915f,0.3915f);
        this.GetComponent<SpriteRenderer>().color = Color.green;
    }
}
