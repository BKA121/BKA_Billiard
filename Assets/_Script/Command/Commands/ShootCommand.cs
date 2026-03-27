using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCommand : ICommand
{
    private readonly Vector3 _direction;
    private readonly float _force;

    public ShootCommand(Vector3 direction, float force)
    {
        _direction = direction;
        _force = force;
    }

    public void Execute()
    {
        
    }
}
