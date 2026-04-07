using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallPhysicConfig", menuName = "ScriptableObject/BallPhysicConfig")]
public class BallPhysicConfig : ScriptableObject
{
    public float radius = 0.033f; // Ban kinh bi 
    public float mass = 0.17f;    // Khoi luong
    public float restitution = 0.98f;  // He so hoi phuc sau va cham voi bi khac
}
