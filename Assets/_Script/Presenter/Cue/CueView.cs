using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueView : MonoBehaviour
{
    private CueInteraction _cueInteraction = new CueInteraction();
    [SerializeField] private float _sensitivity = 0.3f;
    private ICueState _currentState;

    public CueAimState AimState { get; private set; }
    public CuePowerState PowerState { get; private set; }
    public Transform cueModel;
    public Transform ball0Transform;
    public PlayerInputController playerInputController;
    public BallPhysicConfig ballPhysicConfig;
    public CuePhysicConfig cuePhysicConfig;
    public CameraConfig cameraConfig;
    public Transform cameraPlayer;
    public float Sensitivity => _sensitivity;

    private void Start()
    {
        AimState = new CueAimState(this, _cueInteraction);
        PowerState = new CuePowerState(this, _cueInteraction);

        ChangeState(AimState);
    }

    private void Update()
    {
        if (ball0Transform == null || _currentState == null) return;

        _currentState.HandleInput();
        _currentState.UpdateView();
    }

    public void ChangeState(ICueState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

}
