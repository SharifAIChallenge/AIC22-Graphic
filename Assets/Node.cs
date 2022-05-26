using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int id;
    
    public void SetId(int id)
    {
        this.id = id; 
        this.GetComponentInChildren<TextMeshPro>().text = id.ToString();
    }

    private void OnMouseDown()
    {
        EventManager.SelectNode(this);
        this.GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    public void Unselect()
    {
        this.GetComponent<SpriteRenderer>().color = new Color(1.0f,0.3915f,0.3915f);
    }
}
