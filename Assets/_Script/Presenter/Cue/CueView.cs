using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueView : MonoBehaviour
{
    private CueInteraction _cueInteraction = new CueInteraction();
    private BallState _ballState; 
    private bool _isInitialized = false;
    private PhysicVector3 ball0Pos;

    public CueAimState AimState { get; private set; }
    public CuePowerState PowerState { get; private set; }
    public CueOverviewState OverViewState { get; private set; }

    public ICueState currentState;
    public Transform cueModel;
    public Vector3 ball0Position;
    public PlayerInputController playerInputController;
    public BallPhysicConfig ballPhysicConfig;
    public CuePhysicConfig cuePhysicConfig;
    public CameraConfig cameraConfig;
    public Transform firstCamera;
    public Transform overviewAnchor;
    public Transform cameraOffset;
    public Camera secondCamera;
    public BallInteraction ballInteraction;

    public void Initialize(BallState ballState)
    {
        _ballState = ballState;
        _isInitialized = true;

        AimState = new CueAimState(this, _cueInteraction);
        PowerState = new CuePowerState(this, _cueInteraction);
        OverViewState = new CueOverviewState(this, _cueInteraction);

        ChangeState(OverViewState);
    }

    private void Update()
    {
        if (!_isInitialized || currentState == null) return;

        UpdateCuePosition(); // Luon cap nhat vi tri bi trang cho cue

        if (ballInteraction.isMModeActive) return;

        currentState.HandleInput();
        currentState.UpdateView();
    }

    public void ChangeState(ICueState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    private void UpdateCuePosition()
    {
        ball0Pos = _ballState.Positions[0];

        ball0Position = new Vector3(ball0Pos.X, ball0Pos.Y, ball0Pos.Z);
    }
}
