using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Player HP Setter")]
public class PlayerHPSetter : Command
{
    public override void Execute(List<string> logs)
    {
        logs.Add("This command requires at least 1 argument.");
    }

    public override void Execute(string[] args, List<string> logs)
    {
        int number;
        if (args.Length < 2)
        {
            logs.Add("Invalid arguments. Usage: healthpoints <value>");
            return;
        }
        else if (!int.TryParse(args[1], out number) || (number > 100 || number < 1))
        {
            logs.Add("Incorrect value, must be an integer between 1 and 100.");
            return;

        }

        CommandRunner runner = FindObjectOfType<CommandRunner>();
        Player player = runner?.Player;

        if (player == null)
        {
            logs.Add("Player not found.");
            return;
        }

        player.SetHealth(number);
        logs.Add($"Set health to {number}.");
    }
}

