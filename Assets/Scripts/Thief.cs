using GraphCreator;
using UnityEngine;

public class Thief : Agent
{
    private void Awake()
    {
        type = AgentType.THIEF;
    }
    
    public void Setup(Graph map,int id, Team team, double money, int startNode)
    {
        base.Setup(map, id, team, money);
        
        _currentNode = startNode;
        transform.position = _map.GetNodePositionById(_currentNode);
    }
}