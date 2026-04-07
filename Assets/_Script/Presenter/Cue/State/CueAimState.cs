using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueAimState : ICueState
{
    private CueView _view;
    private CueInteraction _interaction;

    public CueAimState(CueView view, CueInteraction interaction)
    {
        _view = view;
        _interaction = interaction;
    }

    public void Enter() { }

    public void HandleInput()
    {
        float mouseX = _view.playerInputController.GetHorizontalAxis();
        float mouseY = _view.playerInputController.GetVerticalAxis();

        _interaction.CaculateAngle(mouseX, _view.Sensitivity);
        _interaction.CaculateCameraPitch(mouseY, _view.cameraConfig.minPitch, _view.cameraConfig.maxPitch, _view.Sensitivity);
    }

    public void UpdateView()
    {
        _view.transform.rotation = Quaternion.Euler(7f, _interaction.CurrentAngle, 0);

        _view.transform.position = _interaction.GetPositionAroundBall0(_view.ball0Position);

        if (_view.firstCamera != null)
        {
            _view.firstCamera.localRotation = Quaternion.Euler(_interaction.CameraPitch, 0, 0);        
        }

        if (_view.playerInputController.IsShootAction())
        {
            _view.ChangeState(_view.PowerState);
        }
    }

    public void Exit() { }
}
