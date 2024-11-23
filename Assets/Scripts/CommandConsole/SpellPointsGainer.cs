using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Spell Points Gainer")]
public class SpellPointsGainer : Command
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
            logs.Add("Invalid arguments. Usage: points <value>");
            return;
        }
        else if (!int.TryParse(args[1], out number) || (number >= 100 || number < 1))
        {
            logs.Add("Incorrect value, must be an integer between 1 and 99.");
            return;
        }

        WindowsManager windowsManager = FindObjectOfType<WindowsManager>();

        windowsManager.SetSpellPoints(number.ToString());

        logs.Add($"Set Spell Points to {number}.");
    }

}

