using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text turnText;

    private PlaybackManager _playbackManager;
    
    public void Setup(PlaybackManager playbackManager)
    {
        _playbackManager = playbackManager;
        _playbackManager.onTurnChange += UpdateTurn;
    }

    private void UpdateTurn(int turnNumber, AgentType turnType)
    {
        turnText.text = $"Turn: {turnNumber} ({turnType.ToString().ToLower()} turn)";
    }

    private void OnDisable()
    {
        _playbackManager.onTurnChange -= UpdateTurn;
    }
}
