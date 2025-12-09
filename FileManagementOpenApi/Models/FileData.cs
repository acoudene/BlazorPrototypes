namespace FileManagementOpenApi.Models;

public class FileData
{
  public byte[] Content { get; set; } = Array.Empty<byte>();
  public string FileName { get; set; } = string.Empty;
  public string ContentType { get; set; } = string.Empty;
}
