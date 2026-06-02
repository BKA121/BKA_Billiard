using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicSystem 
{
    private PhysicData _physicData;
    private float _limitX;
    private float _limitZ;
    private float _offsetPocketCorner;
    private float _offsetPocketCenter;
    private float _boundTableX;
    private float _boundTableZ;
    private float _r;
    private PhysicVector3 _direction;
    private PocketDataPhysicVector3[] _pockets;
    private float _force;
    private BallState _ballState;
    private ShotResult _shotResult;

    public PhysicSystem(PhysicData physicData, BallState ballState, ShotResult shotResult)
    {
        _ballState = ballState;
        _shotResult = shotResult;

        _physicData = physicData;
        _r = _physicData.BallRadius;
        _pockets = _physicData.Pockets;
        _limitX = _physicData.Width/2;
        _limitZ = _physicData.Length/2;
        _offsetPocketCenter = _physicData.offsetPocketCenter;
        _offsetPocketCorner = _physicData.offsetPocketCorner;
        _boundTableX = _limitX - _offsetPocketCorner;
        _boundTableZ = _limitZ - _offsetPocketCorner - _offsetPocketCenter;
    }

    public void InitialShoot(PhysicVector3 direction, float force)
    {
        _direction = direction;
        _force = force;
        
        PhysicVector3 horizontalDir = new PhysicVector3(direction.X, 0, direction.Z);

        horizontalDir = horizontalDir.Normalize();

        PhysicVector3 initialVelocity = horizontalDir * force;

        _ballState.SetVelocity(0, initialVelocity);
    }

    public void UpdatePhysicForFrame(float dt)
    {
        // Xu ly va cham bi voi bi
        ResolveBallToBallCollisions();

        for (int i=0; i<_ballState.TotalBalls; i++)
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

            if (!_ballState.IsDropping[i])
            {
                // Xu ly va cham voi thanh bang
                if ((0 <= newPos.Z - _offsetPocketCenter && newPos.Z - _offsetPocketCenter <= _boundTableZ) ||
                    (0 >= newPos.Z + _offsetPocketCenter && newPos.Z + _offsetPocketCenter >= -_boundTableZ))
                {
                    if (newPos.X < -_limitX || newPos.X > _limitX)
                    {
                        velocity.X = -velocity.X;
                        newPos.X = newPos.X < -_limitX ? -_limitX : _limitX;

                        velocity.X *= _physicData.WallBounce;
                        if (_shotResult.firstBallHitID != -1 && !_shotResult.ballHitCushionAfterShot.Contains(i))
                        {
                            _shotResult.ballHitCushionAfterShot.Add(i);
                        }
                    }
                }

                if (-_boundTableX <= newPos.X && newPos.X <= _boundTableX)
                {
                    if (newPos.Z < -_limitZ || newPos.Z > _limitZ)
                    {
                        velocity.Z = -velocity.Z;
                        newPos.Z = newPos.Z < -_limitZ ? -_limitZ : _limitZ;

                        velocity.Z *= _physicData.WallBounce;
                        if (_shotResult.firstBallHitID != -1 && !_shotResult.ballHitCushionAfterShot.Contains(i))
                        {
                            _shotResult.ballHitCushionAfterShot.Add(i);
                        }
                    }
                }
            }

            // Xu ly va cham voi mieng lo
            float minDistance = _r;
            float finalDistance = 0f;
            PhysicVector3 finalQ = new PhysicVector3(0, 0, 0);       
            for (int p = 0; p < _pockets.Length; p++)
            {
                float distanceToCenter = PhysicVector3.Distance(newPos, _pockets[p].center);

                if (distanceToCenter > 0.21f) continue;

                // Xu ly roi xuong lo
                if (distanceToCenter < _pockets[p].rPocket || _ballState.IsDropping[i])
                {
                    if (!_shotResult.pocketedBallIDs.Contains(i))
                    {
                        _shotResult.pocketedBallIDs.Add(i);
                        MatchManager.Instance.OnChangeColorBallPocketed?.Invoke(i);
                    }

                    if (i == 0) _shotResult.isBall0Pocketed = true;
                    if (i == 8) _shotResult.isBall8Pocketed = true;

                    PhysicVector3 dirToCenter = (_pockets[p].center - newPos).Normalize();
                    float speed = velocity.Magnitude();

                    velocity = velocity + dirToCenter * (speed + 0.5f) * 3f * dt;
                    velocity = velocity * 0.98f;

                    float gravityFall = 0.1f * dt;
                    float velocityFall = speed * dt;

                    newPos.Y -= (velocityFall + gravityFall);

                    if (!_ballState.IsDropping[i]) _ballState.DropBall(i);

                    if (newPos.Y < -0.14f)
                    {
                        _ballState.DeactivateBall(i);
                        velocity.X = 0; velocity.Y = 0; velocity.Z = 0;
                    }
                    break;
                }

                // Xy ly va cham mom lo
                PhysicVector3 upQ = FindProjectionPoint(newPos, _pockets[p].upA, _pockets[p].upB);
                PhysicVector3 downQ = FindProjectionPoint(newPos, _pockets[p].downA, _pockets[p].downB);

                float dUp = PhysicVector3.Distance(newPos, upQ);
                float dDown = PhysicVector3.Distance(newPos, downQ);

                if (dUp < _r || dDown < _r)
                {
                    if (dUp < dDown) { finalDistance = dUp; finalQ = upQ; }
                    else { finalDistance = dDown; finalQ = downQ; }
                    break; 
                }
            }

            // Xy ly va cham mom lo
            if (finalDistance > 0 && !_ballState.IsDropping[i])
            {
                if (_shotResult.firstBallHitID != -1 && !_shotResult.ballHitCushionAfterShot.Contains(i))
                {
                    _shotResult.ballHitCushionAfterShot.Add(i);
                }
                float overlap = _r - finalDistance;

                PhysicVector3 collisionNormal = (newPos - finalQ).Normalize();

                newPos = newPos + (collisionNormal * overlap);

                float vDotN = velocity.Dot(collisionNormal);

                if (vDotN < 0)
                {
                    PhysicVector3 vectorNormal = vDotN * collisionNormal;
                    PhysicVector3 vectorTangent = velocity - vectorNormal;

                    velocity = vectorTangent - (vectorNormal * _physicData.WallBounce);
                }
            }

            _ballState.SetPosition(i, newPos);

            // Giam van toc moi frame do ma sat lan
            if(!_ballState.IsDropping[i]) velocity *= (1f - _physicData.Mr * dt);
            _ballState.SetVelocity(i, velocity);
        }
    }

    public PhysicVector3 FindProjectionPoint(PhysicVector3 pBall0, PhysicVector3 a, PhysicVector3 b)
    {
        PhysicVector3 ap = pBall0 - a;
        PhysicVector3 ab = b - a;

        float t = ab.Dot(ap) / ab.SqrMagnitude();
        t = Mathf.Clamp01(t);

        return a + t * ab;
    }

    public void ResolveBallToBallCollisions()
    {
        for (int i = 0; i < _ballState.TotalBalls; i++)
        {
            for (int j = i + 1; j < _ballState.TotalBalls; j++)
            {
                if (!_ballState.IsActive[i] || !_ballState.IsActive[j]) continue;
                if (_ballState.IsDropping[i] || _ballState.IsDropping[j]) continue;

                float distance = PhysicVector3.Distance(_ballState.Positions[i], _ballState.Positions[j]);
                float minDistance = _r * 2.01f;

                if (distance < minDistance)
                {
                    if (i == 0 && _shotResult.firstBallHitID == -1) _shotResult.firstBallHitID = j;
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

    public bool CheckBallsStatic()
    {
        for(int i = 0; i < _ballState.TotalBalls; i++)
        {
            if (!_ballState.IsActive[i]) continue;

            if (_ballState.Velocities[i].SqrMagnitude() != 0) return false;
        }
        return true;
    }
}
