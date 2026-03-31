using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public PhysicData physicData;
    public BallState ballState;

    // Sau con cac state nhu: turn, score...

    public GameState(PhysicData physicData)
    {
        this.physicData = physicData;
        ballState = new BallState(16);
        Setup8BallPositionForNewMatch();
    }

    // Xep 8 bi cho van dau 
    public void Setup8BallPositionForNewMatch()
    {
        float R = physicData.BallRadius;
        float diameter = R * 2.01f;
        float rowDistance = R * 1.734f;

        ballState.SetPosition(0, physicData.HeadSpot);
        ballState.IsActive[0] = true;

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

        PhysicVector3 apex = physicData.FootSpot;

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col <= row; col++)
            {
                int ballID = rackScheme[row, col];

                float posZ = apex.Z + (row * rowDistance);
                float posX = apex.X + (col - (row / 2f)) * diameter;

                ballState.SetPosition(ballID, new PhysicVector3(posX, 0, posZ));
                ballState.IsActive[ballID] = true;
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
