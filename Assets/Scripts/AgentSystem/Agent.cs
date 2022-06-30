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
    protected int _currentNode;

    [SerializeField] private SpriteRenderer hat;
    [SerializeField] private GameObject agentDataPanel;
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
        _currentNode = startNode;
        transform.position = _map.GetNodePositionById(_currentNode);
    }
    
    public void Move(int from, int to, bool isCaching)
    {
        if(from == to) return;

        if (isCaching)
        {
            _currentNode = to;
            return;
        }
        
        var length = _map.GetEdge(from, to).spline.GetLengthApproximately(0, 1);
        var duration = length / Speed;
        transform.DOPath(_map.GetPathPoint(from, to)[1..], duration).SetEase(Ease.Linear).OnComplete(() =>
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

    public JObject GetJsonObject()
    {
        var o = new JObject();
        o.Add("id", id);
        o.Add("type", type.ToString());
        o.Add("money", money);
        o.Add("currentNode", _currentNode);

        return o;
    }
    
    public void LoadFromJson(JToken j)
    {
        var o = JObject.Parse(j.ToString());
        money = (double) o?["money"];
        _currentNode = (int) o?["currentNode"];
        transform.position = _map.GetNodePositionById(_currentNode);
    }
    
    public void SetMoney(double money)
    {
        this.money = money;
    }
    
    public void SetCurrentNode(int node)
    {
        _currentNode = node;
        transform.position = _map.GetNodePositionById(_currentNode);
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
