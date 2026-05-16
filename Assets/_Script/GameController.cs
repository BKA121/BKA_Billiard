using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MatchManager matchManager;
    public float dt, fixedt;

    private void Start()
    {
        dt = Time.deltaTime;
        fixedt = Time.fixedDeltaTime;
    }

    public void Initialized(MatchManager matchManager)
    {
        this.matchManager = matchManager;
    }

    void FixedUpdate()
    {
        if (matchManager == null) return;

        matchManager.FixedUpdate(fixedt);
    }

    void Update()
    {
        if (matchManager == null) return;

        matchManager.Update(dt);
    }
}
