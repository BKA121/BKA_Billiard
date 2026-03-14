using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "ScriptableObject/CameraConfig")]
public class CameraConfig : ScriptableObject
{
    // Goc min va max khi di chuyen camera theo phuong thang dung
    public float minPitch = -40f;
    public float maxPitch = 0f;
}