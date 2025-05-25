// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Data;
using Core.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Api;

/// <summary>
/// Base controller to expose REST Api
/// </summary>
/// <typeparam name="TDto"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TRepository"></typeparam>
public abstract class RestControllerBase<TDto, TEntity, TRepository> : ControllerBase
  where TDto : class, IIdentifierDto
  where TEntity : class, IIdentifierEntity
  where TRepository : IRepository<TEntity>
{
  private readonly IHostEnvironment _hostEnvironment;
  private readonly ILogger<RestControllerBase<TDto, TEntity, TRepository>> _logger;
  private readonly RestApiBehavior<TDto, TEntity, TRepository> _behavior;

  protected IHostEnvironment HostEnvironment => _hostEnvironment;
  protected ILogger<RestControllerBase<TDto, TEntity, TRepository>> Logger => _logger;
  protected RestApiBehavior<TDto, TEntity, TRepository> Behavior => _behavior;

  protected RestControllerBase(
    IHostEnvironment hostEnvironment,
    ILogger<RestControllerBase<TDto, TEntity, TRepository>> logger,
    RestApiBehavior<TDto, TEntity, TRepository> behavior)
  {
    _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
  }

  protected RestControllerBase(
    IHostEnvironment hostEnvironment, 
    ILogger<RestControllerBase<TDto, TEntity, TRepository>> logger, 
    TRepository repository)
    : this(hostEnvironment, logger, new RestApiBehavior<TDto, TEntity, TRepository>(repository))
  { }

  protected abstract TEntity ToEntity(TDto dto);
  protected abstract TDto ToDto(TEntity entity);

  [HttpGet]
  public virtual async Task<Results<Ok<List<TDto>>, BadRequest, ProblemHttpResult>> GetAllAsync(
    CancellationToken cancellationToken = default)
  {
    try
    {
      return TypedResults.Ok(await _behavior.GetAllAsync(ToDto, cancellationToken));
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpGet("{id:guid}")]
  public virtual async Task<Results<Ok<TDto>, NotFound, BadRequest, ProblemHttpResult>> GetByIdAsync(
    Guid id, 
    CancellationToken cancellationToken = default)
  {
    try
    {
      if (!ModelState.IsValid)
        throw new ArgumentException("ModelState is not validated or invalid");

      var foundEntity = await _behavior.GetByIdAsync(id, ToDto, cancellationToken);
      if (foundEntity is null)
        return TypedResults.NotFound();

      return TypedResults.Ok(foundEntity);
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpGet("byIds")]
  public virtual async Task<Results<Ok<List<TDto>>, BadRequest, ProblemHttpResult>> GetByIdsAsync(
    [FromQuery] List<Guid> ids, 
    CancellationToken cancellationToken = default)
  {
    try
    {
      if (!ModelState.IsValid)
        throw new ArgumentException("ModelState is not validated or invalid");

      return TypedResults.Ok(await _behavior.GetByIdsAsync(ids, ToDto, cancellationToken));
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpPost]
  public virtual async Task<Results<Created<TDto>, BadRequest, ProblemHttpResult>> CreateAsync(
    [FromBody] TDto newDto, 
    CancellationToken cancellationToken = default)
  {
    try
    {
      if (!ModelState.IsValid)
        throw new ArgumentException("ModelState is not validated or invalid");

      return TypedResults.Created("{newDto.Id}", await _behavior.CreateAsync(newDto, ToEntity, cancellationToken));
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpPost("CreateOrUpdate")]
  public virtual async Task<Results<NoContent, Created<TDto>, BadRequest, ProblemHttpResult>> CreateOrUpdateAsync(
      [FromBody] TDto newOrToUpdateDto, 
      CancellationToken cancellationToken = default)
  {
    try
    {
      if (!ModelState.IsValid)
        throw new ArgumentException("ModelState is not validated or invalid");

      if (newOrToUpdateDto is null)
        throw new ArgumentNullException(nameof(newOrToUpdateDto));

      Guid id = newOrToUpdateDto.Id;
      if (id == Guid.Empty)
        throw new ArgumentNullException(nameof(newOrToUpdateDto.Id));

      var updatedDto = await _behavior.UpdateAsync(id, newOrToUpdateDto, ToEntity, cancellationToken);
      if (updatedDto is not null)
        return TypedResults.NoContent();

      return TypedResults.Created("{newOrToUpdateDto.Id}", await _behavior.CreateAsync(newOrToUpdateDto, ToEntity, cancellationToken));
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpPut("{id:guid}")]
  public virtual async Task<Results<NoContent, NotFound, BadRequest, ProblemHttpResult>> UpdateAsync(
    Guid id, 
    [FromBody] TDto updatedDto, 
    CancellationToken cancellationToken = default)
  {
    try
    {
      if (!ModelState.IsValid)
        throw new ArgumentException("ModelState is not validated or invalid");

      var updatedEntity = await _behavior.UpdateAsync(id, updatedDto, ToEntity, cancellationToken);
      if (updatedEntity is null)
        return TypedResults.NotFound();

      return TypedResults.NoContent();
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpDelete("{id:guid}")]
  public virtual async Task<Results<Ok<TDto>, NotFound, BadRequest, ProblemHttpResult>> DeleteAsync(
    Guid id, 
    CancellationToken cancellationToken = default)
  {
    try
    {
      if (!ModelState.IsValid)
        throw new ArgumentException("ModelState is not validated or invalid");

      var deletedEntity = await _behavior.DeleteAsync(id, ToDto, cancellationToken);
      if (deletedEntity is null)
        return TypedResults.NotFound();

      return TypedResults.Ok(deletedEntity);
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }

  [HttpPatch]
  public virtual async Task<Results<Ok<TDto>, NotFound, BadRequest, ProblemHttpResult>> PatchAsync(
    Guid id, 
    [FromBody] JsonPatchDocument<TDto> patchDto,
    CancellationToken cancellationToken = default)
  {
    try
    {
      if (!ModelState.IsValid)
        throw new ArgumentException("ModelState is not validated or invalid");

      var patchedEntity = await _behavior.PatchAsync(id, patchDto, ModelState, ToEntity, ToDto, cancellationToken);
      if (patchedEntity is null)
        return TypedResults.NotFound();

      return TypedResults.Ok(patchedEntity);
    }
    catch (ArgumentException ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Bad request");
      throw;
    }
    catch (ArgumentException ex)
    {
      _logger.LogError(ex, "Bad request");
      return TypedResults.BadRequest();
    }
    catch (Exception ex) when (_hostEnvironment.IsDevelopment())
    {
      _logger.LogError(ex, "Internal error");
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal error");
      return TypedResults.Problem();
    }
  }
}
