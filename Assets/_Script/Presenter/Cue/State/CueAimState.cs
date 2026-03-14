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
        _view.transform.rotation = Quaternion.Euler(0, _interaction.CurrentAngle, 0);

        _view.transform.position = _interaction.GetPositionAroundBall0(
            _view.ball0Transform.position,
            _view.ballPhysicConfig.radius,
            _view.cuePhysicConfig.offsetFromBall0);

        if (_view.cameraPlayer != null)
        {
            _view.cameraPlayer.rotation = Quaternion.Euler(_interaction.CameraPitch, _interaction.CurrentAngle, 0);
        }
    }

    public void Exit() { }
}
