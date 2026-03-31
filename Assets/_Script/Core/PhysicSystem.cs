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
        for(int i=0; i<_ballState.TotalBalls; i++)
        {
            if (!_ballState.IsActive[i]) continue;

            PhysicVector3 velocity = _ballState.Velocities[i];

            if (velocity.SqrMagnitude() < 0.001f)
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
}
