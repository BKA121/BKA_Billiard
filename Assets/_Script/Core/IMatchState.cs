using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMatchState 
{
    void Enter();
    void Update(float dt);
    void FixedUpdate(float fixedt);
    void Exit();

}
