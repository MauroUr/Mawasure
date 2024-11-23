using System.Collections.Generic;
using System.IO;

public interface ICommand
{
    public void Execute(List<string> logs);

    public void Execute(string[] args, List<string> logs);

    public List<string> Aliases { get; }
    public string Description { get; }
}
