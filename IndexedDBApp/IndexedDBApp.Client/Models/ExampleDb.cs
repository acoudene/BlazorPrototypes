using Blazor.IndexedDB;
using Microsoft.JSInterop;

namespace IndexedDBApp.Client.Models;

public class ExampleDb : IndexedDb
{
  public ExampleDb(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }
  public IndexedSet<Person> People { get; set; }
}
