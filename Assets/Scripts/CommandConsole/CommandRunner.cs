using System.Collections.Generic;
using UnityEngine;

public class CommandRunner : MonoBehaviour
{
    [SerializeField] private List<Command> commands;
    private string arguments;

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
        float boxHeight = 130;
        float xPosition = 10;
        float bottomMargin = 100;
        float yPosition = Screen.height - boxHeight - bottomMargin;

        GUI.Box(new Rect(xPosition, yPosition, boxWidth, boxHeight), "");

        float logHeight = 100;
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
        string[] args = arguments.Split(' ');

        if (args[0] == "h" || args[0] == "help")
            HandleHelp();
        else
        {

            foreach (Command command in commands)
            {
                if (command != null)
                {
                    foreach (string alias in command.Aliases)
                    {
                        if (alias == args[0].ToLower())
                            if (args.Length > 1)
                                command.Execute(args, logs);
                            else
                                command.Execute(logs);
                    }
                }
            }
        }
        arguments = "";
        ScrollToBottom();
    }

    private void HandleHelp()
    {
        foreach(Command command in commands)
            logs.Add($"Type '{command.Aliases[0]}' to {command.Description}");

        ScrollToBottom();
    }
    private void ScrollToBottom()
    {
        scroll.y = Mathf.Infinity;
    }
}
