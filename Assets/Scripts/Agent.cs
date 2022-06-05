using System;
using DG.Tweening;
using GraphCreator;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public int id;
    protected AgentType type;
    protected double money;
    protected int _currentNode;

    [SerializeField] private SpriteRenderer hat;
    
    protected Graph _map;

    public virtual void Setup(Graph map, int id, Team team, double money)
    {
        _map = map;
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
        transform.DOPath(_map.GetPathPoint(from, to), 1);
    }

    public void IncreaseBalance(double amount)
    {
        money += amount;
    }

    public void DecreaseBalance(double amount)
    {
        money -= amount;
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
