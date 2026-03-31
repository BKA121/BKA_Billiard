using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueHintState : ICueState
{
    private CueView _view;
    private CueInteraction _interaction;

    public CueHintState(CueView view, CueInteraction interaction)
    {
        _view = view;
        _interaction = interaction;
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void HandleInput()
    {
        
    }

    public void UpdateView()
    {
        if (_view.playerInputController.IsSwitchViewPressed())
        {
            _view.ChangeState(_view.AimState);
        }
    }
}
