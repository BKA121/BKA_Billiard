using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CuePhysicConfig", menuName = "ScriptableObject/CuePhysicConfig")]
public class CuePhysicConfig : ScriptableObject
{
    public float offsetFromBall0 = 0.01f; // Do lech cua gay so voi bi trang

    // Goc min va max khi di chuyen gay theo phuong thang dung
    public float minPitch = -40f;
    public float maxPitch = 0f;
}
