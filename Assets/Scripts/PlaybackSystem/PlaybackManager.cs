using System;
using System.Globalization;
using UnityEngine;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class PlaybackManager : MonoBehaviour
{
    [SerializeField] private AgentsController agentsController;
    [SerializeField] private ChatManager chatManager;
    [SerializeField] private LogHandler logHandler;
    [SerializeField] private HUDManager hudManager;

    private GameStatus _gameStatus;
    public Action<GameStatus> onGameStatusChange = delegate { };

    private int _turnNumber;
    private AgentType _turnAgentType;
    public Action<int, AgentType> onTurnChange = delegate { };

    Regex agentId = new Regex("\"agentId\":\"([^\"]+)\"");
    Regex balance = new Regex("\"balance\":\"([^\"]+)\"");
    Regex team = new Regex("\"team\":\"([^\"]+)\"");
    Regex type = new Regex("\"type\":\"((?:POLICE)|(?:THIEF))\"");
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

    private void Start()
    {
        hudManager.Setup(this);
        ChangeTurn(1, AgentType.THIEF);
    }

    public void NextMove()
    {
        RunLine(logHandler.GetNextLine());
    }

    private void RunLine(string line)
    {
        var log = line.Substring(1, line.Length - 2);

        if (line.Contains("\"type\":\"READINESS_DECLARATION\""))
        {
            var parsed = JsonConvert.DeserializeObject<GameLog<ReadinessLog>>(log, new StringEnumConverter());
            if (parsed is null) return;

            var context = parsed.context;
            agentsController.CreateAgent(context.agentId, context.nodeId, context.type, context.team, context.balance);
        }
        else if (line.Contains("\"type\":\"STATUS_CHANGE\""))
        {
            var parsed = JsonConvert.DeserializeObject<GameLog<StatusChangeLog>>(log, new StringEnumConverter());
            if (parsed is null) return;

            ChangeGameStatus(parsed.context.toStatus);
        }
        else if (line.Contains("\"type\":\"AGENT_MOVEMENT\""))
        {
            var agentIdValue = int.Parse(GetValue(agentId, line));
            var balanceValue = GetValue(balance, line);
            var priceValue = double.Parse(GetValue(price, line), CultureInfo.InvariantCulture);
            var toNodeIdValue = int.Parse(GetValue(toNodeId, line));
            var fromNodeIdValue = int.Parse(GetValue(fromNodeId, line));

            agentsController.MoveAgent(agentIdValue, fromNodeIdValue, toNodeIdValue);
            agentsController.DecreaseBalance(agentIdValue, priceValue);
        }
        else if (line.Contains("\"type\":\"TURN_CHANGE\""))
        {
            var toTurnNumberValue = int.Parse(GetValue(toTurnNumber, line));
            var toTurnValue = GetValue(toTurn, line) == "THIEF" ? AgentType.THIEF : AgentType.POLICE;

            ChangeTurn(toTurnNumberValue, toTurnValue);
        }
        else if (line.Contains("\"type\":\"AGENT_BALANCE_CHARGED\""))
        {
            var agentIdValue = int.Parse(GetValue(agentId, line));
            var balanceValue = double.Parse(GetValue(balance, line), CultureInfo.InvariantCulture);
            var wageValue = double.Parse(GetValue(wage, line), CultureInfo.InvariantCulture);

            agentsController.BalanceCharge(agentIdValue, balanceValue, wageValue);
        }
        else if (line.Contains("\"type\":\"AGENT_SEND_MESSAGE\""))
        {
            var agentIdValue = int.Parse(GetValue(agentId, line));
            var priceValue = double.Parse(GetValue(price, line), CultureInfo.InvariantCulture);
            var teamValue = GetValue(team, line);
            var textValue = GetValue(text, line);
            var typeValue = GetValue(type, line);

            chatManager.UpdateChat(teamValue, typeValue, agentIdValue.ToString(), textValue);
            agentsController.DecreaseBalance(agentIdValue, priceValue);
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

    private void ChangeGameStatus(GameStatus toStatus)
    {
        _gameStatus = toStatus;

        switch (toStatus)
        {
            case GameStatus.PENDING:
                break;
            case GameStatus.ONGOING:
                agentsController.SortAgents();
                break;
            case GameStatus.FINISHED:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(toStatus), toStatus, null);
        }

        onGameStatusChange.Invoke(toStatus);
    }

    private void ChangeTurn(int turnNumber, AgentType turnType)
    {
        _turnNumber = turnNumber;
        _turnAgentType = turnType;
        onTurnChange?.Invoke(turnNumber, turnType);
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

    [Serializable]
    private class StatusChangeLog
    {
        public GameStatus fromStatus;
        public GameStatus toStatus;
    }
}

public enum GameStatus
{
    PENDING,
    ONGOING,
    FINISHED
}