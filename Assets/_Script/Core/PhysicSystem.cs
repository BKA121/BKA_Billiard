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
    private float _mBall;
    private PhysicVector3 _direction;
    private PocketDataPhysicVector3[] _pockets;
    private float _force;
    private BallState _ballState;
    private ShotResult _shotResult;

    private float _inertia;
    private float _gravity = 9f;       
    private float _muK = 0.075f;         
    private float _muR = 0.01f;       

    private PhysicVector3 _wallXNormal = new PhysicVector3(1, 0, 0);
    private PhysicVector3 _wallZNormal = new PhysicVector3(0, 0, -1);

    public PhysicSystem(PhysicData physicData, BallState ballState, ShotResult shotResult)
    {
        _ballState = ballState;
        _shotResult = shotResult;

        _physicData = physicData;
        _r = _physicData.BallRadius;
        _mBall = _physicData.mBall;
        _pockets = _physicData.Pockets;
        _limitX = _physicData.Width/2;
        _limitZ = _physicData.Length/2;
        _offsetPocketCenter = _physicData.offsetPocketCenter;
        _offsetPocketCorner = _physicData.offsetPocketCorner;
        _boundTableX = _limitX - _offsetPocketCorner;
        _boundTableZ = _limitZ - _offsetPocketCorner - _offsetPocketCenter;

        _inertia = 0.4f * _mBall * _r * _r;
}

    public void InitialShoot(PhysicVector3 direction, float force, PhysicVector2 currentSpinPoint)
    {
        _direction = direction;
        _force = force;
        
        PhysicVector3 horizontalDir = new PhysicVector3(direction.X, 0, direction.Z);

        horizontalDir = horizontalDir.Normalize();

        PhysicVector3 initialVelocity = horizontalDir * force;

        _ballState.SetVelocity(0, initialVelocity);

        PhysicVector3 forward = horizontalDir;
        PhysicVector3 up = new PhysicVector3(0f, 1f, 0f);
        PhysicVector3 right = new PhysicVector3(forward.Z, 0f, -forward.X);

        float x = currentSpinPoint.X;
        float y = currentSpinPoint.Y;
        float z = (float)Math.Sqrt(Math.Max(0f, (_r * _r) - (x * x) - (y * y)));

        PhysicVector3 worldOffset = (right * x) + (up * y) - (forward * z);
        PhysicVector3 impulse = forward * force;
        float tx = worldOffset.Y * impulse.Z - worldOffset.Z * impulse.Y;
        float ty = worldOffset.Z * impulse.X - worldOffset.X * impulse.Z;
        float tz = worldOffset.X * impulse.Y - worldOffset.Y * impulse.X;
        PhysicVector3 torque = new PhysicVector3(tx, ty, tz);

        PhysicVector3 initialAngularVelocity = new PhysicVector3(torque.X / _inertia, torque.Y / _inertia, torque.Z / _inertia);

        _ballState.AngularVelocities[0] = initialAngularVelocity;
        _ballState.Rotations[0] = PhysicQuaternion.Identity;
    }

    public void UpdatePhysicForFrame(float dt)
    {
        // Xu ly va cham bi voi bi
        ResolveBallToBallCollisions();

        for (int i=0; i<_ballState.TotalBalls; i++)
        {
            if (!_ballState.IsActive[i]) continue;

            PhysicVector3 velocity = _ballState.Velocities[i];

            if (velocity.SqrMagnitude() == 0 && _ballState.AngularVelocities[i].SqrMagnitude() == 0) continue;

            // Cap nhat vi tri moi
            PhysicVector3 newPos = _ballState.Positions[i];

            if (!_ballState.IsDropping[i])
            {
                newPos += (velocity * dt);
                // Xu ly va cham voi thanh bang
                if ((0 <= newPos.Z - _offsetPocketCenter && newPos.Z - _offsetPocketCenter <= _boundTableZ) ||
                    (0 >= newPos.Z + _offsetPocketCenter && newPos.Z + _offsetPocketCenter >= -_boundTableZ))
                {
                    if (newPos.X < -_limitX || newPos.X > _limitX)
                    {
                        PhysicVector3 wallNormal = new PhysicVector3(newPos.X > 0 ? -1f : 1f, 0f, 0f);
                        ResolveCushionCollision(i, wallNormal, ref velocity, ref newPos);
                    }
                }
                if (-_boundTableX <= newPos.X && newPos.X <= _boundTableX)
                {
                    if (newPos.Z < -_limitZ || newPos.Z > _limitZ)
                    {
                        PhysicVector3 wallNormal = new PhysicVector3(0f, 0f, newPos.Z > 0 ? -1f : 1f);
                        ResolveCushionCollision(i, wallNormal, ref velocity, ref newPos);
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

                    if (!_ballState.IsDropping[i]) _ballState.DropBall(i);

                    PhysicVector3 dirToCenter = (_pockets[p].center - newPos).Normalize();
                    float speed = velocity.Magnitude();

                    velocity = velocity + dirToCenter * (speed + 0.5f) * 2f * dt;
                    velocity = velocity * 0.98f;

                    float gravityFall = 0.18f * dt;
                    float velocityFall = speed * dt;

                    float force = velocityFall + gravityFall;

                    newPos.Y -= force;

                    if (newPos.Y < -0.15f)
                    {
                        AudioManager.Instance.PlayPocketSound(force);
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
                    if (dUp < dDown)
                    {
                        finalDistance = dUp; finalQ = upQ;
                        // Xu ly am thanh khi va cham mieng lo tren
                        PlayAudio(velocity, _pockets[p].upA, _pockets[p].upB);
                    }
                    else
                    {
                        finalDistance = dDown; finalQ = downQ;
                        // Xu ly am thanh khi va cham mieng lo duoi
                        PlayAudio(velocity, _pockets[p].downA, _pockets[p].downB);
                    }
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

            // Xu ly ma sat va xoay
            if (!_ballState.IsDropping[i])
            {
                PhysicVector3 angularVel = _ballState.AngularVelocities[i];
                PhysicVector3 rDown = new PhysicVector3(0, -_r, 0);
                PhysicVector3 vContact = velocity + angularVel.Cross(rDown); // van toc tai diem tiep xuc
                float vContactMag = vContact.Magnitude();

                // Luong giam van toc toi da trong 1frame
                float maxVContactDrop = 2.5f * _muK * _gravity * dt; 

                if (vContactMag > maxVContactDrop)
                {
                    PhysicVector3 slideDirection = vContact.Normalize();
                    // vector ma sat truot
                    PhysicVector3 frictionForce = slideDirection * (-_muK * _mBall * _gravity);

                    velocity = velocity + (frictionForce * (1f / _mBall)) * dt;

                    PhysicVector3 torque = rDown.Cross(frictionForce);
                    angularVel = angularVel + (torque * (1f / _inertia)) * dt;
                }
                else
                {
                    float vMag = velocity.Magnitude();

                    float maxVRollDrop = _muR * _gravity * dt;

                    if (vMag > maxVRollDrop)
                    {
                        PhysicVector3 rollDirection = velocity.Normalize();
                        velocity = velocity - (rollDirection * maxVRollDrop);

                        PhysicVector3 rUp = new PhysicVector3(0, _r, 0);
                        angularVel = rUp.Cross(velocity) * (1f / (_r * _r));
                    }
                    else
                    {
                        velocity = new PhysicVector3(0, 0, 0);
                        angularVel = new PhysicVector3(0, 0, 0);
                    }
                }

                _ballState.SetVelocity(i, velocity);
                _ballState.AngularVelocities[i] = angularVel;

                if (angularVel.SqrMagnitude() > 0.01f)
                {
                    _ballState.Rotations[i] = _ballState.Rotations[i].Integrate(angularVel, dt);
                }
            }
        }
    }

    private void ResolveCushionCollision(int ballIndex, PhysicVector3 wallNormal, ref PhysicVector3 velocity, ref PhysicVector3 newPos)
    {
        float forceRatio = Mathf.Abs(velocity.Dot(wallNormal));
        AudioManager.Instance.PlayBallHitTableSound(forceRatio);

        if (_shotResult.firstBallHitID != -1 && !_shotResult.ballHitCushionAfterShot.Contains(ballIndex))
        {
            _shotResult.ballHitCushionAfterShot.Add(ballIndex);
        }

        PhysicVector3 currentAngularVel = _ballState.AngularVelocities[ballIndex];

        if (wallNormal.X != 0)
        {
            velocity.X = -velocity.X * _physicData.WallBounce;
            newPos.X = newPos.X < -_limitX ? -_limitX : _limitX;
        }
        else if (wallNormal.Z != 0)
        {
            velocity.Z = -velocity.Z * _physicData.WallBounce;
            newPos.Z = newPos.Z < -_limitZ ? -_limitZ : _limitZ;
        }

        PhysicVector3 rContact = wallNormal * -_r;
        PhysicVector3 vContact = velocity + currentAngularVel.Cross(rContact);
        PhysicVector3 vSlide = vContact - (wallNormal * vContact.Dot(wallNormal));

        float cushionFriction = 0.07f;
        PhysicVector3 frictionImpulse = vSlide * -cushionFriction;

        frictionImpulse.Y = 0f;

        velocity = velocity + frictionImpulse;

        velocity.Y = 0f;

        PhysicVector3 torque = rContact.Cross(frictionImpulse);
        _ballState.AngularVelocities[ballIndex] = currentAngularVel + (torque * (1f / _inertia));
    }

    private void PlayAudio(PhysicVector3 velocity, PhysicVector3 a, PhysicVector3 b)
    {
        PhysicVector3 direction = (a - b).Normalize();

        PhysicVector3 normal = new PhysicVector3(-direction.Z, 0f, direction.X);

        float force = Mathf.Abs(velocity.Dot(normal));

        AudioManager.Instance.PlayBallHitTableSound(force);
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
                float minDistance = _r * 2f;

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

        // Xu ly am thanh va cham
        PhysicVector3 relativeVelocity = velI - velJ;
        float impactForce = Mathf.Abs(relativeVelocity.Dot(collisionNormal));
        AudioManager.Instance.PlayBallHitSound(impactForce);

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

            if (_ballState.Velocities[i].SqrMagnitude() != 0 || _ballState.AngularVelocities[i].SqrMagnitude() != 0) return false;
        }
        return true;
    }
}
