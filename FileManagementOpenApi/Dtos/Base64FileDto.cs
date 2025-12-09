namespace FileManagementOpenApi.Dtos;

public class Base64FileDto
{
  public string FileName { get; set; } = string.Empty;
  public string ContentType { get; set; } = string.Empty;
  public string Base64Data { get; set; } = string.Empty;
  public string? Description { get; set; }
}
