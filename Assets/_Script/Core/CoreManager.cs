using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoreManager 
{
    private PhysicSystem _physicSystem;
    private GameState _gameState;

    public bool isCaculateShoot = false;

    public CoreManager(GameState gameState)
    {
        _gameState = gameState;
        _physicSystem = new PhysicSystem(_gameState.physicData);
    }

    // Bat cong tac isCaculateShoot de update trong gamecontroller goi CaculateShoot
    public void PrepareCaculateShoot(PhysicVector3 direction, float force)
    {
        _physicSystem.InitialShoot(direction, force, _gameState.ballState);
        isCaculateShoot = true;
    }

    public void CaculateShoot(float dt)
    {
        if (!isCaculateShoot) return;

        _physicSystem.UpdatePhysicForFrame(dt);

        //if (_physicSystem.AreBallsStatic())
        //{
        //    _isCaculateShoot = false;
        //}
    }
}
