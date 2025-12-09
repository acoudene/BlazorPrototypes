using FileManagementOpenApi.Dtos;
using FileManagementOpenApi.Models;

namespace FileManagementOpenApi.Services;

public interface IFileStorageService
{
  Task<FileMetadataDto> SaveFileAsync(IFormFile file, string? description);
  Task<FileMetadataDto> SaveBase64FileAsync(Base64FileDto fileDto);
  Task<FileData?> GetFileAsync(Guid id);
  Task<List<FileMetadataDto>> GetAllFilesAsync();
  Task<bool> DeleteFileAsync(Guid id);
}
