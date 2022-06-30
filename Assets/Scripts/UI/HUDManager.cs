using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text turnText;

    [Header("Playback Hud")]
    [SerializeField] private TMP_InputField toTurnInputField;
    [SerializeField] private Button playPauseButton;
    [SerializeField] private Button nextTurnButton;
    [SerializeField] private Button previousTurnButton;
    [SerializeField] private Button goToTurnButton;
    

    private PlaybackManager _playbackManager;
    
    private bool _isPlaying;
    
    public void Setup(PlaybackManager playbackManager)
    {
        _playbackManager = playbackManager;
        _playbackManager.onTurnChange += UpdateTurn;
        _isPlaying = false;
        
        playPauseButton.onClick.AddListener(TogglePlayBack);
        nextTurnButton.onClick.AddListener(Next);
        previousTurnButton.onClick.AddListener(PreviousTurn);
        goToTurnButton.onClick.AddListener(GoToTurn);
    }

    private void GoToTurn()
    {
        if(_isPlaying) return;

        if(toTurnInputField.text == "" || !int.TryParse(toTurnInputField.text, out var toTurnNumber))
        {
            return;
        }
        _playbackManager.LoadTurn(toTurnNumber);
    }

    private void PreviousTurn()
    {
        if(_isPlaying) return;
        
        _playbackManager.PreviousTurn();
    }

    private void Next()
    {
        if(_isPlaying) return;
        
        _playbackManager.NextMove();
    }

    private void TogglePlayBack()
    {
        if (_isPlaying)
        {
            _isPlaying = !_isPlaying;
            _playbackManager.Pause();
            playPauseButton.GetComponentInChildren<TMP_Text>().text = "Play";
            nextTurnButton.interactable = true;
            previousTurnButton.interactable = _playbackManager.TurnNumber > 1;
            goToTurnButton.interactable = true;
        }
        else
        {
            _isPlaying = !_isPlaying;
            _playbackManager.Play();
            playPauseButton.GetComponentInChildren<TMP_Text>().text = "Pause";
            nextTurnButton.interactable = false;
            previousTurnButton.interactable = false;
            goToTurnButton.interactable = false;
        }
    }

    private void UpdateTurn(int turnNumber, AgentType turnType)
    {
        turnText.text = $"Turn: {turnNumber} ({turnType.ToString().ToLower()} turn)";
        previousTurnButton.interactable = turnNumber > 1 && !_isPlaying;
    }

    private void OnDisable()
    {
        _playbackManager.onTurnChange -= UpdateTurn;
    }
}
