using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchPresenter : MonoBehaviour
{
    public MatchManager matchManager;
    [SerializeField] private TextMeshProUGUI timerText;

    private int _lastPlayerId = -1;

    public void Initialize(MatchManager matchManager)
    {
        this.matchManager = matchManager;

        matchManager.OnNotifyUI += ShowNotifyMatch;
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
        if(_lastPlayerId == -1)
        {
            _lastPlayerId = info.activePlayerId;
        }

        if (info.isBreakShot)
        {
            Debug.Log(info.namePlayer.ToUpper() + " PHÁ BI!");
            return;
        }

        if(info.lastFoulType != FoulType.None)
        {
            Debug.Log(info.notifyMessage);
        }
        
        if(_lastPlayerId != info.activePlayerId)
        {
            Debug.Log("LƯỢT CỦA " + info.namePlayer.ToUpper() + "!");
            _lastPlayerId = info.activePlayerId;
        }

        if (info.isGameOver)
        {
            Debug.Log(info.namePlayer.ToUpper() + " THẮNG!");
        }
    }
}
