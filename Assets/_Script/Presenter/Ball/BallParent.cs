using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallParent : MonoBehaviour
{
    private BallView[] _ballViews;
    private BallState _ballState;
    [SerializeField] private GameObject[] ballPrefabs;
    public GameController gameController;

    public void Initialize(BallState state)
    {
        _ballState = state;
        _ballViews = new BallView[_ballState.TotalBalls];

        for (int i = 0; i < _ballState.TotalBalls; i++)
        {
            PhysicVector3 corePos = _ballState.Positions[i];
            Vector3 spawnPos = new Vector3(corePos.X, corePos.Y, corePos.Z);

            GameObject ballGO = Instantiate(ballPrefabs[i], spawnPos, Quaternion.identity, this.transform);

            BallView view = ballGO.GetComponent<BallView>();
            if (view != null)
            {
                _ballViews[i] = view;
            }
        }
    }

    private void Update()
    {
        if (!gameController.coreManager.isCaculateShoot) return;

        if (_ballState == null || _ballViews == null) return;

        for (int i = 0; i < _ballViews.Length; i++)
        {
            PhysicVector3 corePos = _ballState.Positions[i];
            bool active = _ballState.IsActive[i];

            Vector3 unityPos = new Vector3(corePos.X, corePos.Y, corePos.Z);

            _ballViews[i].Render(unityPos, active);
        }
    }
}