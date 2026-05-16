using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotResult 
{
    public bool isTimeOut;
    public bool isBall0Pocketed;
    public bool isBall8Pocketed;
    public int firstBallHitID;                   // bi dau tien cham bi trang
    public List<int> pocketedBallIDs;            // danh sach bi roi
    public List<int> ballHitCushionAfterShot;   // luu cac bi cham bang sau cu pha, kiem tra cu pha hop le

    public ShotResult()
    {
        isTimeOut = false;
        isBall0Pocketed = false;
        isBall8Pocketed = false;
        firstBallHitID = -1;
        pocketedBallIDs = new List<int>();
        ballHitCushionAfterShot = new List<int>();
    }

    public void Reset()
    {
        isTimeOut = false;
        isBall0Pocketed = false;
        isBall8Pocketed = false;
        firstBallHitID = -1;
        pocketedBallIDs.Clear();
        ballHitCushionAfterShot.Clear();
    }
}
