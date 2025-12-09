namespace FileManagementOpenApi.Dtos;

public class FileMetadataDto
{
  public Guid Id { get; set; }
  public string FileName { get; set; } = string.Empty;
  public string ContentType { get; set; } = string.Empty;
  public long SizeInBytes { get; set; }
  public string? Description { get; set; }
  public DateTime UploadDate { get; set; }
  public string SizeFormatted => FormatBytes(SizeInBytes);

  private static string FormatBytes(long bytes)
  {
    string[] sizes = { "B", "KB", "MB", "GB" };
    double len = bytes;
    int order = 0;
    while (len >= 1024 && order < sizes.Length - 1)
    {
      order++;
      len /= 1024;
    }
    return $"{len:0.##} {sizes[order]}";
  }
}
