namespace IndexedDBApp.Client.Models;

public record Person
{
  [System.ComponentModel.DataAnnotations.Key]
  public long Id { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
}
