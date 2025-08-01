namespace MyBlazorApp.Client.Dtos;

public record MyEntityDto
{
  public required Guid Id { get; set; }
  public required string Name { get; set; }
}
