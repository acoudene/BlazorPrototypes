using TodoCommandPatternBlazorApp.Client.CommandPattern.Receivers;

namespace TodoCommandPatternBlazorApp.Client.CommandPattern.Commands;

public class RemoveItemCommand : ICommand
{
  private readonly TodoListReceiver _todoList;
  private readonly string _item;

  public RemoveItemCommand(TodoListReceiver todoList, string item)
  {
    _todoList = todoList;
    _item = item;
  }

  public void Execute()
  {
    _todoList.RemoveItem(_item);
  }

  public void Undo()
  {
    _todoList.AddItem(_item);
  }
}

