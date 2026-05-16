using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatingState : IMatchState
{
    private MatchManager _matchManager;

    public SimulatingState(MatchManager matchManager)
    {
        _matchManager = matchManager;
    }

    public void Enter()
    { 
        _matchManager.physicSystem.InitialShoot(_matchManager.gameState.currentShotDirection, _matchManager.gameState.currentShotForce);
    }

    public void Exit()
    {
        
    }

    public void FixedUpdate(float fixedt)
    {
        _matchManager.physicSystem.UpdatePhysicForFrame(fixedt);

        if (_matchManager.physicSystem.CheckBallsStatic())
        {
            _matchManager.ChangeState(_matchManager._ruleCheckingState, MatchStateEnum.RuleChecking);
        }
    }

    public void Update(float dt)
    {
        
    }

}
