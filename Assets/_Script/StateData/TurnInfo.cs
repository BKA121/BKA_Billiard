using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInfo 
{
    public int activePlayerId;
    public string namePlayer;
    public bool hasBallInHand;
    public float timeLimit;

    public bool isBreakShot;
    public bool isGameOver;
    public int winnerId = -1; 

    public FoulType lastFoulType; 
    public string notifyMessage;   
}
