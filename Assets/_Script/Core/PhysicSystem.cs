using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicSystem 
{
    private PhysicData _physicData;
    private float _limitX;
    private float _limitZ;
    private float _r;
    private PhysicVector3 _direction;
    private float _force;
    private BallState _ballState;

    public PhysicSystem(PhysicData physicData)
    {
        _physicData = physicData;
        _r = _physicData.BallRadius;
        _limitX = _physicData.Width/2 - _r;
        _limitZ = _physicData.Length/2 - _r;
    }

    public void InitialShoot(PhysicVector3 direction, float force, BallState ballState)
    {
        _direction = direction;
        _force = force;
        _ballState = ballState;
        
        PhysicVector3 horizontalDir = new PhysicVector3(direction.X, 0, direction.Z);

        horizontalDir = horizontalDir.Normalize();

        PhysicVector3 initialVelocity = horizontalDir * force;

        ballState.SetVelocity(0, initialVelocity);
    }

    public void UpdatePhysicForFrame(float dt)
    {
        ResolveBallToBallCollisions();

        for (int i=0; i<_ballState.TotalBalls; i++)
        {
            if (!_ballState.IsActive[i]) continue;

            PhysicVector3 velocity = _ballState.Velocities[i];

            if (velocity.SqrMagnitude() < 0.0001f)
            {
                _ballState.SetVelocity(i, new PhysicVector3(0, 0, 0));
                continue;
            }

            // Cap nhat vi tri moi
            PhysicVector3 newPos = _ballState.Positions[i] + (velocity * dt);

            if (newPos.X < -_limitX || newPos.X > _limitX)
            {
                velocity.X = -velocity.X;                              
                newPos.X = newPos.X < -_limitX ? -_limitX : _limitX;

                velocity.X *= _physicData.WallBounce; 
            }

            if (newPos.Z < -_limitZ || newPos.Z > _limitZ)
            {
                velocity.Z = -velocity.Z; 
                newPos.Z = newPos.Z < -_limitZ ? -_limitZ : _limitZ;

                velocity.Z *= _physicData.WallBounce;
            }

            _ballState.SetPosition(i, newPos);

            // Giam van toc moi frame do ma sat lan
            velocity *= Mathf.Pow(_physicData.Mr, dt);
            _ballState.SetVelocity(i, velocity);
        }
    }

    public void ResolveBallToBallCollisions()
    {
        for (int i = 0; i < _ballState.TotalBalls; i++)
        {
            for (int j = i + 1; j < _ballState.TotalBalls; j++)
            {
                if (!_ballState.IsActive[i] || !_ballState.IsActive[j]) continue;

                float distance = PhysicVector3.Distance(_ballState.Positions[i], _ballState.Positions[j]);
                float minDistance = _r * 2.01f;

                if (distance < minDistance)
                {
                    HandleBallCollision(i, j, distance, minDistance);
                }
            }
        }
    }

    private void HandleBallCollision(int i, int j, float distance, float minDistance)
    {
        PhysicVector3 posI = _ballState.Positions[i];
        PhysicVector3 posJ = _ballState.Positions[j];
        PhysicVector3 velI = _ballState.Velocities[i];
        PhysicVector3 velJ = _ballState.Velocities[j];

        float overlap = minDistance - distance;

        // Vector noi tam da chuan hoa
        PhysicVector3 collisionNormal = (posI - posJ).Normalize();

        // Dich toa do bi tranh chong lap
        PhysicVector3 separation = collisionNormal * (overlap * 0.5f);
        _ballState.SetPosition(i, posI + separation);
        _ballState.SetPosition(j, posJ - separation);

        // Thanh phan do lon van toc tren duong noi tam
        float vInormal = velI.Dot(collisionNormal);
        float vJnormal = velJ.Dot(collisionNormal);

        if (vInormal - vJnormal > 0) return;

        // Thanh phan vector van toc tren duong noi tam
        PhysicVector3 velInormal = collisionNormal * vInormal;
        PhysicVector3 velJnormal = collisionNormal * vJnormal;

        // Thanh phan vector van toc theo phuong tiep tuyen
        PhysicVector3 velItangent = velI - velInormal;
        PhysicVector3 velJtangent = velJ - velJnormal;

        float e = _physicData.ballRestitution; 

        PhysicVector3 nextVelInormal = velJnormal * e;
        PhysicVector3 nextVelJnormal = velInormal * e;

        _ballState.SetVelocity(i, velItangent + nextVelInormal);
        _ballState.SetVelocity(j, velJtangent + nextVelJnormal);
    }

}
