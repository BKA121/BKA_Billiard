using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoreManager 
{
    private PhysicSystem _physicSystem;
    private GameState _gameState;

    public CoreManager(GameState gameState)
    {
        _gameState = gameState;
        _physicSystem = new PhysicSystem();
    }

    public void HandlePhysic(Vector3 direction, float force)
    {
        _physicSystem.CaculateShoot(direction, force);
    }
}
