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
        Debug.Log("hi");
    }
}
