using System;
using System.Collections.Generic;
using System.Linq;
using GraphCreator;
using UnityEngine;

public class AgentsController : MonoBehaviour
{
    [SerializeField] private MapManager _mapManager;
    
    [SerializeField] private Police policePrefab;
    [SerializeField] private Thief thiefPrefab;

    [SerializeField] private Transform agentsParent;

    /*private List<Agent> _team1_Cop = new();
    private List<Agent> _team1_Thief = new();
    private List<Agent> _team2_Cop = new();
    private List<Agent> _team2_Thief = new();*/

    private List<Agent> _agents = new();

    private Graph map => _mapManager.Map;
    
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

    
    
    public void SortAgents()
    {
        _agents.Sort((a, b) => a.id - b.id);
    }

    public void MoveAgent(int id, int from, int to)
    {
        _agents[id - 1].Move(from, to);
    }

    public void BalanceCharge(int agentId, double balance, double wage)
    {
        _agents[agentId - 1].IncreaseBalance(wage);
    }

    public void DecreaseBalance(int agentId, double amount)
    {
        _agents[agentId - 1].DecreaseBalance(amount);
    }
}