namespace MyBlazor10.Client.WorkerServices;

public record StampInfo
{
  public required DateTimeOffset UpdatedAt { get; set; }
  public required string Identifier { get; set; }
}