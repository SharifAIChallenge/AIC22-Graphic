using System;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public int id;
    
    protected AgentType type;
    protected double money;

    private int _currentNode;

    [SerializeField] private SpriteRenderer hat;
    
    public void Setup(int id, Team team, double money)
    {
        this.id = id;
        this.money = money;
        switch (team)
        {
            case Team.FIRST:
                hat.color = Color.blue;
                break;
            default:
            case Team.SECOND:
                hat.color = Color.red;
                break;
        }
    }
    
    public void Move(int from, int to)
    {
        
    }
}

public enum AgentType
{
    POLICE,
    THIEF
}

public enum Team
{
    FIRST,
    SECOND
}
