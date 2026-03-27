using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDispatcher : MonoBehaviour
{
    public static CommandDispatcher Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
    }
}
