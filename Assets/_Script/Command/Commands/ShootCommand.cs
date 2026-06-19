using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCommand : ICommand
{
    private readonly MatchManager _matchManager;
    private readonly PhysicVector3 _direction;
    private readonly float _force;
    private readonly PhysicVector2 _currentSpinPoint;

    public ShootCommand(MatchManager matchManager, PhysicVector3 direction, float force, PhysicVector2 currentSpinPoint)
    {
        _matchManager = matchManager;
        _direction = direction;
        _force = force;
        _currentSpinPoint = currentSpinPoint;
    }

    public void Execute()
    {
        _matchManager.ExecuteShoot(_direction, _force, _currentSpinPoint);
        AudioManager.Instance.PlayCueHitSound(_force);
    }
}
