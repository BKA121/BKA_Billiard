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

        if (Input.GetKeyUp(KeyCode.S))
        {
            _view.ChangeState(_view.AimState);
        }
    }

    public void UpdateView()
    {
        if (_view.cueModel != null)
        {
            _view.cueModel.localPosition = new Vector3(0, 0, _defaultLocalZ + _interaction.CurrentPull);
        }

        if (_interaction.CurrentPull <= -0.01f)
        {
            OnCueHitBall();
            _view.ChangeState(_view.AimState); // Sau la chuyen ve trang thai an cue
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
        Vector3 dir = _view.transform.forward;

        // tinh lai luc danh
        float force = _view.cuePhysicConfig.sensitivityPull;

        ICommand shoot = new ShootCommand(
            CommandDispatcher.dispatcher.CoreManager,
            dir,
            force
        );

        CommandDispatcher.dispatcher.ExecuteCommand(shoot);
    }
}