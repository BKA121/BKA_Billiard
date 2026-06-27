using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoulType
{
    None,               
    NoBallCollision,    // Bi trang khong va cham voi bi khac    
    Ball0Pocketed,      // Bi trang roi
    WrongBallHit,       // Danh sai nhom bi
    NoCushionContact,   // Khong co bi cham bang
    InvalidBreak,       // Pha khong hop le
    TimeOut,            // Het gio
    EightBallFoul,      // Loi roi 8
    Quit                // Bo cuoc
}

public class RuleCheckingState : IMatchState
{
    private MatchManager _matchManager;
    private BallState _ballState;
    private FoulType _hasFoul;
    private bool _hasScored;  // kiem tra cu danh co an bi muc tieu de duoc danh tiep
    private bool _win;

    public RuleCheckingState(MatchManager matchManager)
    {
        _matchManager = matchManager;
        _ballState = matchManager.gameState.ballState;
    }

    public void Enter()
    {
        ProcessShotLogic();

        _matchManager.ChangeState(_matchManager._notifyingState, MatchStateEnum.Notifying);
    }

    public void Exit()
    {
        _matchManager.gameState.shotResult.Reset();
    }

    public void FixedUpdate(float fixedt)
    {
        
    }

    public void Update(float dt)
    {
        
    }

    public void ProcessShotLogic()
    {
        PlayerInfo currentPlayer = _matchManager.gameState.GetCurrentPlayer();
        PlayerInfo otherPlayer = _matchManager.gameState.GetOtherPlayer();

        var turnInfo = _matchManager.gameState.currentTurnInfo;

        if(turnInfo.lastFoulType == FoulType.Quit)
        {
            turnInfo.notifyMessage = currentPlayer.name + " left the match.";
            _matchManager.gameState.currentPlayerIndex = (_matchManager.gameState.currentPlayerIndex == 0) ? 1 : 0;
            PlayerInfo nextPlayer = _matchManager.gameState.GetCurrentPlayer();

            turnInfo.currentPlayer = _matchManager.gameState.currentPlayerIndex;
            turnInfo.isGameOver = true;
            nextPlayer.score += 1;
            nextPlayer.isWinner = true;
         
            return;
        }

        var result = _matchManager.gameState.shotResult;

        // Kiem tra loi
        CheckFoul(result, turnInfo, currentPlayer, otherPlayer);

        // Quyet dinh luot danh moi
        turnInfo.hasBallInHand = false;
        turnInfo.lastFoulType = _hasFoul;
        if(_hasFoul != FoulType.None) turnInfo.notifyMessage = GetFoulMessage(_hasFoul);

        if (_hasFoul == FoulType.EightBallFoul)
        {
            _matchManager.gameState.currentPlayerIndex = (_matchManager.gameState.currentPlayerIndex == 0) ? 1 : 0;
            PlayerInfo nextPlayer = _matchManager.gameState.GetCurrentPlayer();

            turnInfo.currentPlayer = _matchManager.gameState.currentPlayerIndex;
            turnInfo.isGameOver = true;
            nextPlayer.isWinner = true;
            nextPlayer.score += 1;
            return;
        }
        else if (_win)
        {
            turnInfo.isGameOver = true;
            currentPlayer.isWinner = true;
            currentPlayer.score += 1;
            return;
        }
        else if (!_hasScored) // doi luot
        {
            _matchManager.gameState.currentPlayerIndex = (_matchManager.gameState.currentPlayerIndex == 0) ? 1 : 0;
            PlayerInfo nextPlayer = _matchManager.gameState.GetCurrentPlayer();

            turnInfo.currentPlayer = _matchManager.gameState.currentPlayerIndex;

            if (_hasFoul != FoulType.None && _hasFoul != FoulType.EightBallFoul)
            {
                turnInfo.hasBallInHand = true;
            }
        }

        float rBall = _matchManager.gameState.physicData.BallRadius;

        // Dat lai bi 8
        if (result.isBall8Pocketed && turnInfo.isBreakShot)
        {
            _ballState.SetPosition(8, _matchManager.gameState.physicData.FootSpot);
            _ballState.IsActive[8] = true;
            _ballState.UndoDropBall(8);

            float safeDistance = rBall * 2.01f; 
            float stepDelta = 0.01f;
            bool isOverlapping = true;

            int safetyCounter = 0;
            while (isOverlapping && safetyCounter < 100)
            {
                isOverlapping = false;

                for (int i = 0; i < 16; i++)
                {
                    if (i == 8 || !_ballState.IsActive[i]) continue;

                    float distance = PhysicVector3.Distance(_ballState.Positions[8], _ballState.Positions[i]);

                    if (distance < safeDistance)
                    {
                        isOverlapping = true;
                        break;
                    }
                }

                if (isOverlapping)
                {
                    PhysicVector3 currentPos = _ballState.Positions[8];

                    _ballState.SetPosition(8, new PhysicVector3(currentPos.X, currentPos.Y, currentPos.Z + stepDelta));
                }

                safetyCounter++;
            }
            _matchManager.OnResetBall8?.Invoke();
            _matchManager.OnChangeColorBallPocketed?.Invoke(8);
        }

        turnInfo.isBreakShot = false;

        // Dat lai bi trang
        if (result.isBall0Pocketed && !_win)
        {
            _ballState.SetPosition(0, _matchManager.gameState.physicData.HeadSpot);
            _ballState.IsActive[0] = true;
            _ballState.UndoDropBall(0);

            float safeDistance = rBall * 2.01f;
            float stepDelta = 0.01f;
            bool isOverlapping = true;

            int safetyCounter = 0;
            while (isOverlapping && safetyCounter < 100)
            {
                isOverlapping = false;

                for (int i = 1; i < 16; i++)
                {
                    if (!_ballState.IsActive[i]) continue;

                    float distance = PhysicVector3.Distance(_ballState.Positions[0], _ballState.Positions[i]);

                    if (distance < safeDistance)
                    {
                        isOverlapping = true;
                        break;
                    }
                }

                if (isOverlapping)
                {
                    PhysicVector3 currentPos = _ballState.Positions[0];

                    _ballState.SetPosition(0, new PhysicVector3(currentPos.X + stepDelta, currentPos.Y, currentPos.Z));
                }

                safetyCounter++;
            }
        }
    }

