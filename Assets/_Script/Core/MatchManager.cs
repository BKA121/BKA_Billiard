using System;
using System.Collections.Generic;

public enum MatchStateEnum { Initializing, Awaiting, Simulating, RuleChecking, Notifying, GameOver }

public class MatchManager 
{
    private IMatchState _currentState;

    public InitializingState _initializingState;
    public AwaitingState _awaitingState;
    public SimulatingState _simulatingState;
    public RuleCheckingState _ruleCheckingState;
    public NotifyingState _notifyingState;
    public FinishState _finishState;

    public MatchStateEnum CurrentStateEnum { get; private set; }
    public PhysicSystem physicSystem;
    public GameState gameState;
    public MatchConfig matchConfig;
    public static MatchManager Instance { get; private set; }

    public MatchManager(GameState gameState, MatchConfig matchConfig)
    {
        Instance = this;

        this.gameState = gameState;
        this.matchConfig = matchConfig;
        physicSystem = new PhysicSystem(this.gameState.physicData, this.gameState.ballState, this.gameState.shotResult);

        _initializingState = new InitializingState(this);
        _awaitingState = new AwaitingState(this);
        _simulatingState = new SimulatingState(this);
        _ruleCheckingState = new RuleCheckingState(this);
        _notifyingState = new NotifyingState(this);
        _finishState = new FinishState(this);
    }

    public void StartMatch()
    {
        ChangeState(_initializingState, MatchStateEnum.Initializing);
    }

    public void ChangeState(IMatchState newState, MatchStateEnum newStateEnum)
    {
        _currentState?.Exit();
        _currentState = newState;
        CurrentStateEnum = newStateEnum;
        _currentState.Enter();
    }

    public void ExecuteShoot(PhysicVector3 direction, float force, PhysicVector2 spinPoint)
    {
        gameState.currentShotDirection = direction;
        gameState.currentShotForce = force;
        gameState.currentSpinPoint = spinPoint;

        ChangeState(_simulatingState, MatchStateEnum.Simulating);
    }

    public void ExecutePlayerQuit()
    {
        gameState.currentTurnInfo.lastFoulType = FoulType.Quit;
        ChangeState(_ruleCheckingState, MatchStateEnum.RuleChecking);
    }

    public void FixedUpdate(float fixedt)
    {
        _currentState.FixedUpdate(fixedt);
    }

    public void Update(float dt)
    {
        _currentState.Update(dt);
    }

    public Action<TurnInfo> OnNotifyInMatch;

    public Action<bool> OnBallInHandStarted;

    public Action<bool> OnBallInHandFinished;

    public Action<int> OnTimerUpdated;

    public Action<List<PlayerInfo>> OnNotifyFinishMatch;

    public Action OnReplayMatch;

    public Action<List<PlayerInfo>> OnShowScoreBar;

    public Action<TurnInfo> OnShowTurn;

    public Action<int> OnShowRaceText;

    public Action<int> OnChangeColorBallPocketed;
}
