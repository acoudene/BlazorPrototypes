namespace CameraCaptureApp.Models;

public record PhotoCapture
{
    public required Guid Id { get; init; }    
    public required DateTimeOffset Timestamp { get; init; }
    public required string DataUri { get; init; }
}
