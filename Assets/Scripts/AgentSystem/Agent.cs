using System;
using DG.Tweening;
using GraphCreator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

[JsonConverter(typeof(AgentJsonConverter))]
public abstract class Agent : MonoBehaviour
{
    public int id;
    protected AgentType type;
    protected double money;
    
    private int _currentNode;
    protected int CurrentNode
    {
        get => _currentNode;
        set
        {
            var oldNode = _map.GetNodeById(_currentNode);
            if (oldNode is not null)
            {
                oldNode.RemoveAgent(this);
            }
            var newNode = _map.GetNodeById(value);
            newNode.AddAgent(this);

            _currentNode = value;
        }
    }

    [SerializeField] private SpriteRenderer hat;
    [SerializeField] private GameObject agentDataPanel;
    [SerializeField] private TMP_Text idText;
    [SerializeField] private TMP_Text moneyText;

    protected Graph _map;


    private static float Speed => Config.agentsMoveSpeed;
    
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
        CurrentNode = startNode;
    }
    
    public void Move(int from, int to, bool isCaching)
    {
        if(from == to) return;

        if (isCaching)
        {
            CurrentNode = to;
            return;
        }
        
        var edge = _map.GetEdge(from, to);
        if (edge is null)
        {
            Debug.LogError("No Edge between " + from + " and " + to);
            return;
        }
        
        var length = edge.spline.GetLengthApproximately(0, 1);
        var duration = length / Speed;
        transform.DOPath(_map.GetPathPoint(from, to), duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            CurrentNode = to;
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
        idText.text = "ID: " + id;
        moneyText.text = "Money: " + money;
        agentDataPanel.SetActive(true);
    }

    private void OnMouseExit()
    {
        agentDataPanel.SetActive(false);
    }

    public JObject GetJsonObject()
    {
        var o = new JObject();
        o.Add("id", id);
        o.Add("type", type.ToString());
        o.Add("money", money);
        o.Add("currentNode", CurrentNode);

        return o;
    }
    
    public void LoadFromJson(JToken j)
    {
        var o = JObject.Parse(j.ToString());
        money = (double) o?["money"];
        CurrentNode = (int) o?["currentNode"];
    }
    
    public void SetMoney(double money)
    {
        this.money = money;
    }
    
    public void SetCurrentNode(int node)
    {
        CurrentNode = node;
        transform.position = _map.GetNodePositionById(CurrentNode);
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

public class AgentJsonConverter : JsonConverter<Agent>
{
    public override Agent ReadJson(JsonReader reader, Type objectType, Agent existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        /*Debug.Log(hasExistingValue);
        
        var o = JObject.Load(reader);
        var money = (double) o?["money"];
        var currentNode = (int) o?["currentNode"];
        existingValue.SetMoney(money);
        existingValue.SetCurrentNode(currentNode);

        return existingValue;*/
        throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
    }

    public override void WriteJson(JsonWriter writer, Agent value, JsonSerializer serializer)
    {
        writer.WriteRawValue(value.GetJsonObject().ToString(Formatting.None));
    }
}
