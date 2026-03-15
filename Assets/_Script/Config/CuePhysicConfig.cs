using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CuePhysicConfig", menuName = "ScriptableObject/CuePhysicConfig")]
public class CuePhysicConfig : ScriptableObject
{
    public float offsetFromBall0 = 0.01f; // Do lech cua gay so voi bi trang
    public float maxPull = 0.3f; // Do dai keo cue toi da
    public float sensitivityPull = 0.2f; // Do nhay keo cue
}
