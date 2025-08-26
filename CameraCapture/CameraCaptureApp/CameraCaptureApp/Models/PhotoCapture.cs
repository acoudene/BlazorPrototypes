namespace CameraCaptureApp.Models;

public record PhotoCapture
{
    public required string Id { get; init; }    
    public required DateTimeOffset Timestamp { get; init; }
    public required string ContentType { get; init; }
    public required long Size { get; init; }
    public required string Url { get; init; }
}
