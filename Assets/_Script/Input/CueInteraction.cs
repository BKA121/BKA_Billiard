using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueInteraction 
{
    private float currentAngle;

    public float CurrentAngle => currentAngle;

    // Tinh toan goc xoay theo phuong ngang
    public void CaculateAngle(float mouseInputX, float sensitivity)
    {
        currentAngle += mouseInputX * sensitivity;
    }

    public Vector3 GetPositionAroundBall0(Vector3 ball0, float radius, float offsetFromBall0)
    {
        float offset = radius + offsetFromBall0;
        Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
        return ball0 - (rotation * Vector3.forward * offset);
    }
}
