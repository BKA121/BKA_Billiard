using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueInteraction 
{
    public float CurrentAngle { get; private set; }
    public float CameraPitch { get; private set; }

    public void CaculateAngle(float mouseInputX, float sensitivity)
    {
        CurrentAngle += mouseInputX * sensitivity;
    }

    public void CaculateCameraPitch(float mouseInputY, float minPitch, float maxPitch, float sensitivity)
    {
        CameraPitch += mouseInputY * sensitivity;
        CameraPitch = Mathf.Clamp(CameraPitch, minPitch, maxPitch);
    }

    public Vector3 GetPositionAroundBall0(Vector3 ball0, float radius, float offsetFromBall0)
    {
        float totalOffset = radius + offsetFromBall0;
        Quaternion rotation = Quaternion.Euler(0, CurrentAngle, 0);
        return ball0 - (rotation * Vector3.forward * totalOffset);
    }
}
