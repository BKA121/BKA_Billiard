using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueInteraction 
{
    private float _lastPull;

    public float CurrentAngle { get; private set; }
    public float CameraPitch { get; private set; } // Goc di chuyen cua camera player
    public float CurrentPull { get; private set; } // Do doi khi keo gay
    public float CurrentStrikeSpeed { get; private set; } // Van toc danh bi trong 1 frame

    public float HeightSecondCam = 0.7f; // Do cao second cam

    public Vector2 CurrentSpinPoint { get; private set; } 

    // Ham tinh toan xoay gay de ngam ban
    public void CaculateAngle(float mouseInputX, float sensitivity)
    {
        CurrentAngle += mouseInputX * Time.deltaTime * sensitivity;
    }

    public void CaculateCameraPitch(float mouseInputY, float minPitch, float maxPitch, float sensitivity)
    {
        CameraPitch += mouseInputY * Time.deltaTime * sensitivity;
        CameraPitch = Mathf.Clamp(CameraPitch, minPitch, maxPitch);
    }

    public Vector3 GetPositionAroundBall0(Vector3 ball0)
    {
        Quaternion rotation = Quaternion.Euler(0, CurrentAngle, 0);
        return ball0 + (rotation * Vector3.back * 0);
    }

    // Ham tinh toan khi nhap cue
    public void CalculatePull(float mouseInputY, float sensitivity, float maxPull)
    {
        _lastPull = CurrentPull;

        CurrentPull -= mouseInputY * Time.deltaTime * sensitivity;
        CurrentPull = Mathf.Clamp(CurrentPull, -0.01f, maxPull); // -0.01f la khi cham bi trang

        float frameVelocity = (_lastPull - CurrentPull) / Time.deltaTime;
        CurrentStrikeSpeed = Mathf.Lerp(CurrentStrikeSpeed, Mathf.Max(0, frameVelocity), 0.5f);
    }

    // Tinh do cao second camera
    public void CaculateHeightSecondCam(float mouseInputY, float minHeight, float maxHeight, float heightSpeed)
    {
        HeightSecondCam += mouseInputY * heightSpeed * Time.deltaTime;
        HeightSecondCam = Mathf.Clamp(HeightSecondCam, minHeight, maxHeight);
    }

    public void ResetPull()
    {
        CurrentPull = 0;
    }

    public void ResetSpin()
    {
        CurrentSpinPoint = Vector2.zero;
    }

    public void CalculateSpinPoint(float mouseX, float mouseY, float sensitivity, float ballRadius)
    {
        CurrentSpinPoint += new Vector2(mouseX, mouseY) * sensitivity * Time.deltaTime;

        CurrentSpinPoint = Vector2.ClampMagnitude(CurrentSpinPoint, ballRadius-0.01f);
    }
}
