using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Seconds Logger")]
public class SecondsLogger : Command
{
    public override void Execute(List<string> logs)
    {
        logs.Add("Time since started playing: " + Time.realtimeSinceStartup.ToString() + " seconds.");
    }

    public override void Execute(string[] args, List<string> logs)
    {
        logs.Add("This command should not receive arguments");
    }

}

