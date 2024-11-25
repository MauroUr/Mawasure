using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandRunner : MonoBehaviour
{
    [SerializeField] private List<Command> commands;
    private string arguments = "";

    [SerializeField] private Player player;
    public Player Player => player;

    private List<string> logs = new List<string>();
    private bool showConsole = false;
    private Vector2 scroll;

    private void OnEnable()
    {
        player.OnCheatPressed += OnToggleDebug;
        player.OnEnterPressed += HandleInput;
    }

    private void OnDisable()
    {
        player.OnCheatPressed -= OnToggleDebug;
        player.OnEnterPressed -= HandleInput;
    }

    public void OnToggleDebug()
    {
        showConsole = !showConsole;
    }
    private void OnGUI()
    {
        if (!showConsole) { return; }

        float boxWidth = (Screen.width / 10) * 4;
        float boxHeight = 200;
        float xPosition = 10;
        float bottomMargin = Screen.height/6;
        float yPosition = Screen.height - boxHeight - bottomMargin;

        GUI.Box(new Rect(xPosition, yPosition, boxWidth, boxHeight), "");

        float logHeight = boxHeight - 30;
        Rect viewport = new Rect(0, 0, boxWidth - 30, 20 * logs.Count);
        scroll = GUI.BeginScrollView(new Rect(xPosition, yPosition + 5f, boxWidth, logHeight), scroll, viewport);

        for (int i = 0; i < logs.Count; i++)
        {
            Rect labelRect = new Rect(5, 20 * i, viewport.width - 10, 20);
            GUI.Label(labelRect, logs[i]);
        }
        GUI.EndScrollView();

        float inputHeight = 30;
        float inputY = yPosition + logHeight + 5f;
        GUI.backgroundColor = Color.white;
        arguments = GUI.TextField(new Rect(xPosition + 10f, inputY, boxWidth - 20f, inputHeight - 5f), arguments);
    }

    private void HandleInput()
    {
        string[] args = arguments.ToLower().Split(' ');
        bool commandExecuted = false;

        if (string.IsNullOrEmpty(args[0]))
        {
            logs.Add("Please type a valid command, or type 'help' to read all commands.");
            commandExecuted = true;
        }
        else if (args[0] == "h" || args[0] == "help")
        {
            HandleHelp();
            commandExecuted = true;
        }
        else if (args[0] == "aliases")
        {
            commandExecuted = true;
            if (args.Length == 1 || string.IsNullOrEmpty(args[1]))
                logs.Add("Please specify which command you wish to know aliases of.");
            else
                HandleAliases(args[1]);
        }
        else
            foreach (Command command in commands)
                foreach (string alias in command.Aliases)
                    if (alias == args[0])
                    {
                        commandExecuted = true;
                        if (args.Length > 1)
                            command.Execute(args, logs);
                        else
                            command.Execute(logs);
                    }
        
        if (!commandExecuted)
            logs.Add("Please type a valid command, or type 'help' to read all commands.");
        
        arguments = "";
        ScrollToBottom();
    }

    private void HandleAliases(string name)
    {
        foreach (Command command in commands)
            if (name == command.Aliases[0]) 
            {
                string[] aliases = new string[command.Aliases.Count];
                command.Aliases.CopyTo(aliases);
                aliases[0] = "";
                string aliasesString = string.Join(" or ", aliases.Where(alias => !string.IsNullOrEmpty(alias)));
                logs.Add($"Command '{name}' can also be called as {aliasesString} ");
                return;
            }
        logs.Add("There is no command with that name.");
    }

    private void HandleHelp()
    {
        foreach(Command command in commands)
            logs.Add($"Type '{command.Aliases[0]}' to {command.Description}");

        logs.Add("Type 'aliases' and the name of a command to show all his aliases.");
        ScrollToBottom();
    }
    private void ScrollToBottom()
    {
        scroll.y = Mathf.Infinity;
    }
}
