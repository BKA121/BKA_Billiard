using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDispatcher : MonoBehaviour
{
    public static CommandDispatcher dispatcher { get; private set; }

    [SerializeField] private CoreManager _coreManager;
    public CoreManager CoreManager => _coreManager;

    private void Awake()
    {
        dispatcher = this;
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
    }
}
