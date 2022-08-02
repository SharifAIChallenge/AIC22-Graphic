using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text turnText;

    [Header("Playback Hud")] [SerializeField]
    private TMP_InputField toTurnInputField;

    [SerializeField] private Button playPauseButton;
    [SerializeField] private Button nextStepButton;
    [SerializeField] private Button previousTurnButton;
    [SerializeField] private Button nextTurnButton;
    [SerializeField] private Button goToTurnButton;

    [SerializeField] private CanvasGroup alertPanel;
    [SerializeField] private TMP_Text alertPanelText;

    [SerializeField] private Slider gameSpeedSlider;
    [SerializeField] private TMP_Text gameSpeedText;
    
    private PlaybackManager _playbackManager;

    private bool _isPlaying;

    public void Setup(PlaybackManager playbackManager)
    {
        _playbackManager = playbackManager;
        _playbackManager.onTurnChange += UpdateTurn;
        _isPlaying = false;

        playPauseButton.onClick.AddListener(TogglePlayBack);
        nextStepButton.onClick.AddListener(Next);
        previousTurnButton.onClick.AddListener(PreviousTurn);
        nextTurnButton.onClick.AddListener(NextTurn);
        goToTurnButton.onClick.AddListener(GoToTurn);
        
        gameSpeedSlider.onValueChanged.AddListener(UpdateGameSpeed);
    }

    private void GoToTurn()
    {
        if (_isPlaying) return;

        if (toTurnInputField.text == "" || !int.TryParse(toTurnInputField.text, out var toTurnNumber))
        {
            return;
        }

        toTurnInputField.text = "";
        _playbackManager.LoadTurn(toTurnNumber);
    }

    private void PreviousTurn()
    {
        if (_isPlaying) return;

        _playbackManager.PreviousTurn();
    }

    private void NextTurn()
    {
        if (_isPlaying) return;

        _playbackManager.NextTurn();
    }

    private void Next()
    {
        if (_isPlaying) return;

        _playbackManager.NextMove();
    }

    public void TogglePlayBack()
    {
        if (_isPlaying)
        {
            _isPlaying = !_isPlaying;
            _playbackManager.Pause();
            playPauseButton.GetComponentInChildren<TMP_Text>().text = "Play";
            nextStepButton.interactable = _playbackManager.GameStatus == GameStatus.ONGOING;
            previousTurnButton.interactable = _playbackManager.TurnNumber > 1;
            nextTurnButton.interactable = _playbackManager.TurnNumber < Config.lastTurn;
            goToTurnButton.interactable = true;
        }
        else
        {
            if (_playbackManager.GameStatus == GameStatus.FINISHED)
                return;
            _isPlaying = !_isPlaying;
            _playbackManager.Play();
            playPauseButton.GetComponentInChildren<TMP_Text>().text = "Pause";
            nextStepButton.interactable = false;
            previousTurnButton.interactable = false;
            nextTurnButton.interactable = false;
            goToTurnButton.interactable = false;
        }
    }

    private void UpdateTurn(int turnNumber, AgentType turnType)
    {
        turnText.text = $"Turn: {turnNumber} ({turnType.ToString().ToLower()} turn)";
        previousTurnButton.interactable = turnNumber > 1 && !_isPlaying;
        nextTurnButton.interactable = turnNumber < Config.lastTurn && !_isPlaying;
    }

    private void OnDisable()
    {
        _playbackManager.onTurnChange -= UpdateTurn;
    }

    public void ThiefCaughtAlert(int thiefId)
    {
        alertPanelText.text = $"Thief with ID {thiefId}\n<color=#8f0000>ARRESTED</color>";
        alertPanel.transform.localScale = Vector3.zero;
        alertPanel.alpha = 1;
        alertPanel.transform.DOScale(1, 0.2f);
        StartCoroutine(CloseAlert());
    }

    private IEnumerator CloseAlert()
    {
        yield return new WaitForSeconds(2);
        alertPanel.DOFade(0, 0.3f);
    }

    public void EnableNextButton(bool enable)
    {
        nextStepButton.interactable = enable;
    }

    public void GameEndAlert(GameResult finalResult)
    {
        if (finalResult == GameResult.TIE)
        {
            alertPanelText.text = "Game Finished:\nIt Was a DRAW!";
        }
        else
        {
            var winner = finalResult == GameResult.FIRST_WINS ? "FIRST" : "SECOND";
            var color = finalResult == GameResult.FIRST_WINS ? "blue" : "red";
            var colorTag = finalResult == GameResult.FIRST_WINS ? "<color=\"blue\">" : "<color=#8f0000>";
            alertPanelText.text = $"Game Finished:\n{colorTag}{winner} ({color})</color>\nTeam Won The Game!";
        }

        alertPanel.transform.localScale = Vector3.zero;
        alertPanel.alpha = 1;
        alertPanel.transform.DOScale(1, 0.2f);
        StartCoroutine(CloseAlert());
    }
    
    private void UpdateGameSpeed(float value)
    {
        Config.GameSpeed = value;
        gameSpeedText.text = $"{value.ToString("F1", CultureInfo.InvariantCulture)}x";
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && nextTurnButton.interactable)
        {
            NextTurn();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && previousTurnButton.interactable)
        {
            PreviousTurn();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePlayBack();
        }
        else if(Input.GetKeyDown(KeyCode.N) && nextStepButton.interactable)
        {
            Next();
        }
    }
}