using Blazor.IndexedDB;
using Microsoft.JSInterop;

namespace IndexedDBApp.Client.Models;

public class MyDatabaseDb : IndexedDb
{
  public MyDatabaseDb(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }
  public IndexedSet<Person>? People { get; set; }
  public IndexedSet<FileStorage>? Files { get; set; }
}
