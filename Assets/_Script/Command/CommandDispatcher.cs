using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDispatcher : MonoBehaviour
{
    public CoreManager coreManager;
    public static CommandDispatcher Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(CoreManager coreManager)
    {
        this.coreManager = coreManager;
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
    }
}
