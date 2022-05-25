using System.Collections;
using System.Collections.Generic;
using GraphCreator;
using UnityEngine;
using UnityEngine.Serialization;

public class EventManager : MonoBehaviour
{
    public Graph theGraph;
    void Update()
    {
         if(Input.GetKeyDown(KeyCode.N))
             theGraph.AddNode();
            
    }
}
