using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuePowerState : ICueState
{
    private CueView _view;
    private CueInteraction _interaction;
    private float _defaultLocalZ; // = radius + offsetFromBall0

    public CuePowerState(CueView view, CueInteraction interaction)
    {
        _view = view;
        _interaction = interaction;
    }

    public void Enter()
    {
        if (_view.cueModel != null) _defaultLocalZ = _view.cueModel.localPosition.z;
    }

    public void HandleInput()
    {
        float mouseY = _view.playerInputController.GetVerticalAxis();
        _interaction.CalculatePull(mouseY, _view.cuePhysicConfig.sensitivityPull, _view.cuePhysicConfig.maxPull);

        if (_view.playerInputController.IsExitShootAction())
        {
            _view.ChangeState(_view.AimState);
        }
    }

    public void UpdateView()
    {
        if (_view.cueModel != null)
        {
            _view.cueModel.localPosition = new Vector3(0, 0, _defaultLocalZ - _interaction.CurrentPull);
        }

        if (_interaction.CurrentPull <= -0.01f)
        {
            OnCueHitBall();
            _view.ChangeState(_view.OverViewState); 
        }
    }

    public void Exit()
    {
        _interaction.ResetPull();
        if (_view.cueModel != null)
            _view.cueModel.localPosition = new Vector3(0, 0, _defaultLocalZ);
    }

    private void OnCueHitBall()
    {
        PhysicVector3 dir = new PhysicVector3(_view.transform.forward.x, _view.transform.forward.y, _view.transform.forward.z);
        float speed = _interaction.CurrentStrikeSpeed * _view.cuePhysicConfig.forceMultiplier;

        // Tinh luc danh
        float force = speed;

        ICommand shoot = new ShootCommand(CommandDispatcher.Instance.coreManager, dir, force);

        CommandDispatcher.Instance.ExecuteCommand(shoot);
    }
}