using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "ScriptableObject/CameraConfig")]
public class CameraConfig : ScriptableObject
{
    // Goc min va max khi di chuyen first camera theo phuong thang dung
    public float minPitch = -40f;
    public float maxPitch = 0f;

    // Do cao second camera theo phuong thang dung
    public float minHeight = 0.1f;
    public float maxHeight = 0.7f;
    public float heightSpeed = 2f;

    // Do nhay camera
    public float sensitivityFirstCam = 5f;
    public float sensitivitySecondCam = 7f;
}