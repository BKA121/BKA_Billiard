using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueView : MonoBehaviour
{
    private CueInteraction _cueInteraction = new CueInteraction();
    [SerializeField] private float _sensitivity = 0.2f;
    private ICueState _currentState;
    private BallState _ballState; 
    private bool _isInitialized = false;
    private PhysicVector3 ball0Pos;

    public CueAimState AimState { get; private set; }
    public CuePowerState PowerState { get; private set; }
    public CueOverviewState OverViewState { get; private set; }

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
    public float Sensitivity => _sensitivity;

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
        if (!_isInitialized || _currentState == null) return;

        UpdateCuePosition(); // Luon cap nhat vi tri bi trang cho cue

        _currentState.HandleInput();
        _currentState.UpdateView();
    }

    public void ChangeState(ICueState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    private void UpdateCuePosition()
    {
        ball0Pos = _ballState.Positions[0];

        ball0Position = new Vector3(ball0Pos.X, ball0Pos.Y, ball0Pos.Z);
    }
}
