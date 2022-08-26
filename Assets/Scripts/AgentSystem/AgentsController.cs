using System.Collections.Generic;
using System.Linq;
using GraphCreator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class AgentsController : Cacheable
{
    [SerializeField] private MapManager _mapManager;
    
    [SerializeField] private Police policePrefab;
    [SerializeField] private Thief thiefPrefab;

    [SerializeField] private Transform agentsParent;

    //private List<Agent> _agents = new();
    private Dictionary<int, Agent> _agents = new();

    private Graph map => _mapManager.Map;
    
    public void CreateAgent(int id, int startNode, AgentType agentType, Team team, double balanceValue)
    {
        if (agentType == AgentType.POLICE)
        {
            var c = Instantiate(policePrefab, agentsParent);
            c.Setup(map, id, team, balanceValue, startNode);
            _agents.Add(id, c);
        }
        else
        {
            var c = Instantiate(thiefPrefab, agentsParent);
            c.Setup(map, id, team, balanceValue, startNode);
            _agents.Add(id, c);
        }
    }

    public void MoveAgent(int id, int from, int to)
    {
        try
        {
            _agents[id].Move(from, to, IsCaching);
        }
        catch (KeyNotFoundException)
        {
            Debug.LogError($"Agent with id {id} not found");
        }
    }

    public void BalanceCharge(int agentId, double balance, double wage)
    {
        try{
            _agents[agentId].IncreaseBalance(wage);
        }
        catch (KeyNotFoundException)
        {
            Debug.LogError($"Agent with id {agentId} not found");
        }
    }

    public void DecreaseBalance(int agentId, double amount)
    {
        try{
            _agents[agentId].DecreaseBalance(amount);
        }
        catch (KeyNotFoundException)
        {
            Debug.LogError($"Agent with id {agentId} not found");
        }
    }

    public override void SaveState()
    {
        var json = JsonConvert.SerializeObject(_agents, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        history.Add(json);
    }
    
    public override void LoadState(int index)
    {
        var json = history[index];

        var dict = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(json);
        
        foreach (var (id, jObject) in dict)
        {
            _agents[int.Parse(id)].LoadFromJson(jObject.ToString(Formatting.None));
        }
    }

    public void ThiefCaught(int thiefId)
    {
        try
        {
            ((Thief) _agents[thiefId]).Caught();
        }
        catch (KeyNotFoundException)
        {
            Debug.LogError($"Agent with id {thiefId} not found");
        }
    }
    
    public void ChangeVisibleState(bool visible)
    {
        foreach (var (id, agent) in _agents.Where(agent => agent.Value.type == AgentType.THIEF))
        {
            ((Thief) agent).IsVisible = visible;
        }
    }

    public void UpdateAgentsView()
    {
        foreach (var agent in _agents)
        {
            agent.Value.UpdateViewState();
        }
    }
}