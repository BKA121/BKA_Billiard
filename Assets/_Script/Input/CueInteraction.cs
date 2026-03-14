using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueInteraction 
{
    private float currentAngle; // Goc xoay theo phuong ngang
    private float cameraPitch; // Goc do doc cua camera player theo phuong thang dung

    public float CurrentAngle => currentAngle;
    public float CameraPitch => cameraPitch;

    // Tinh toan goc xoay theo phuong ngang
    public void CaculateAngle(float mouseInputX, float sensitivity)
    {
        currentAngle += mouseInputX * sensitivity;
    }

    public void CaculateCameraPitch(float mouseInputY, float minPitch, float maxPitch, float sensitivity)
    {
        cameraPitch += mouseInputY * sensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
    }

    public Vector3 GetPositionAroundBall0(Vector3 ball0, float radius, float offsetFromBall0)
    {
        float totalOffset = radius + offsetFromBall0;
        Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
        return ball0 - (rotation * Vector3.forward * totalOffset);
    }
}
