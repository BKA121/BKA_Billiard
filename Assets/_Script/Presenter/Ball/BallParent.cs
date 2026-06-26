using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallParent : MonoBehaviour
{
    private BallView[] _ballViews;
    private Transform[] _shadowTransforms;
    private BallState _ballState;
    [SerializeField] private GameObject[] ballPrefabs;
    [SerializeField] private GameObject shadowPrefab;
    private float shadowPosY = -0.0329f;

    public GameController gameController;
    public GameObject BallShadows;

    private void Start()
    {
        MatchManager.Instance.OnReplayMatch += RenderBallsForReplayMatch;
    }

    public void Initialize(BallState state)
    {
        _ballState = state;
        _ballViews = new BallView[_ballState.TotalBalls];
        _shadowTransforms = new Transform[_ballState.TotalBalls];

        for (int i = 0; i < _ballState.TotalBalls; i++)
        {
            PhysicVector3 corePos = _ballState.Positions[i];
            Vector3 spawnPos = new Vector3(corePos.X, corePos.Y, corePos.Z);

            GameObject ballGO = Instantiate(ballPrefabs[i], spawnPos, Quaternion.identity, this.transform);
            GameObject ballShadow = Instantiate(shadowPrefab, new Vector3(corePos.X, shadowPosY, corePos.Z), 
                Quaternion.Euler(90f, 0f, 0f), BallShadows.transform);

            _shadowTransforms[i] = ballShadow.transform;

            BallView view = ballGO.GetComponent<BallView>();
            if (view != null)
            {
                _ballViews[i] = view;
            }
        }
    }

    public void RenderBallsForReplayMatch()
    {
        if (_ballState == null || _ballViews == null) return;

        for (int i = 0; i < _ballViews.Length; i++)
        {
            PhysicVector3 corePos = _ballState.Positions[i];
            bool active = _ballState.IsActive[i];
            Vector3 unityPos = new Vector3(corePos.X, corePos.Y, corePos.Z);
            PhysicQuaternion coreQuat = _ballState.Rotations[i];
            Quaternion unityQuat = new Quaternion(coreQuat.X, coreQuat.Y, coreQuat.Z, coreQuat.W);

            _ballViews[i].Render(unityPos, unityQuat, active);
            _shadowTransforms[i].gameObject.SetActive(true);
            _shadowTransforms[i].position = new Vector3(corePos.X, shadowPosY, corePos.Z);
            _shadowTransforms[i].rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }

    private void LateUpdate()
    {
        if (_ballState == null || _ballViews == null) return;

        if (gameController.matchManager.CurrentStateEnum != MatchStateEnum.Simulating && 
            !gameController.matchManager._awaitingState.HasBallInHand) return;

        for (int i = 0; i < _ballViews.Length; i++)
        {
            if (gameController.matchManager._awaitingState.HasBallInHand && i != 0) continue;

            if (_ballState.IsDropping[i])
            {
                _shadowTransforms[i].gameObject.SetActive(false);
            }

            if (!_ballState.IsActive[i])
            {
                _ballViews[i].DeactivateBall();
                continue;
            }

            PhysicVector3 corePos = _ballState.Positions[i];
            bool active = _ballState.IsActive[i];

            Vector3 unityPos = new Vector3(corePos.X, corePos.Y, corePos.Z);

            PhysicQuaternion coreQuat = _ballState.Rotations[i];
            Quaternion unityQuat = new Quaternion(coreQuat.X, coreQuat.Y, coreQuat.Z, coreQuat.W);

            _ballViews[i].Render(unityPos, unityQuat, active);
            _shadowTransforms[i].gameObject.SetActive(true);
            _shadowTransforms[i].position = new Vector3(corePos.X, shadowPosY, corePos.Z);
            _shadowTransforms[i].rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}