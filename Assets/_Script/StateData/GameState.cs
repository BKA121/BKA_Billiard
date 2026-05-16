using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    // Du lieu cu danh hien tai cua nguoi choi
    public float currentShotForce;
    public PhysicVector3 currentShotDirection;

    // Du lieu vat ly
    public PhysicData physicData;
    public BallState ballState;

    // Thong tin player, luot danh
    public List<PlayerInfo> listPlayer;
    public TurnInfo currentTurnInfo;
    public int currentPlayerIndex;

    // Cac su kien sau cu danh
    public ShotResult shotResult;

    public FoulType hasFoulInCurrentTurn = FoulType.None;

    public GameState(PhysicData physicData)
    {
        this.physicData = physicData;
        ballState = new BallState(16);
        currentTurnInfo = new TurnInfo();
        shotResult = new ShotResult();
    }

    public PlayerInfo GetCurrentPlayer() => listPlayer[currentPlayerIndex];
    public PlayerInfo GetOtherPlayer()
    {
        int otherIndex = (currentPlayerIndex == 0) ? 1 : 0;
        return listPlayer[otherIndex];
    }

    public int CountBallsInGroup(BallGroupType group)
    {
        int count = 0;
        if (group == BallGroupType.Solid)
        {
            for (int i = 1; i <= 7; i++)
            {
                if (ballState.IsActive[i]) count++;
            }
        }
        else if (group == BallGroupType.Stripe)
        {
            for (int i = 9; i <= 15; i++)
            {
                if (ballState.IsActive[i]) count++;
            }
        }
        return count;
    }
}
