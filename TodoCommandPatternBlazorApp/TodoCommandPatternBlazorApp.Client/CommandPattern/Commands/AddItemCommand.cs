using TodoCommandPatternBlazorApp.Client.CommandPattern.Receivers;

namespace TodoCommandPatternBlazorApp.Client.CommandPattern.Commands;

public class AddItemCommand : ICommand
{
  private readonly TodoListReceiver _todoList;
  private readonly string _item;

  public AddItemCommand(TodoListReceiver todoList, string item)
  {
    _todoList = todoList;
    _item = item;
  }

  public void Execute()
  {
    _todoList.AddItem(_item);
  }

  public void Undo()
  {
    _todoList.RemoveItem(_item);
  }
}
