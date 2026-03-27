using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TableSize", menuName = "ScriptableObject/TableSize")]
public class TableSize : ScriptableObject
{
    public float length = 2.5428f; // Chieu dai table
    public float width = 1.2424f;  // Chieu rong table


    // Vi tri duong line pha bi, co do dai z: [-width/2 - width/2]
    public Vector3 HeadSpot => new Vector3(0.6521f, 0.033f, 0);
    // Vi tri duong line xep bi
    public Vector3 FootSpot => new Vector3(-0.6521f, 0.033f, 0);


    // Ban kinh lo
    public float pocketOffset = 0.05f;
    // Dung tu vi tri pha bi
    public Vector3[] GetPocketCenters = new Vector3[] 
    {
    new Vector3(-1.2714f, 0.033f, -0.6212f), // Tren trai
    new Vector3(-1.2714f, 0.033f, 0.6212f),  // Tren phai
    new Vector3(1.2714f, 0.033f, -0.6212f),  // Duoi trai
    new Vector3(1.2714f, 0.033f, 0.6212f),   // Duoi phai

    new Vector3(0, 0.033f, -0.6212f),        // Giua trai
    new Vector3(0, 0.033f, 0.6212f)          // Giua phai
    };
}
