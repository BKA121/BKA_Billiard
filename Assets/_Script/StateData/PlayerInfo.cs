using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { Local, AI, Remote }
public enum BallGroupType { None, Solid, Stripe }

public class PlayerInfo
{
    public int Id;
    public string name;
    public PlayerType type;
    public int score;
    public bool isWinner;
    public int remainingBalls;
    public BallGroupType targetGroup;
    public int foulCount;
    public bool canPlayEightBall => remainingBalls == 0 && targetGroup != BallGroupType.None;
}
