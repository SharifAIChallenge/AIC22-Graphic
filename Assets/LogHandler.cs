using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class LogHandler : MonoBehaviour
{

    void Start()
    {
        
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

            var lines = File.ReadLines("log.txt");
            foreach (var line in lines)
            {
                if(line.Contains("\"type\":\"READINESS_DECLARATION\"")) {
                    String agentIdValue = getValue(agentId, line);
                    String balanceValue = getValue(balance, line);
                    String teamValue = getValue(team, line);
                    String typeValue = getValue(type, line);
                    String nodeIdValue = getValue(nodeId, line);
                    String tokenValue = getValue(token, line);
                    //call function
                } else if(line.Contains("\"type\":\"STATUS_CHANGE\"")) {
                    String toStatusValue = getValue(toStatus, line);
                    String fromStatusValue = getValue(fromStatus, line);
                    //call function
                }else if(line.Contains("\"type\":\"AGENT_MOVEMENT\"")) {
                    String agentIdValue = getValue(agentId, line);
                    String balanceValue = getValue(balance, line);
                    String priceValue = getValue(price, line);
                    String toNodeIdValue = getValue(toNodeId, line);
                    String fromNodeIdValue = getValue(fromNodeId, line);
                    //call function
                } else if(line.Contains("\"type\":\"TURN_CHANGE\"")) {
                    String toTurnNumberValue = getValue(toTurnNumber, line);
                    String toTurnValue = getValue(toTurn, line);
                    //call function
                } else if(line.Contains("\"type\":\"AGENT_BALANCE_CHARGED\"")) {
                    String agentIdValue = getValue(agentId, line);
                    String balanceValue = getValue(balance, line);
                    String wageValue = getValue(wage, line);
                    //call function
                } else if(line.Contains("\"type\":\"AGENT_SEND_MESSAGE\"")) {
                    String agentIdValue = getValue(agentId, line);
                    String balanceValue = getValue(balance, line);
                    String priceValue = getValue(price, line);
                    String teamValue = getValue(team, line);
                    String textValue = getValue(text, line);
                    String typeValue = getValue(type, line);
                    //call function
                } else if(line.Contains("\"type\":\"POLICES_CAUGHT_THIEVES\"")) {
                    String thiefIdValue = getValue(thiefId, line);
                    String nodeIdValue = getValue(nodeId, line);
                    //call function
                } else if(line.Contains("\"type\":\"GAME_RESULT_CHANGED\"")) {
                    String gameResultValue = getValue(gameResult, line);
                    //call function
                }
                
            }
    }

     static String getValue(Regex regex, String str) {
            Match match = regex.Match(str);
            return match.Groups[1].Value;
        }
}

