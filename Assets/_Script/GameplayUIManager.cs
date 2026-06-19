using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    public PlayerInputController playerInputController;

    public GameObject exitConfirmPanel;
    public GameObject matchResultPanel;
    public GameObject controlsPanel;

    // Hien thi ket qua sau match
    public TMP_Text player1WinnerText;
    public TMP_Text player1NameText;
    public TMP_Text player1ScoreText;

    public TMP_Text player2WinnerText;
    public TMP_Text player2NameText;
    public TMP_Text player2ScoreText;

    // Hien thi score bar
    public TMP_Text player1Name;
    public TMP_Text player1Score;
    public TMP_Text player2Name;
    public TMP_Text player2Score;
    public TMP_Text races;

    // Hien thi luot danh
    public Image turn1;
    public Image turn2;
    public Image turnSolid;
    public Image turnStripe;

    // Hien thi trang thai ball
    public Image[] balls;
    public Color pocketedColor = new Color(50, 50, 50, 255);
    public Color originalColor = new Color(255, 255, 255, 255);

    private bool _isPaused = false;

    public void Awake()
    {
        MatchManager.Instance.OnNotifyFinishMatch += ShowResultMatch;
        MatchManager.Instance.OnShowScoreBar += ShowScoreBar;
        MatchManager.Instance.OnShowTurn += ShowTurn;
        MatchManager.Instance.OnChangeColorBallPocketed += ChangeColorBallPocketed;
    }

    void Update()
    {
        if (playerInputController.IsExitMatch())
        {
            ToggleExitMenu();
        }

        if (playerInputController.IsShowControls())
        {
            controlsPanel.SetActive(true);
        }
        else if (playerInputController.IsHideControls())
        {
            controlsPanel.SetActive(false);
        }
    }

    private void ToggleExitMenu()
    {
        _isPaused = !_isPaused; 

        exitConfirmPanel.SetActive(_isPaused);

        Time.timeScale = _isPaused ? 0f : 1f;
    }

    public void OnYesClickedInExitPanel()
    {
        Time.timeScale = 1f;
        exitConfirmPanel.SetActive(false);
        MatchManager.Instance.ExecutePlayerQuit();
    }

    public void OnNoClickedInExitPanel()
    {
        ToggleExitMenu();
    }

    public void ShowScoreBar(List<PlayerInfo> listPlayer)
    {
        player1Name.text = listPlayer[0].name;
        player1Score.text = listPlayer[0].score.ToString();

        player2Name.text = listPlayer[1].name;
        player2Score.text = listPlayer[1].score.ToString();

        races.text = "Rack " + MatchManager.Instance.gameState.races.ToString();
    }

    public void ShowResultMatch(List<PlayerInfo> listPlayer)
    {
        Time.timeScale = 0f;

        player1NameText.text = listPlayer[0].name;
        if (listPlayer[0].isWinner) player1WinnerText.text = "Winner!";
        else player1WinnerText.text = "";
        player1ScoreText.text = listPlayer[0].score.ToString();

        player2NameText.text = listPlayer[1].name;
        if (listPlayer[1].isWinner) player2WinnerText.text = "Winner!";
        else player2WinnerText.text = "";
        player2ScoreText.text = listPlayer[1].score.ToString();

        matchResultPanel.SetActive(true);
    }

    private void ShowTurn(TurnInfo info)
    {
        if (info.currentPlayer == 0)
        {
            turn1.enabled = true; turn2.enabled = false;
        }
        else
        {
            turn1.enabled = false; turn2.enabled = true;
        }

        BallGroupType currentTarget = MatchManager.Instance.gameState.GetCurrentPlayer().targetGroup;
        if (currentTarget == BallGroupType.Solid)
        {
            turnSolid.enabled = true; turnStripe.enabled = false;
        }
        else if(currentTarget == BallGroupType.Stripe)
        {
            turnSolid.enabled = false; turnStripe.enabled = true;
        }
        else
        {
            turnSolid.enabled = false; turnStripe.enabled = false;
        }
    }

    public void ChangeColorBallPocketed(int ballID)
    {
        if(ballID > 0)
            balls[ballID - 1].color = pocketedColor;
    }

    public void OnReplayClicked()
    {
        matchResultPanel.SetActive(false);
        Time.timeScale = 1f;
        MatchManager.Instance._initializingState.isReplay = true;
        ResetColorBall();
        MatchManager.Instance.ChangeState(MatchManager.Instance._initializingState, MatchStateEnum.Initializing);
    }

    public void ResetColorBall()
    {
        for(int i=0; i<balls.Length; i++)
        {
            balls[i].color = originalColor;
        }
    }

    public void OnExitClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
