using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MatchManager matchManager;

    public void Initialized(MatchManager matchManager)
    {
        this.matchManager = matchManager;
    }

    void FixedUpdate()
    {
        if (matchManager == null) return;

        matchManager.FixedUpdate(Time.fixedDeltaTime);
    }

    void Update()
    {
        if (matchManager == null) return;

        matchManager.Update(Time.deltaTime);
    }
}
