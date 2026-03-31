using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TableSize", menuName = "ScriptableObject/TableSize")]
public class TableSize : ScriptableObject
{
    public float length = 2.5428f;         // Chieu dai table
    public float width = 1.2424f;          // Chieu rong table

    public float Mr = 0.99f;               // Sau moi frame thi giam di 0.01 do ma sat lan
    public float WallBounce = 0.8f;        // He so nay bang cua ban

    // Vi tri duong line pha bi, co do dai z: [-width/2 - width/2]
    public Vector3 HeadSpot => new Vector3(0, 0, -0.6521f);
    // Vi tri duong line xep bi
    public Vector3 FootSpot => new Vector3(0, 0, 0.6521f);


    // Ban kinh lo
    public float pocketOffset = 0.05f;
    // Dung tu vi tri pha bi
    public Vector3[] GetPocketCenters = new Vector3[] 
    {
    new Vector3(-0.6212f, 0, 1.2714f),  // Tren trai
    new Vector3(0.6212f, 0, 1.2714f),   // Tren phai
    new Vector3(-0.6212f, 0, -1.2714f),  // Duoi trai
    new Vector3(0.6212f, 0, -1.2714f),   // Duoi phai

    new Vector3(-0.6212f, 0, 0),        // Giua trai
    new Vector3(0.6212f, 0, 0)          // Giua phai
    };
}
