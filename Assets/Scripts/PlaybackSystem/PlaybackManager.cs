using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

    public int TurnNumber => _turnNumber;
    
    public GameStatus GameStatus => _gameStatus;

    private AgentType _turnAgentType;
    public Action<int, AgentType> onTurnChange = delegate { };

    private GameResult _finalResult;
    
    #region regexes
    
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
    Regex isVisible = new Regex("\"isVisible\":\"([^\"]+)\"");
    Regex wage = new Regex("\"wage\":\"([^\"]+)\"");
    Regex text = new Regex("\"text\":\"([^\"]*)\"");
    Regex thiefId = new Regex("\"thiefId\":\"([^\"]+)\"");
    Regex gameResult = new Regex("\"game result\":\"([^\"]+)\"");

    #endregion
    
    private List<Cacheable> _cacheables = new();
    private Dictionary<int, int> turnStartsLineNumbers = new();
    private bool _isCaching;

    private Coroutine autoPlayCoroutine;

    private void Awake()
    {
        _cacheables.Add(agentsController);
        _cacheables.Add(chatManager);
        hudManager.Setup(this);
    }

    public void CreateCache()
    {
        _isCaching = true;
        _cacheables.ForEach(c => c.IsCaching = true);

        var line = logHandler.GetNextLine();
        while (line is not null)
        {
            RunLine(line);
            line = logHandler.GetNextLine();
        }
        Config.lastTurn = _turnNumber;

        _cacheables.ForEach(c => c.IsCaching = false);
        _isCaching = false;
        
        FirstSetupGame();
    }

    private void FirstSetupGame()
    {
        LoadTurn(1);
    }

    public void Play()
    {
        autoPlayCoroutine = StartCoroutine(AutoPlay());
    }

    public void Pause()
    {
        StopCoroutine(autoPlayCoroutine);
    }

    private IEnumerator AutoPlay()
    {
        while (GameStatus != GameStatus.FINISHED)
        {
            NextMove();
            yield return new WaitForSeconds(0.7f);
        }

        hudManager.TogglePlayBack();
    }
    
    public void NextMove()
    {
        var line = logHandler.GetNextLine();

        if(line is null)
        {
            return;
        }
        RunLine(line);
    }

    public void PreviousTurn()
    {
        LoadTurn(_turnNumber - 1);
    }
    
    public void NextTurn()
    {
        LoadTurn(_turnNumber + 1);
    }
    
    public void LoadTurn(int turn)
    {
        if (!Config.Cached)
        {
            Debug.LogError("GAME IS NOT CACHED!");
            return;
        }
        
        if(turn < 1 || turn > Config.lastTurn)
        {
            Debug.LogError("TURN IS OUT OF RANGE!");
            return;
        }

        _cacheables.ForEach(c => c.LoadState(turn - 1));
        logHandler.SetCurrentLine(turnStartsLineNumbers[turn]);
        
        _turnNumber = turn;
        hudManager.EnableNextButton(true);
        _gameStatus = GameStatus.ONGOING;
        
        NextMove();
    }

    #region running

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
            var isVisibleValue = GetValue(isVisible, line) == "true";

            ChangeTurn(toTurnNumberValue, toTurnValue, isVisibleValue);
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
            var thiefIdValue = int.Parse(GetValue(thiefId, line));
            var nodeIdValue = GetValue(nodeId, line);
            
            agentsController.ThiefCaught(thiefIdValue);
            if (!_isCaching)
            {
                hudManager.ThiefCaughtAlert(thiefIdValue);
            }
            Debug.Log($"Thief with Id {thiefIdValue} arrested");
        }
        else if (line.Contains("\"type\":\"GAME_RESULT_CHANGED\""))
        {
            var gameResultValue = GetValue(gameResult, line);
            if (!Enum.TryParse(gameResultValue, out _finalResult))
            {
                Debug.LogError("GAME RESULT IS NOT VALID!");
            }
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
                ChangeTurn(1, AgentType.THIEF, false);
                hudManager.EnableNextButton(true);
                break;
            case GameStatus.FINISHED:
                hudManager.EnableNextButton(false);
                if (!_isCaching)
                {
                    hudManager.GameEndAlert(_finalResult);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(toStatus), toStatus, null);
        }

        onGameStatusChange.Invoke(toStatus);
    }

    private void ChangeTurn(int turnNumber, AgentType turnType, bool isVisibleValue)
    {
        _turnNumber = turnNumber;
        _turnAgentType = turnType;
        onTurnChange?.Invoke(turnNumber, turnType);
        
        agentsController.ChangeVisibleState(isVisibleValue);

        if (_isCaching)
        {
            _cacheables.ForEach(c => c.SaveState());
            turnStartsLineNumbers.Add(turnNumber, logHandler.GetCurrentLine() - 1);
        }
    }

    #endregion

    #region JsonClasses
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
    
    #endregion
}

public enum GameStatus
{
    PENDING,
    ONGOING,
    FINISHED
}
public enum GameResult
{
    FIRST_WINS,
    SECOND_WINS,
    TIE
}