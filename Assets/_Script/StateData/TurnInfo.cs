using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInfo 
{
    public int currentPlayer;
    public bool hasBallInHand;
    public float timeLimit;

    public bool isBreakShot;
    public bool isDeviceBallGroup;
    public bool isGameOver; 

    public FoulType lastFoulType; 
    public string notifyMessage;   
}
