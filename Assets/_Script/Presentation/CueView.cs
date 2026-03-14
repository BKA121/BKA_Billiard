using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueView : MonoBehaviour
{
    public Transform ball0Transform;

    private float mouseXInput = 0f, mouseYInput = 0f;
    private CueInteraction cueInteraction = new CueInteraction();
    [SerializeField] private float sensitivity = 2.0f;
    [SerializeField] private Transform cameraPlayer;
    [SerializeField] private PlayerInputController playerInputController;
    [SerializeField] private BallPhysicConfig ballPhysicConfig;
    [SerializeField] private CuePhysicConfig cuePhysicConfig;
    [SerializeField] private CameraConfig cameraConfig;

    private void Update()
    {
        UpdatePositionCueSystem();
    }

    public void UpdatePositionCueSystem()
    {
        if (ball0Transform == null) return;

        mouseXInput = playerInputController.GetHorizontalAxis();
        mouseYInput = playerInputController.GetVerticalAxis();

        cueInteraction.CaculateAngle(mouseXInput, sensitivity);
        cueInteraction.CaculateCameraPitch(mouseYInput, cameraConfig.minPitch, cameraConfig.maxPitch, sensitivity);

        transform.rotation = Quaternion.Euler(0, cueInteraction.CurrentAngle, 0);
        
        transform.position = cueInteraction.GetPositionAroundBall0(ball0Transform.position, 
                             ballPhysicConfig.radius, cuePhysicConfig.offsetFromBall0);

        if (cameraPlayer != null)
        {
            cameraPlayer.rotation = Quaternion.Euler(cueInteraction.CameraPitch, cueInteraction.CurrentAngle, 0);
        }
    }
}
