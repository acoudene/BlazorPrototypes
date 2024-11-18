namespace IndexedDBApp.Client.Models;

public record FileStorage
{
  [System.ComponentModel.DataAnnotations.Key]
  public long Id { get; set; }

  public List<byte[]> Files { get; set; } = Enumerable.Empty<byte[]>().ToList();
}
