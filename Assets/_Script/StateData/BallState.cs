using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallState
{
    public int TotalBalls { get; }
    public PhysicVector3[] Positions { get; private set; }
    public PhysicVector3[] Velocities { get; private set; }
    public bool[] IsActive { get; private set; }

    public BallState(int totalBalls)
    {
        TotalBalls = totalBalls;
        Positions = new PhysicVector3[totalBalls];
        IsActive = new bool[totalBalls];
        Velocities = new PhysicVector3[totalBalls];

        for (int i = 0; i < totalBalls; i++) IsActive[i] = true;
    }

    public void SetPosition(int index, PhysicVector3 pos) => Positions[index] = pos;
    public void SetVelocity(int index, PhysicVector3 vel) => Velocities[index] = vel;
    public void DeactivateBall(int index) => IsActive[index] = false;
}