using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class InitializingState : IMatchState
{
    private MatchManager _matchManager;
    private PhysicData _physicData;
    private BallState _ballState;

    public InitializingState(MatchManager matchManager)
    {
        _matchManager = matchManager;
        _physicData = _matchManager.gameState.physicData;
        _ballState = _matchManager.gameState.ballState;
    }

    public void Enter()
    {
        Setup8BallPositionForNewMatch();

        InitializePlayer(_matchManager.matchConfig);
        InitializeTurnInfo(_matchManager.gameState.currentTurnInfo, _matchManager.matchConfig);

        _matchManager.ChangeState(_matchManager._notifyingState, MatchStateEnum.Notifying);
    }

    public void FixedUpdate(float fixedt)
    {
        
    }

    public void Update(float dt)
    {

    }

    public void Exit()
    {

    }

    private void InitializePlayer(MatchConfig matchConfig)
    {
        _matchManager.gameState.listPlayer = new List<PlayerInfo>();

        foreach (var reg in matchConfig.listPlayer)
        {
            _matchManager.gameState.listPlayer.Add(new PlayerInfo
            {
                Id = reg.Id,
                name = reg.name,
                type = reg.type,
                score = 0,
                foulCount = 0,
                remainingBalls = 7,
                targetGroup = BallGroupType.None
            });
        }
        _matchManager.gameState.currentPlayerIndex = 0;
    }
    private void InitializeTurnInfo(TurnInfo currentTurnInfo, MatchConfig matchConfig)
    {
        currentTurnInfo.activePlayerId = _matchManager.gameState.GetCurrentPlayer().Id;
        currentTurnInfo.namePlayer = _matchManager.gameState.GetCurrentPlayer().name;
        currentTurnInfo.isBreakShot = true;
        currentTurnInfo.hasBallInHand = false;
        currentTurnInfo.timeLimit = matchConfig.timeLimit;

        currentTurnInfo.isGameOver = false;
        currentTurnInfo.lastFoulType = FoulType.None;
        currentTurnInfo.notifyMessage = "";
    }

    public void Setup8BallPositionForNewMatch()
    {
        float R = _physicData.BallRadius;
        float diameter = R * 2.01f;
        float rowDistance = R * 1.734f;

        _ballState.SetPosition(0, _physicData.HeadSpot);
        _ballState.IsActive[0] = true;

        System.Random _rng = new System.Random();
        List<int> solids = new List<int>();  // 1-7
        List<int> stripes = new List<int>(); // 9-15
        for (int i = 1; i <= 7; i++) solids.Add(i);
        for (int i = 9; i <= 15; i++) stripes.Add(i);

        int corner1Idx = _rng.Next(solids.Count);
        int cornerBall1 = solids[corner1Idx];
        solids.RemoveAt(corner1Idx);

        int corner2Idx = _rng.Next(stripes.Count);
        int cornerBall2 = stripes[corner2Idx];
        stripes.RemoveAt(corner2Idx);

        // Gom danh sach soc tron va tron
        List<int> remainingBalls = new List<int>();
        remainingBalls.AddRange(solids);
        remainingBalls.AddRange(stripes);
        ShuffleList(remainingBalls, _rng);

        int[,] rackScheme = new int[5, 5];
        int remainingIdx = 0;

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col <= row; col++)
            {
                if (row == 2 && col == 1)
                {
                    rackScheme[row, col] = 8;
                }
                else if (row == 4 && col == 0)
                {
                    rackScheme[row, col] = cornerBall1;
                }
                else if (row == 4 && col == 4)
                {
                    rackScheme[row, col] = cornerBall2;
                }
                else
                {
                    rackScheme[row, col] = remainingBalls[remainingIdx];
                    remainingIdx++;
                }
            }
        }

        PhysicVector3 apex = _physicData.FootSpot;

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col <= row; col++)
            {
                int ballID = rackScheme[row, col];

                float posZ = apex.Z + (row * rowDistance);
                float posX = apex.X + (col - (row / 2f)) * diameter;

                _ballState.SetPosition(ballID, new PhysicVector3(posX, 0, posZ));
                _ballState.IsActive[ballID] = true;
            }
        }
    }
    private void ShuffleList(List<int> list, System.Random _rng)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
