using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInteraction : MonoBehaviour
{
    private MatchManager _matchManager;
    private bool _canPlaceBall;
    [SerializeField] private float _moveSensitivity = 0.1f;

    public bool isMModeActive = false;

    public PlayerInputController playerInputController;
    public CueView cueView;
    public Transform camTransform;

    public void Initialized(MatchManager matchManager)
    {
        this._matchManager = matchManager;

        _matchManager.OnBallInHandStarted += EnablePlacement;

        _matchManager.OnBallInHandFinished += DisablePlacement;
    }
    
    void Update()
    {
        if (!_canPlaceBall) return;

        if (cueView.currentState != cueView.OverViewState) return;

        if (playerInputController.IsBallInHandAction()) 
        {
            isMModeActive = !isMModeActive;

            Cursor.lockState = isMModeActive ? CursorLockMode.Locked : CursorLockMode.None;
        }

        if (isMModeActive)
        {
            HandleMModeMovement();
        }
    }

    private void EnablePlacement(bool isBreakShot)
    {
        _canPlaceBall = true;
    }

    private void DisablePlacement(bool isBreakShot)
    {
        _canPlaceBall = false;
    }

    public void HandleMModeMovement()
    {
        float mouseX = playerInputController.GetHorizontalAxis() * _moveSensitivity;
        float mouseY = playerInputController.GetVerticalAxis() * _moveSensitivity;

        Vector3 camRight = camTransform.right;
        Vector3 camForward = camTransform.forward;

        camRight.y = 0;
        camForward.y = 0;

        camRight.Normalize();
        camForward.Normalize();

        PhysicVector3 moveDirectionRight = new PhysicVector3(camRight.x, 0, camRight.z);
        PhysicVector3 moveDirectionForward = new PhysicVector3(camForward.x, 0, camForward.z);

        PhysicVector3 movement = (moveDirectionRight * mouseX) + (moveDirectionForward * mouseY);

        PhysicVector3 currentPos = _matchManager.gameState.ballState.Positions[0];
        currentPos.X += movement.X;
        currentPos.Z += movement.Z;

        _matchManager._awaitingState.UpdateCueBallPosition(currentPos);
    }
}