    private void CheckFoul(ShotResult result, TurnInfo turnInfo, PlayerInfo currentPlayer, PlayerInfo otherPlayer)
    {
        _hasFoul = FoulType.None;
        _hasScored = false;
        _win = false;

        // kiem tra het gio
        if (result.isTimeOut)
        {
            _hasFoul = FoulType.TimeOut;
            return;
        }

        // kiem tra bi trang va cham bi khac
        if (result.firstBallHitID == -1)
        {
            _hasFoul = FoulType.NoBallCollision;
            return;
        }

        // kiem tra cu pha hop le
        if(turnInfo.isBreakShot)
        {
            int countBallHitCushionAfterBreak = 0;
            for(int i=0; i<result.ballHitCushionAfterShot.Count; i++)
            {
                // chi tinh bi muc tieu cham bang
                if (result.ballHitCushionAfterShot[i] != 0) countBallHitCushionAfterBreak++;
            }

            if(countBallHitCushionAfterBreak < 4 && result.pocketedBallIDs.Count == 0)
            {
                _hasFoul = FoulType.InvalidBreak;
                return;
            }
        }

        // kiem tra va cham bang sau va cham bi
        if (result.ballHitCushionAfterShot.Count == 0 && result.pocketedBallIDs.Count == 0)
        {
            _hasFoul = FoulType.NoCushionContact;
            return;
        }

        if (result.pocketedBallIDs.Count == 0) // truong hop khong roi bi
        {
            if ((currentPlayer.targetGroup == BallGroupType.Solid && result.firstBallHitID > 8) ||
                (currentPlayer.targetGroup == BallGroupType.Stripe && result.firstBallHitID < 8) ||
                (currentPlayer.targetGroup == BallGroupType.None && result.firstBallHitID == 8) ||
                (!currentPlayer.canPlayEightBall && result.firstBallHitID == 8))
            {
                _hasFoul = FoulType.WrongBallHit;
            }
        }
        else // truong hop co roi bi
        {
            if(currentPlayer.targetGroup != BallGroupType.None)
            {
                currentPlayer.remainingBalls = _matchManager.gameState.CountBallsInGroup(currentPlayer.targetGroup);
                otherPlayer.remainingBalls = _matchManager.gameState.CountBallsInGroup(otherPlayer.targetGroup);
            }

            if(result.isBall0Pocketed && !result.isBall8Pocketed)
            {
                _hasFoul = FoulType.Ball0Pocketed;
                return;
            }
            else
            {
                if (result.isBall8Pocketed)
                {
                    if(turnInfo.isBreakShot)
                    {
                        if (result.isBall0Pocketed) _hasFoul = FoulType.Ball0Pocketed;
                        else _hasScored = true;
                        return;
                    }
                    else
                    {
                        if (!currentPlayer.canPlayEightBall)
                            _hasFoul = FoulType.EightBallFoul;
                        else if (currentPlayer.canPlayEightBall && result.firstBallHitID != 8)
                            _hasFoul = FoulType.EightBallFoul;
                        else if (currentPlayer.canPlayEightBall && result.firstBallHitID == 8 && result.isBall0Pocketed)
                            _hasFoul = FoulType.EightBallFoul;
                        else _win = true;
                        return;
                    }
                }
                else
                {
                    if (result.firstBallHitID == 8 && !currentPlayer.canPlayEightBall)
                    {
                        _hasFoul = FoulType.WrongBallHit;
                        return;
                    }

                    if(result.firstBallHitID != 8)
                    {
                        if (currentPlayer.targetGroup == BallGroupType.None)
                        {
                            if (!turnInfo.isBreakShot)
                            {
                                if (result.pocketedBallIDs[0] < 8)
                                {
                                    currentPlayer.targetGroup = BallGroupType.Solid;
                                    turnInfo.notifyMessage = currentPlayer.name + " is assigned solids.";
                                }
                                else
                                {
                                    currentPlayer.targetGroup = BallGroupType.Stripe;
                                    turnInfo.notifyMessage = currentPlayer.name + " is assigned stripes.";
                                }
                                turnInfo.isDeviceBallGroup = true;
                                otherPlayer.targetGroup = (currentPlayer.targetGroup == BallGroupType.Solid)
                                                            ? BallGroupType.Stripe
                                                            : BallGroupType.Solid;

                                currentPlayer.remainingBalls = _matchManager.gameState.CountBallsInGroup(currentPlayer.targetGroup);
                                otherPlayer.remainingBalls = _matchManager.gameState.CountBallsInGroup(otherPlayer.targetGroup);
                            }
                            _hasScored = true;
                            return;
                        }
                        else
                        {
                            if (result.firstBallHitID < 8 && currentPlayer.targetGroup != BallGroupType.Solid)
                                _hasFoul = FoulType.WrongBallHit;

                            else if (result.firstBallHitID > 8 && currentPlayer.targetGroup != BallGroupType.Stripe)
                                _hasFoul = FoulType.WrongBallHit;
                            else
                            {
                                for (int j = 0; j < result.pocketedBallIDs.Count; j++)
                                {
                                    if (result.pocketedBallIDs[j] < 8 && currentPlayer.targetGroup == BallGroupType.Solid)
                                    {
                                        _hasScored = true;
                                        break;
                                    }
                                    else if (result.pocketedBallIDs[j] > 8 && currentPlayer.targetGroup == BallGroupType.Stripe)
                                    {
                                        _hasScored = true;
                                        break;
                                    }
                                }
                            }
                            return;
                        }

                    }
                }

            }
        }
    }

    private string GetFoulMessage(FoulType foul)
    {
        switch (foul)
        {
            case FoulType.TimeOut: return "Time out!";
            case FoulType.EightBallFoul: return "8 ball foul!";
            case FoulType.Ball0Pocketed: return "Cue ball scratch!";
            case FoulType.NoBallCollision: return "No ball hit!";
            case FoulType.NoCushionContact: return "No ball hit a rail after contact!";
            case FoulType.WrongBallHit: return "Wrong ball first!";
            case FoulType.InvalidBreak: return "Invalid break shot!";
            default: return "";
        }
    }
}
