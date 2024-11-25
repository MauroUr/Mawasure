using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Enemy ReSpawner")]
public class EnemyReSpawner : Command
{
    public override void Execute(List<string> logs)
    {
        Spawner spawner = FindObjectOfType<Spawner>();

        spawner.Spawn();

        logs.Add("Enemies spawned succesfully.");
    }

    public override void Execute(string[] args, List<string> logs)
    {
        logs.Add("This command should not receive arguments");
    }

}

