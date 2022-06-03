using System;
using System.Collections.Generic;
using System.Linq;
using GraphCreator;
using UnityEngine;

public class AgentsController : MonoBehaviour
{
    [SerializeField] private Police policePrefab;
    [SerializeField] private Thief thiefPrefab;

    [SerializeField] private Transform agentsParent;
    
    [SerializeField] private Graph mapPrefab;
    [SerializeField] private Transform mapParent;
    
    private Graph map;

    /*private List<Agent> _team1_Cop = new();
    private List<Agent> _team1_Thief = new();
    private List<Agent> _team2_Cop = new();
    private List<Agent> _team2_Thief = new();*/

    private List<Agent> _agents = new();

    private void Awake()
    {
        map = Instantiate(mapPrefab, mapParent);
    }

    public void CreateAgent(int id, int startNode, AgentType agentType, Team team, double balanceValue)
    {
        if (agentType == AgentType.POLICE)
        {
            var c = Instantiate(policePrefab, agentsParent);
            c.Setup(map, id, team, balanceValue);
            _agents.Add(c);
        }
        else
        {
            var c = Instantiate(thiefPrefab, agentsParent);
            c.Setup(map, id, team, balanceValue, startNode);
            _agents.Add(c);
        }
    }

    public void MoveAgent(int id, int from, int to)
    {
        _agents.FirstOrDefault(c => c.id == id)?.Move(from, to);
    }
}