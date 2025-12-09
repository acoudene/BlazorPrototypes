using FileManagementOpenApi.Dtos;
using FileManagementOpenApi.Models;
using System.Collections.Concurrent;

namespace FileManagementOpenApi.Services;

public class InMemoryFileStorageService : IFileStorageService
{
  private readonly ConcurrentDictionary<Guid, (FileMetadataDto Metadata, byte[] Content)> _storage = new();

  public Task<FileMetadataDto> SaveFileAsync(IFormFile file, string? description)
  {
    var id = Guid.NewGuid();

    using var memoryStream = new MemoryStream();
    file.CopyTo(memoryStream);
    var content = memoryStream.ToArray();

    var metadata = new FileMetadataDto
    {
      Id = id,
      FileName = file.FileName,
      ContentType = file.ContentType,
      SizeInBytes = file.Length,
      Description = description,
      UploadDate = DateTime.UtcNow
    };

    _storage[id] = (metadata, content);

    return Task.FromResult(metadata);
  }

  public Task<FileMetadataDto> SaveBase64FileAsync(Base64FileDto fileDto)
  {
    var id = Guid.NewGuid();
    var content = Convert.FromBase64String(fileDto.Base64Data);

    var metadata = new FileMetadataDto
    {
      Id = id,
      FileName = fileDto.FileName,
      ContentType = fileDto.ContentType,
      SizeInBytes = content.Length,
      Description = fileDto.Description,
      UploadDate = DateTime.UtcNow
    };

    _storage[id] = (metadata, content);

    return Task.FromResult(metadata);
  }

  public Task<FileData?> GetFileAsync(Guid id)
  {
    if (!_storage.TryGetValue(id, out var stored))
      return Task.FromResult<FileData?>(null);

    var fileData = new FileData
    {
      Content = stored.Content,
      FileName = stored.Metadata.FileName,
      ContentType = stored.Metadata.ContentType
    };

    return Task.FromResult<FileData?>(fileData);
  }

  public Task<List<FileMetadataDto>> GetAllFilesAsync()
  {
    var files = _storage.Values.Select(x => x.Metadata).OrderByDescending(x => x.UploadDate).ToList();
    return Task.FromResult(files);
  }

  public Task<bool> DeleteFileAsync(Guid id)
  {
    return Task.FromResult(_storage.TryRemove(id, out _));
  }
}
