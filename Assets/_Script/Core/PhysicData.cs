using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PhysicVector3
{
    public float X;
    public float Y;
    public float Z;

    public PhysicVector3(float x, float y, float z) {X = x; Y = y; Z = z;}

    // Dinh nghia toan tu + - * / cho struct
    public static PhysicVector3 operator +(PhysicVector3 a, PhysicVector3 b)
    {
        return new PhysicVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }
    public static PhysicVector3 operator -(PhysicVector3 a, PhysicVector3 b)
    {
        return new PhysicVector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }
    public static PhysicVector3 operator *(PhysicVector3 a, float d)
    {
        return new PhysicVector3(a.X * d, a.Y * d, a.Z * d);
    }
    public static PhysicVector3 operator *(float d, PhysicVector3 a)
    {
        return new PhysicVector3(a.X * d, a.Y * d, a.Z * d);
    }
    public static PhysicVector3 operator /(PhysicVector3 a, float d)
    {
        return new PhysicVector3(a.X / d, a.Y / d, a.Z / d);
    }

    public float SqrMagnitude() => X * X + Y * Y + Z * Z;

    public float Magnitude() => (float)System.Math.Sqrt(SqrMagnitude());

    public static float Distance(PhysicVector3 a, PhysicVector3 b)
    {
        float dx = a.X - b.X;
        float dy = a.Y - b.Y;
        float dz = a.Z - b.Z;
        return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public float Dot(PhysicVector3 other)
    {
        return X * other.X + Y * other.Y + Z * other.Z;
    }

    public PhysicVector3 Normalize()
    {
        float mag = Magnitude();
        if (mag > 0.00001f) return this / mag;
        return new PhysicVector3(0, 0, 0);
    }
}

public class PhysicData
{
    // TableSize data 
    public float Length { get; }
    public float Width { get; }
    public PhysicVector3 HeadSpot { get; }
    public PhysicVector3 FootSpot { get; }
    public PhysicVector3[] PocketCenters { get; }
    public float Mr { get; } // ma sat lan
    public float WallBounce { get; } // he so nay thanh bang


    // BallPhysicConfig data
    public float BallRadius { get; }
    public float mBall { get; } // khoi luong ball
    public float ballRestitution { get; } // he so hoi phuc sau va cham bi


    public PhysicData(float length, float width, float Mr, float WallBounce,
                      float ballRadius, float mass, float ballRestitution,
                      PhysicVector3 headSpot, PhysicVector3 footSpot, PhysicVector3[] pockets)
    {
        Length = length;
        Width = width;
        this.Mr = Mr;
        this.WallBounce = WallBounce;

        this.mBall = mass;
        BallRadius = ballRadius;
        this.ballRestitution = ballRestitution;

        HeadSpot = headSpot;
        FootSpot = footSpot;
        PocketCenters = pockets;
    }
}