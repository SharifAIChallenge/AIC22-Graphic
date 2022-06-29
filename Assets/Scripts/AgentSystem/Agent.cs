using System;
using DG.Tweening;
using GraphCreator;
using TMPro;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public int id;
    protected AgentType type;
    protected double money;
    protected int _currentNode;

    [SerializeField] private SpriteRenderer hat;
    [SerializeField] private GameObject agentDataPanel;
    [SerializeField] private TMP_Text moneyText;
    
    protected Graph _map;

    public void Setup(Graph map, int id, Team team, double money, int startNode)
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
        _currentNode = startNode;
        transform.position = _map.GetNodePositionById(_currentNode);
    }
    
    public void Move(int from, int to)
    {
        if(from == to) return;
        
        transform.DOPath(_map.GetPathPoint(from, to)[1..], 1.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            _currentNode = to;
        });
    }

    public void IncreaseBalance(double amount)
    {
        money += amount;
    }

    public void DecreaseBalance(double amount)
    {
        money -= amount;
    }

    private void OnMouseEnter()
    {
        moneyText.text = "Money: " + money;
        agentDataPanel.SetActive(true);
    }

    private void OnMouseExit()
    {
        agentDataPanel.SetActive(false);
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
