using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueOverviewState : ICueState
{
    private CueView _view;
    private CueInteraction _interaction;

    public CueOverviewState(CueView view, CueInteraction interaction)
    {
        _view = view;
        _interaction = interaction;
    }

    public void Enter()
    {
        Vector3 lastShotDir = _view.transform.forward;
        _view.overviewAnchor.rotation = Quaternion.LookRotation(new Vector3(lastShotDir.x, 0, lastShotDir.z));
        _view.firstCamera.gameObject.SetActive(false);
        _view.secondCamera.gameObject.SetActive(true);
        _view.cueModel.gameObject.SetActive(false);
    }

    public void HandleInput()
    {
        float mouseX = _view.playerInputController.GetHorizontalAxis();
        float mouseY = _view.playerInputController.GetVerticalAxis();

        _interaction.CaculateAngle(mouseX, _view.Sensitivity);
        _interaction.CaculateHeightSecondCam(mouseY, _view.cameraConfig.minHeight, _view.cameraConfig.maxHeight, _view.cameraConfig.heightSpeed);
    }

    public void UpdateView()
    {
        _view.secondCamera.transform.LookAt(_view.overviewAnchor.position);
        _view.overviewAnchor.rotation = Quaternion.Euler(0, _interaction.CurrentAngle, 0);
        _view.cameraOffset.localPosition = new Vector3(1.7f, _interaction.HeightSecondCam, 0);

        if (_view.playerInputController.IsSwitchViewPressed())
        {
            _view.ChangeState(_view.AimState);
        }
    }

    public void Exit()
    {
        _view.secondCamera.gameObject.SetActive(false);
        _view.cueModel.gameObject.SetActive(true);
        _view.firstCamera.gameObject.SetActive(true);
    }
}
