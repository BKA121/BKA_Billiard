using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishState : IMatchState
{
    private MatchManager _matchManager;

    public FinishState(MatchManager matchManager)
    {
        _matchManager = matchManager;
    }
    public void Enter()
    {
        var listPlayer = _matchManager.gameState.listPlayer;

        _matchManager.OnNotifyFinishMatch?.Invoke(listPlayer);
    }

    public void Exit()
    {
        
    }

    public void FixedUpdate(float fixedt)
    {
        
    }

    public void Update(float dt)
    {
        
    }
}
