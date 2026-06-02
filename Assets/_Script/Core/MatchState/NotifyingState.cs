using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyingState : IMatchState
{
    private MatchManager _matchManager;
    private float _displayTimer;
    private const float DISPLAY_DURATION = 2f;

    public NotifyingState(MatchManager matchManager)
    {
        _matchManager = matchManager;
    }

    public void Enter()
    {
        _displayTimer = DISPLAY_DURATION;
        _matchManager.OnShowTurn?.Invoke(_matchManager.gameState.currentTurnInfo);
        _matchManager.OnNotifyInMatch?.Invoke(_matchManager.gameState.currentTurnInfo);
    }

    public void Exit()
    {
        
    }

    public void FixedUpdate(float fixedt)
    {
        
    }

    public void Update(float dt)
    {
        _displayTimer -= dt;
        if (_displayTimer <= 0)
        {
            if (_matchManager.gameState.currentTurnInfo.isGameOver)
            {
                // Chuyen man hinh thong bao ket qua van dau
                var listPlayer = _matchManager.gameState.listPlayer;

                _matchManager.OnNotifyFinishMatch?.Invoke(listPlayer);
            }
            else
            {
                _matchManager.ChangeState(_matchManager._awaitingState, MatchStateEnum.Awaiting);
            }
        }
    }
}
