using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class Command : ScriptableObject, ICommand
{
    [field: SerializeField] public List<string> Aliases { get; set; }

    public abstract void Execute(List<string> logs);

    public abstract void Execute(string[] args, List<string> logs);

    [field: SerializeField] public string Description { get; set; }
}
