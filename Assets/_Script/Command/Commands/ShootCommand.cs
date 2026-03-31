using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCommand : ICommand
{
    private readonly CoreManager _coreManager;
    private readonly PhysicVector3 _direction;
    private readonly float _force;

    public ShootCommand(CoreManager coreManager, PhysicVector3 direction, float force)
    {
        _coreManager = coreManager;
        _direction = direction;
        _force = force;
    }

    public void Execute()
    {
        _coreManager.PrepareCaculateShoot(_direction, _force);
    }
}
