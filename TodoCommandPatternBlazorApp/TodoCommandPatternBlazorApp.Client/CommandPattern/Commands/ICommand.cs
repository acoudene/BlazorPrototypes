namespace TodoCommandPatternBlazorApp.Client.CommandPattern.Commands;

public interface ICommand
{
  void Execute();
  void Undo();
}

