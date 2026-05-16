using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDispatcher : MonoBehaviour
{
    public MatchManager coreManager;
    public static CommandDispatcher Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(MatchManager coreManager)
    {
        this.coreManager = coreManager;
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
    }
}
