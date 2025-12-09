using FileManagementOpenApi.Dtos;
using FileManagementOpenApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileManagementOpenApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class FilesController : ControllerBase
{
  private readonly IFileStorageService _fileStorage;
  private readonly ILogger<FilesController> _logger;

  public FilesController(IFileStorageService fileStorage, ILogger<FilesController> logger)
  {
    _fileStorage = fileStorage;
    _logger = logger;
  }

  /// <summary>
  /// Upload un fichier via multipart/form-data
  /// </summary>
  [HttpPost("upload")]
  [Consumes("multipart/form-data")]
  [ProducesResponseType(typeof(FileMetadataDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<FileMetadataDto>> UploadFile(IFormFile file, [FromForm] string? description)
  {
    if (file == null || file.Length == 0)
      return BadRequest("Aucun fichier fourni");

    try
    {
      var metadata = await _fileStorage.SaveFileAsync(file, description);
      return Ok(metadata);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erreur lors de l'upload du fichier");
      return BadRequest(ex.Message);
    }
  }

  /// <summary>
  /// Upload un fichier encodé en base64
  /// </summary>
  [HttpPost("upload-base64")]
  [Consumes("application/json")]
  [ProducesResponseType(typeof(FileMetadataDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<FileMetadataDto>> UploadFileBase64([FromBody] Base64FileDto fileDto)
  {
    if (string.IsNullOrEmpty(fileDto.Base64Data))
      return BadRequest("Données base64 manquantes");

    try
    {
      var metadata = await _fileStorage.SaveBase64FileAsync(fileDto);
      return Ok(metadata);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erreur lors de l'upload base64");
      return BadRequest(ex.Message);
    }
  }

  /// <summary>
  /// Télécharge un fichier par son ID
  /// </summary>
  [HttpGet("download/{id}")]
  [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> DownloadFile(Guid id)
  {
    var fileData = await _fileStorage.GetFileAsync(id);

    if (fileData == null)
      return NotFound("Fichier introuvable");

    return File(fileData.Content, fileData.ContentType, fileData.FileName);
  }

  /// <summary>
  /// Liste tous les fichiers
  /// </summary>
  [HttpGet]
  [ProducesResponseType(typeof(List<FileMetadataDto>), StatusCodes.Status200OK)]
  public async Task<ActionResult<List<FileMetadataDto>>> ListFiles()
  {
    var files = await _fileStorage.GetAllFilesAsync();
    return Ok(files);
  }

  /// <summary>
  /// Supprime un fichier
  /// </summary>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> DeleteFile(Guid id)
  {
    var success = await _fileStorage.DeleteFileAsync(id);

    if (!success)
      return NotFound("Fichier introuvable");

    return NoContent();
  }

}
