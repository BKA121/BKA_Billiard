using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueInteraction 
{
    private float currentAngle; // Goc xoay theo phuong ngang
    private float currentPitch; // Goc do doc cua gay theo phuong thang dung

    public float CurrentAngle => currentAngle;
    public float CurrentPitch => currentPitch;

    // Tinh toan goc xoay theo phuong ngang
    public void CaculateAngle(float mouseInputX, float sensitivity)
    {
        currentAngle += mouseInputX * sensitivity;
    }

    public void CaculatePitch(float mouseInputY, float minPitch, float maxPitch, float sensitivity)
    {
        currentPitch += mouseInputY * sensitivity;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
    }

    public Vector3 GetPositionAroundBall0(Vector3 ball0, float radius, float offsetFromBall0)
    {
        float totalOffset = radius + offsetFromBall0;
        Quaternion rotation = Quaternion.Euler(currentPitch, currentAngle, 0);
        return ball0 - (rotation * Vector3.forward * totalOffset);
    }
}
