using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueInteraction 
{
    public float CurrentAngle { get; private set; }
    public float CameraPitch { get; private set; }
    public float CurrentPull { get; private set; }

    // Ham tinh toan xoay gay de ngam ban
    public void CaculateAngle(float mouseInputX, float sensitivity)
    {
        CurrentAngle += mouseInputX * sensitivity;
    }

    public void CaculateCameraPitch(float mouseInputY, float minPitch, float maxPitch, float sensitivity)
    {
        CameraPitch += mouseInputY * sensitivity;
        CameraPitch = Mathf.Clamp(CameraPitch, minPitch, maxPitch);
    }

    public Vector3 GetPositionAroundBall0(Vector3 ball0)
    {
        Quaternion rotation = Quaternion.Euler(-7f, CurrentAngle, 0);
        return ball0 + (rotation * Vector3.forward * 0);
    }

    // Ham tinh toan khi nhap cue
    public void CalculatePull(float mouseInputY, float sensitivity, float maxPull)
    {
        CurrentPull -= mouseInputY * sensitivity;
        CurrentPull = Mathf.Clamp(CurrentPull, -0.01f, maxPull); // -0.01f la khi cham bi trang
    }

    public void ResetPull()
    {
        CurrentPull = 0;
    }
}
