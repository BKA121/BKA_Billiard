using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICueState
{
    void Enter();
    void HandleInput();   
    void UpdateView();
    void Exit();
}
