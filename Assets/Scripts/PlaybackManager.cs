using System;
using UnityEngine;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class PlaybackManager : MonoBehaviour
{
    [SerializeField] private AgentsController agentsController;
    [SerializeField] private LogHandler logHandler;

    Regex agentId = new Regex("\"agentId\":\"([^\"]+)\"");
    Regex balance = new Regex("\"balance\":\"([^\"]+)\"");
    Regex team = new Regex("\"team\":\"([^\"]+)\"");
    Regex type = new Regex("\"type\":\"([^\"]+)\"");
    Regex nodeId = new Regex("\"nodeId\":\"([^\"]+)\"");
    Regex token = new Regex("\"token\":\"([^\"]+)\"");
    Regex toStatus = new Regex("\"toStatus\":\"([^\"]+)\"");
    Regex fromStatus = new Regex("\"fromStatus\":\"([^\"]+)\"");
    Regex price = new Regex("\"price\":\"([^\"]+)\"");
    Regex toNodeId = new Regex("\"toNodeId\":\"([^\"]+)\"");
    Regex fromNodeId = new Regex("\"fromNodeId\":\"([^\"]+)\"");
    Regex toTurnNumber = new Regex("\"toTurnNumber\":\"([^\"]+)\"");
    Regex toTurn = new Regex("\"toTurn\":\"([^\"]+)\"");
    Regex wage = new Regex("\"wage\":\"([^\"]+)\"");
    Regex text = new Regex("\"text\":\"([^\"]*)\"");
    Regex thiefId = new Regex("\"thiefId\":\"([^\"]+)\"");
    Regex gameResult = new Regex("\"game result\":\"([^\"]+)\"");

    public void NextMove()
    {
        RunLine(logHandler.GetNextLine());
    }

    private void RunLine(string line)
    {
        if (line.Contains("\"type\":\"READINESS_DECLARATION\""))
        {
            var log = line.Substring(1, line.Length - 2);
            var parsed = JsonConvert.DeserializeObject<GameLog<ReadinessLog>>(log, new StringEnumConverter());
            if (parsed is null) return;
            
            var context = parsed.context;
            agentsController.CreateAgent(context.agentId, context.nodeId, context.type, context.team, context.balance);

            /*var agentIdValue = int.Parse(GetValue(agentId, line));
            var balanceValue = double.Parse(GetValue(balance, line), CultureInfo.InvariantCulture);
            var teamValue = GetValue(team, line) == "FIRST" ? Team.FIRST : Team.SECOND;
            var typeValue = GetValue(type, line) == "POLICE" ? AgentType.POLICE : AgentType.THIEF;
            var nodeIdValue = int.Parse(GetValue(nodeId, line));
            var tokenValue = GetValue(token, line);

            agentsController.CreateAgent(agentIdValue, nodeIdValue, typeValue, teamValue, balanceValue);*/
        }
        else if (line.Contains("\"type\":\"STATUS_CHANGE\""))
        {
            var toStatusValue = GetValue(toStatus, line);
            var fromStatusValue = GetValue(fromStatus, line);
            //call function
        }
        else if (line.Contains("\"type\":\"AGENT_MOVEMENT\""))
        {
            var agentIdValue = int.Parse(GetValue(agentId, line));
            var balanceValue = GetValue(balance, line);
            var priceValue = GetValue(price, line);
            var toNodeIdValue = int.Parse(GetValue(toNodeId, line));
            var fromNodeIdValue = int.Parse(GetValue(fromNodeId, line));
            
            agentsController.MoveAgent(agentIdValue, fromNodeIdValue, toNodeIdValue);
        }
        else if (line.Contains("\"type\":\"TURN_CHANGE\""))
        {
            var toTurnNumberValue = GetValue(toTurnNumber, line);
            var toTurnValue = GetValue(toTurn, line);
            //call function
        }
        else if (line.Contains("\"type\":\"AGENT_BALANCE_CHARGED\""))
        {
            var agentIdValue = GetValue(agentId, line);
            var balanceValue = GetValue(balance, line);
            var wageValue = GetValue(wage, line);
            //call function
        }
        else if (line.Contains("\"type\":\"AGENT_SEND_MESSAGE\""))
        {
            var agentIdValue = GetValue(agentId, line);
            var balanceValue = GetValue(balance, line);
            var priceValue = GetValue(price, line);
            var teamValue = GetValue(team, line);
            var textValue = GetValue(text, line);
            var typeValue = GetValue(type, line);
            //call function
        }
        else if (line.Contains("\"type\":\"POLICES_CAUGHT_THIEVES\""))
        {
            var thiefIdValue = GetValue(thiefId, line);
            var nodeIdValue = GetValue(nodeId, line);
            //call function
        }
        else if (line.Contains("\"type\":\"GAME_RESULT_CHANGED\""))
        {
            var gameResultValue = GetValue(gameResult, line);
            //call function
        }
    }

    static string GetValue(Regex regex, string str)
    {
        Match match = regex.Match(str);
        return match.Groups[1].Value;
    }

    [Serializable]
    private class GameLog<T>
    {
        public string id;
        public string timeStamp;
        public string type;
        public string message;
        public T context;
    }
    [Serializable]
    private class ReadinessLog
    {
        public int agentId;
        public double balance;
        public Team team;
        public AgentType type;
        public int nodeId;
        public string token;
    }
}

