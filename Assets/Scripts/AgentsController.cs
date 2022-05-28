using System.Collections.Generic;
using UnityEngine;

public class AgentsController : MonoBehaviour
{
    [SerializeField] private Agent copPrefab;
    [SerializeField] private Agent thiefPrefab;

    private List<Agent> _team1_Cop = new();
    private List<Agent> _team1_Thief = new();
    private List<Agent> _team2_Cop = new();
    private List<Agent> _team2_Thief = new();

    public void CreateAgent(int id, int startNode, AgentType agentType, int team)
    {
        
    }

    public void MoveAgent(int id, int from, int to)
    {
        
    }
}