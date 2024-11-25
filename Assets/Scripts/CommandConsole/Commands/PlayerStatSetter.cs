using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Player Stat Setter")]
public class PlayerStatSetter : Command
{
    public override void Execute(List<string> logs)
    {
        logs.Add("This command requires at least 2 arguments.");
    }

    public override void Execute(string[] args, List<string> logs)
    {
        if (args.Length < 3)
        {
            logs.Add("Invalid arguments. Usage: <stat_name> <value>");
            return;
        }

        string statName = args[1].ToLower();
        if (!int.TryParse(args[2], out int value) || (value < 1 || value >= 100))
        {
            logs.Add("Incorrect value, must be an integer between 1 and 99.");
            return;
        }

        CommandRunner runner = FindObjectOfType<CommandRunner>();
        Player player = runner?.Player;

        if (player == null)
        {
            logs.Add("Player not found.");
            return;
        }

        Stats stats = player.stats;

        switch (statName)
        {
            case "strength": case "str": stats.strength = value; break;
            case "dexterity": case "dex": stats.dexterity = value; break;
            case "agility": case "agi":  stats.agility = value; break;
            case "intelligence": case "int":  stats.intelligence = value; break;
            case "vitality": case "vit": stats.vitality = value; break;
            case "luck": stats.luck = value; break;
            default:
                logs.Add($"Unknown stat: {statName}");
                return;
        }

        player.stats = stats;
        logs.Add($"Set {statName} to {value}.");
    }

}