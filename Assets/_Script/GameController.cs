using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CoreManager coreManager;
    private float _dt;

    public void Initialized(CoreManager coreManager)
    {
        this.coreManager = coreManager;
    }

    private void Start()
    {
        _dt = Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        if (coreManager.isCaculateShoot)
        {
            coreManager.CaculateShoot(_dt);
        }
    }
}
