using System;
using GraphCreator;
using UnityEngine;

public class Police : Agent
{
    private void Awake()
    {
        type = AgentType.POLICE;
    }
    
    public override void UpdateViewState()
    {
        if(team == Team.FIRST)
        {
            visualGO.SetActive(Config.firstTeamPoliceView);
        }
        else
        {
            visualGO.SetActive(Config.secondTeamPoliceView);
        }
    }
}