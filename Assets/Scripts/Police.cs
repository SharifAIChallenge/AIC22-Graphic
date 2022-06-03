using System;
using GraphCreator;
using UnityEngine;

public class Police : Agent
{
    private void Awake()
    {
        type = AgentType.POLICE;
    }


    public override void Setup(Graph map, int id, Team team, double money)
    {
        base.Setup(map, id, team, money);

        _currentNode = map.GetPoliceStation(team);
        transform.position = _map.GetNodePositionById(_currentNode);
    }
}