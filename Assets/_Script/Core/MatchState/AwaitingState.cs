using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwaitingState : IMatchState
{
    private float _limitX, _limitZUp, _limitZDown;

    public bool HasBallInHand { get; private set; }
    private float _timeLimit;
    private int _lastDisplayedTime;
    private MatchManager _matchManager;
    private BallState _ballState;

    public AwaitingState(MatchManager matchManager)
    {
        _matchManager = matchManager;
        _ballState = matchManager.gameState.ballState;
    }

    public void Enter()
    {
        SetupTurnContext();

        _lastDisplayedTime = Mathf.CeilToInt(_timeLimit);

        _matchManager.OnTimerUpdated?.Invoke(_lastDisplayedTime);
    }

    public void Exit()
    {
        if(HasBallInHand)
        {
            HasBallInHand = false;
            _matchManager.OnBallInHandFinished?.Invoke(HasBallInHand);
        }
    }

    public void FixedUpdate(float fixedt)
    {
        
    }

    public void Update(float dt)
    {
        // Bo dem time
        if (_timeLimit > 0)
        {
            _timeLimit -= dt;

            if (_timeLimit < 0) _timeLimit = 0;

            int currentTimeInt = Mathf.CeilToInt(_timeLimit);

            if (currentTimeInt < _lastDisplayedTime)
            {
                _lastDisplayedTime = currentTimeInt;
                _matchManager.OnTimerUpdated?.Invoke(_lastDisplayedTime); 
            }

            if (_timeLimit <= 0)
            {
                HandleTimeOut();
            }
        }
    }

    private void HandleTimeOut()
    {
        _matchManager.gameState.shotResult.isTimeOut = true;
        _matchManager.ChangeState(_matchManager._ruleCheckingState, MatchStateEnum.RuleChecking);
    }

    private void SetupTurnContext()
    {
        var turnInfo = _matchManager.gameState.currentTurnInfo;
        _timeLimit = turnInfo.timeLimit;

        _limitX = _matchManager.gameState.physicData.Width / 2;
        _limitZDown = -_matchManager.gameState.physicData.Length / 2;
        if (turnInfo.isBreakShot)
        {
            HasBallInHand = true;

            // gioi han vung pha bi
            _limitZUp = _matchManager.gameState.physicData.HeadSpot.Z;
        }
        else if(turnInfo.hasBallInHand)
        {
            HasBallInHand = true;

            // gioi han vung dat bi trang
            _limitZUp = -_limitZDown;
        }

        if(HasBallInHand)
        {
            _matchManager.OnBallInHandStarted?.Invoke(HasBallInHand);
        }
    }

    public void UpdateCueBallPosition(PhysicVector3 newPos)
    {
        // Kiem tra vi tri dat bi trong vung thoa man
        if (newPos.X < -_limitX) newPos.X = -_limitX;
        else if(newPos.X > _limitX) newPos.X = _limitX;

        if (newPos.Z < _limitZDown) newPos.Z = _limitZDown;
        else if (newPos.Z > _limitZUp) newPos.Z = _limitZUp;

        _ballState.SetPosition(0, newPos);

        float rBall = _matchManager.gameState.physicData.BallRadius;
        float safeDistance = rBall * 2.01f; 
        bool isOverlapping = true;
        int safetyCounter = 0;

        while (isOverlapping && safetyCounter < 10)
        {
            isOverlapping = false;

            for (int i = 1; i < _ballState.TotalBalls; i++)
            {
                if (!_ballState.IsActive[i]) continue;

                float distance = PhysicVector3.Distance(_ballState.Positions[0], _ballState.Positions[i]);

                if (distance < safeDistance)
                {
                    isOverlapping = true;

                    PhysicVector3 pushDir = (distance < 0.001f)
                        ? new PhysicVector3(1, 0, 0)
                        : (_ballState.Positions[0] - _ballState.Positions[i]).Normalize();

                    PhysicVector3 snappedPos = _ballState.Positions[i] + pushDir * safeDistance;

                    if (snappedPos.X < -_limitX) snappedPos.X = -_limitX;
                    if (snappedPos.X > _limitX) snappedPos.X = _limitX;
                    if (snappedPos.Z < _limitZDown) snappedPos.Z = _limitZDown;
                    if (snappedPos.Z > _limitZUp) snappedPos.Z = _limitZUp;

                    _ballState.SetPosition(0, snappedPos);

                    break;
                }
            }
            safetyCounter++;
        }
    }
}
