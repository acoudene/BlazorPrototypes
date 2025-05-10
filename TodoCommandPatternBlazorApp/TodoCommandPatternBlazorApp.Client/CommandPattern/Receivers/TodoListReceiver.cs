namespace TodoCommandPatternBlazorApp.Client.CommandPattern.Receivers;

public class TodoListReceiver
{
  private readonly List<string> _items = new List<string>();

  public void AddItem(string item)
  {
    _items.Add(item);
    Console.WriteLine($"Item '{item}' added to the to-do list.");
  }

  public void RemoveItem(string item)
  {
    _items.Remove(item);
    Console.WriteLine($"Item '{item}' removed from the to-do list.");
  }

  public void ShowItems()
  {
    Console.WriteLine("Current to-do list: " + string.Join(", ", _items));
  }
}

