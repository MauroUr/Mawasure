using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Puzzle Toggler")]
public class PuzzleToggler : Command
{
    public override void Execute(List<string> logs)
    {
        FireCauldron[] fireCauldrons = FindObjectsOfType<FireCauldron>();

        if (fireCauldrons[0].IsPuzzleOn())
        {
            fireCauldrons[0].AdvancePuzzle(false);
            fireCauldrons[1].AdvancePuzzle(false);
            logs.Add("Puzzle finished.");
        }
        else
        {
            fireCauldrons[0].RestartPuzzle();
            fireCauldrons[1].RestartPuzzle();
            logs.Add("Puzzle restarted.");
        }
    }

    public override void Execute(string[] args, List<string> logs)
    {
        logs.Add("This command should not receive arguments");
    }

}

