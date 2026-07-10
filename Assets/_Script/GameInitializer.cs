using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private MatchManager _matchManager;
    private GameState _gameState;

    public TableSize tableConfig;
    public CueView cueView;
    public BallPhysicConfig ballConfig;
    public BallParent ballParent;
    public CommandDispatcher commandDispatcher;
    public GameController gameController;
    public MatchPresenter matchPresenter;
    public BallInteraction ballInteraction;

    // Khoi tao lop du lieu vat ly thuan C# cho core su dung
    private PhysicData CreatePhysicData()
    {
        PhysicVector3 headSpot = new PhysicVector3(tableConfig.HeadSpot.x, tableConfig.HeadSpot.y, tableConfig.HeadSpot.z);
        PhysicVector3 footSpot = new PhysicVector3(tableConfig.FootSpot.x, tableConfig.FootSpot.y, tableConfig.FootSpot.z);

        PocketDataPhysicVector3[] pockets = new PocketDataPhysicVector3[tableConfig.pockets.Length];
        for (int i = 0; i < tableConfig.pockets.Length; i++)
        {
            var p = tableConfig.pockets[i];

            pockets[i] = new PocketDataPhysicVector3
            {
                upA = new PhysicVector3(p.upA.x, p.upA.y, p.upA.z),
                upB = new PhysicVector3(p.upB.x, p.upB.y, p.upB.z),
                downA = new PhysicVector3(p.downA.x, p.downA.y, p.downA.z),
                downB = new PhysicVector3(p.downB.x, p.downB.y, p.downB.z),
                center = new PhysicVector3(p.center.x, p.center.y, p.center.z),
                rPocket = p.rPocket
            };
        }

        return new PhysicData(
            tableConfig.length, tableConfig.width, tableConfig.WallBounce, tableConfig.offsetPocketCorner, tableConfig.offsetPocketCenter,
            ballConfig.radius, ballConfig.mass, ballConfig.restitution,
            headSpot,
            footSpot,
            pockets
        );
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        PhysicData pureData = CreatePhysicData();
        _gameState = new GameState(pureData);

        MatchConfig currentMatchConfig;

        if (GameManager.Instance != null && GameManager.Instance.MatchConfigToLoad != null)
        {
            currentMatchConfig = GameManager.Instance.MatchConfigToLoad;
        }
        else
        {
            currentMatchConfig = MatchConfig.CreatePvPMatch(102, 1, "Test 1", PlayerType.Local, 2, "Test 2", PlayerType.Local, 40f);
        }

        _matchManager = new MatchManager(_gameState, currentMatchConfig);

        matchPresenter.Initialize(_matchManager);
        ballInteraction.Initialized(_matchManager);
    }

    private void Start()
    {
        _matchManager.StartMatch();
        ballParent.Initialize(_gameState.ballState);
        cueView.Initialize(_gameState.ballState);
        commandDispatcher.Initialize(_matchManager);
        gameController.Initialized(_matchManager);
    }
}
