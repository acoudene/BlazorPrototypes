// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MyFeature.Proxies;
using MyFeature.ViewObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using MyFeature.Proxies.Ftp;

namespace MyFeature.Api.BackendForFrontend;

/// <summary>
/// API to interact with views through ViewObjects
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MyEntityBffController : ControllerBase
{
  private readonly ILogger<MyEntityBffController> _logger;
  private readonly IMyEntityClient _client;
  private readonly IFtpProxyClient _ftpProxyClient;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="logger"></param>
  /// <param name="client"></param>
  /// <param name="ftpProxyClient"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public MyEntityBffController(
    ILogger<MyEntityBffController> logger, 
    IMyEntityClient client,
    IFtpProxyClient ftpProxyClient)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _client = client ?? throw new ArgumentNullException(nameof(client));
    _ftpProxyClient = ftpProxyClient ?? throw new ArgumentNullException(nameof(ftpProxyClient));
  }

  /// <summary>
  /// Get all ViewObjects
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  [HttpGet]
  [Produces(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public virtual async Task<ActionResult<List<MyEntityVo>>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      return (await _client.GetAllAsync(cancellationToken))
        .Select(dto => dto.ToViewObject())
        .ToList();
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return Problem();
    }
  }

  /// <summary>
  /// Get a ViewObject from its id
  /// </summary>
  /// <param name="id"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  [HttpGet("{id:guid}")]
  [Produces(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public virtual async Task<ActionResult<MyEntityVo?>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    try
    {
      return (await _client.GetByIdAsync(id, cancellationToken))?
        .ToViewObject();
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return Problem();
    }
  }

  /// <summary>
  /// Create if needed and update an item through ViewObject
  /// </summary>
  /// <param name="newOrToUpdateVo"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="InvalidOperationException"></exception>
  [HttpPost("CreateOrUpdate")]
  [Consumes(MediaTypeNames.Application.Json)]
  [Produces(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public virtual async Task<ActionResult<MyEntityVo>> CreateOrUpdateAsync(
     [FromBody] MyEntityVo newOrToUpdateVo,
     CancellationToken cancellationToken = default)
  {
    try
    {
      if (newOrToUpdateVo is null)
        throw new ArgumentNullException(nameof(newOrToUpdateVo));

      var dto = newOrToUpdateVo.ToDto();
      if (dto is null)
        throw new InvalidOperationException("Problem while converting to view object");

      await _client.CreateOrUpdateAsync(dto, cancellationToken);
      return dto.ToViewObject();
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return Problem();
    }
  }

  /// <summary>
  /// Delete an item from its id
  /// </summary>
  /// <param name="id"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  [HttpDelete("{id:guid}")]
  [Produces(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public virtual async Task<ActionResult<MyEntityVo>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
  {
    try
    {
      if (id == Guid.Empty)
        throw new ArgumentException(nameof(id));

      var dto = await _client.DeleteAsync(id, cancellationToken);
      if (dto is null)
        throw new InvalidOperationException("Problem while deleting view object");

      return dto.ToViewObject();
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return Problem();
    }
  }

  [HttpPost("Export")]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public virtual async Task<ActionResult> ExportAsync(
     [FromBody] List<MyEntityVo> toExportVos,
     CancellationToken cancellationToken = default)
  {
    try
    {
      if (toExportVos is null || !toExportVos.Any())
        return NoContent();
      
      var dtos = toExportVos
        .Select(vo => vo.ToDto())
        .ToList();
      await _ftpProxyClient.ExportAsync(dtos, cancellationToken);
      return NoContent();
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return BadRequest();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return Problem();
    }

  }
}
