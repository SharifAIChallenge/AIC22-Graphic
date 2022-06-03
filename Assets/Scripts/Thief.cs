using UnityEngine;

public class Thief : Agent
{
    private void Awake()
    {
        type = AgentType.THIEF;
    }
    
    public void Setup(int id, Team team, double money, int startNode)
    {
        base.Setup(id, team, money);
    }
}