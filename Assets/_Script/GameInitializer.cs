using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private CoreManager _coreManager;
    private GameState _gameState;

    public TableSize tableConfig;
    public CueView cueView;
    public BallPhysicConfig ballConfig;
    public BallParent ballParent;
    public CommandDispatcher commandDispatcher;
    public GameController gameController;

    // Khoi tao lop du lieu vat ly thuan C# cho core su dung
    private PhysicData CreatePhysicData()
    {
        PhysicVector3 headSpot = new PhysicVector3(tableConfig.HeadSpot.x, tableConfig.HeadSpot.y, tableConfig.HeadSpot.z);
        PhysicVector3 footSpot = new PhysicVector3(tableConfig.FootSpot.x, tableConfig.FootSpot.y, tableConfig.FootSpot.z);

        PhysicVector3[] pockets = new PhysicVector3[tableConfig.GetPocketCenters.Length];
        for (int i = 0; i < pockets.Length; i++)
        {
            var p = tableConfig.GetPocketCenters[i];
            pockets[i] = new PhysicVector3(p.x, p.y, p.z);
        }

        return new PhysicData(
            tableConfig.length, tableConfig.width, tableConfig.Mr, tableConfig.WallBounce,
            ballConfig.radius, ballConfig.mass,
            headSpot,
            footSpot,
            pockets
        );
    }

    private void Awake()
    {
        PhysicData pureData = CreatePhysicData();
        _gameState = new GameState(pureData);
        _coreManager = new CoreManager(_gameState);
        ballParent.Initialize(_gameState.ballState);
        cueView.Initialize(_gameState.ballState);
        commandDispatcher.Initialize(_coreManager);
        gameController.Initialized(_coreManager);
    }
}
