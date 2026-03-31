using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CoreManager coreManager;
    [SerializeField] private float dt = 0.02f;

    public void Initialized(CoreManager coreManager)
    {
        this.coreManager = coreManager;
    }

    void Update()
    {
        if (coreManager.isCaculateShoot)
        {
            coreManager.CaculateShoot(dt);
        }
    }
}
