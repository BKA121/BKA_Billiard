using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueView : MonoBehaviour
{
    public Transform ball0Transform;

    private float mouseInput = 0f;
    private CueInteraction cueInteraction = new CueInteraction();
    [SerializeField] private float sensitivity = 2.0f;
    [SerializeField] private PlayerInputController playerInputController;
    [SerializeField] private BallPhysicConfig ballPhysicConfig;
    [SerializeField] private CuePhysicConfig cuePhysicConfig;

    private void Update()
    {
        UpdatePositionCue();
    }

    public void UpdatePositionCue()
    {
        if (ball0Transform == null) return;

        transform.position = ball0Transform.position;
        mouseInput = playerInputController.GetHorizontalAxis();
        cueInteraction.CaculateAngle(mouseInput, sensitivity);
        transform.rotation = Quaternion.Euler(0, cueInteraction.CurrentAngle, 0);
        transform.position = cueInteraction.GetPositionAroundBall0(ball0Transform.position, ballPhysicConfig.radius, cuePhysicConfig.offsetFromBall0);
    }
}
