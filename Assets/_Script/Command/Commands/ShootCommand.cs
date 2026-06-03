using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCommand : ICommand
{
    private readonly MatchManager _matchManager;
    private readonly PhysicVector3 _direction;
    private readonly float _force;

    public ShootCommand(MatchManager matchManager, PhysicVector3 direction, float force)
    {
        _matchManager = matchManager;
        _direction = direction;
        _force = force;
    }

    public void Execute()
    {
        _matchManager.ExecuteShoot(_direction, _force);
        AudioManager.Instance.PlayCueHitSound(_force);
    }
}
