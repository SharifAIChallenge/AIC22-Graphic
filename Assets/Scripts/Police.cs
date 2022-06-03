using System;
using UnityEngine;

public class Police : Agent
{
    private void Awake()
    {
        type = AgentType.POLICE;
    }
}