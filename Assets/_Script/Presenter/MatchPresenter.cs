using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchPresenter : MonoBehaviour
{
    public MatchManager matchManager;
    public TMP_Text textFoulDescription;

    [SerializeField] private TextMeshProUGUI timerText;
    private Queue<string> _msgQueue = new Queue<string>();


    private int _lastPlayerId = -1;

    public void Initialize(MatchManager matchManager)
    {
        this.matchManager = matchManager;

        matchManager.OnNotifyInMatch += ShowNotifyMatch;
        matchManager.OnTimerUpdated += UpdateTimerText;
    }

    public void UpdateTimerText(int seconds)
    {
        if (timerText != null)
        {
            timerText.text = seconds.ToString(); 
        }
    }

    private void ShowNotifyMatch(TurnInfo info)
    {
        if (_lastPlayerId == -1)
        {
            _lastPlayerId = info.currentPlayer;
        }

        if (info.lastFoulType != FoulType.None)
        {
            _msgQueue.Enqueue(info.notifyMessage);
        }

        if (info.isGameOver)
        {
            StartCoroutine(ProcessNotificationQueue());
            return;
        }

        if (info.isBreakShot)
        {
            _msgQueue.Enqueue(matchManager.gameState.GetCurrentPlayer().name + " is breaking!");
            StartCoroutine(ProcessNotificationQueue());
            return;
        }

        if (info.isDeviceBallGroup)
        {
            _msgQueue.Enqueue(info.notifyMessage);
            info.isDeviceBallGroup = false;
            StartCoroutine(ProcessNotificationQueue());
            return;
        }
        
        if(_lastPlayerId != info.currentPlayer)
        {
            _msgQueue.Enqueue(matchManager.gameState.GetCurrentPlayer().name + "'s turn!");
            _lastPlayerId = info.currentPlayer;
        }
        StartCoroutine(ProcessNotificationQueue());
    }

    private IEnumerator ProcessNotificationQueue()
    {
        textFoulDescription.gameObject.SetActive(true);

        while(_msgQueue.Count > 0)
        {
            textFoulDescription.text = _msgQueue.Dequeue();

            yield return new WaitForSeconds(4f);
        }

        textFoulDescription.gameObject.SetActive(false);
    }
}
