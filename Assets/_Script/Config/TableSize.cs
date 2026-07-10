using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
public struct PocketData
{
    // Moi lo co 4 diem, A la diem nhon mom lo, B la diem trong lo, AB tao thanh vecto mat cheo mom lo
    public Vector3 upA, upB;
    public Vector3 downA, downB;
    public Vector3 center;
    public float rPocket;
}

[CreateAssetMenu(fileName = "TableSize", menuName = "ScriptableObject/TableSize")]
public class TableSize : ScriptableObject
{
    public float length = 2.5428f;         // Chieu dai table
    public float width = 1.2424f;          // Chieu rong table

    public float WallBounce = 0.8f;        // He so nay bang cua ban

    // Vi tri duong line pha bi, co do dai z: [-width/2; width/2]
    public Vector3 HeadSpot => new Vector3(0, 0, -0.6521f);
    // Vi tri duong line xep bi
    public Vector3 FootSpot => new Vector3(0, 0, 0.6521f);


    // Khoang cach tam lo toi thanh bang
    public float offsetPocketCorner = 0.0702f;
    public float offsetPocketCenter = 0.0897f;
    public PocketData[] pockets = new PocketData[6];

}
