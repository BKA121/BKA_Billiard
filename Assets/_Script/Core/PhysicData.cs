using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PhysicVector3
{
    public float X, Y, Z;
    public PhysicVector3(float x, float y, float z) { X = x; Y = y; Z = z; }
}

public class PhysicData
{
    // TableSize data 
    public float Length { get; }
    public float Width { get; }
    public PhysicVector3 HeadSpot { get; }
    public PhysicVector3 FootSpot { get; }
    public PhysicVector3[] PocketCenters { get; }


    // BallPhysicConfig data
    public float BallRadius { get; }


    public PhysicData(float length, float width, float ballRadius,
                      PhysicVector3 headSpot, PhysicVector3 footSpot, PhysicVector3[] pockets)
    {
        Length = length;
        Width = width;
        BallRadius = ballRadius;
        HeadSpot = headSpot;
        FootSpot = footSpot;
        PocketCenters = pockets;
    }
}