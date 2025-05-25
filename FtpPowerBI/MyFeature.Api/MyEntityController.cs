// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyFeature.Api;

/// <summary>
/// Backend API interacting with proxy through DTOs
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MyEntityController : ControllerBase
{
  /// <remarks>
  /// If tests are done with Swagger for example, in case of using inheritance, don't forget to manually add $type to json definition of DTO parameter
  /// Example: on a POST call, you should add ("$type" must be at the first line of json properties!!!)
  /// {
  ///   "$type": "myEntity.myEntityInherited",
  ///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  /// }
  /// </remarks>
  /// 

  private readonly IHostEnvironment _hostEnvironment;
  private readonly ILogger<MyEntityController> _logger;

  protected RestApiBehavior<MyEntityDto, MyEntity, IMyEntityRepository> RestComponent { get => _restComponent; }
  private readonly RestApiBehavior<MyEntityDto, MyEntity, IMyEntityRepository> _restComponent;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="hostEnvironment"></param>
  /// <param name="logger"></param>
  /// <param name="repository"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public MyEntityController(IHostEnvironment hostEnvironment, ILogger<MyEntityController> logger, IMyEntityRepository repository)
  {
    _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _restComponent = new RestApiBehavior<MyEntityDto, MyEntity, IMyEntityRepository>(repository);
  }

  // This commented part could be used to have benefits of json entity typing
  //protected virtual MyEntityDtoBase ToDto(MyEntityBase entity)
  //  => entity.ToInheritedDto();

  // This commented part could be used to have benefits of json entity typing
  //protected virtual MyEntityBase ToEntity(MyEntityDtoBase dto)
  //  => dto.ToInheritedEntity();

  protected virtual MyEntityDto ToDto(MyEntity entity)
    => entity.ToDto();

  protected virtual MyEntity ToEntity(MyEntityDto dto)
    => dto.ToEntity();

  /// <summary>
  /// Get all dtos from backend
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  [HttpGet]
  public virtual async Task<Results<Ok<List<MyEntityDto>>, BadRequest, ProblemHttpResult>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      _logger.LogDebug("Receiving request for {Method}...", nameof(GetAllAsync));

      return TypedResults.Ok(await _restComponent.GetAllAsync(ToDto, cancellationToken));
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

  /// <summary>
  /// Get a dto from its id
  /// </summary>
  /// <param name="id"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  [HttpGet("{id:guid}")]
  public virtual async Task<Results<Ok<MyEntityDto>, NotFound, BadRequest, ProblemHttpResult>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    try
    {
      _logger.LogDebug("Receiving request for {Method}({Id})...", nameof(GetByIdAsync), id);

      var foundEntity = await _restComponent.GetByIdAsync(id, ToDto, cancellationToken);
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

  /// <summary>
  /// Get a list of dtos from a list of ids
  /// </summary>
  /// <param name="ids"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  [HttpGet("byIds")]
  public virtual async Task<Results<Ok<List<MyEntityDto>>, BadRequest, ProblemHttpResult>> GetByIdsAsync(
    [FromQuery] List<Guid> ids, CancellationToken cancellationToken = default)
  {
    try
    {
      _logger.LogDebug("Receiving request for {Method}({Ids})...", nameof(GetByIdsAsync), string.Join(',', ids));

      return TypedResults.Ok(await _restComponent.GetByIdsAsync(ids, ToDto, cancellationToken));
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

  /// <summary>
  /// Create an item from a dto
  /// </summary>
  /// <param name="newDto"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  [HttpPost]
  public virtual async Task<Results<Created<MyEntityDto>, BadRequest, ProblemHttpResult>> CreateAsync(
    [FromBody] MyEntityDto newDto, CancellationToken cancellationToken = default)
  {
    try
    {
      _logger.LogDebug("Receiving request for {Method}({Dto})...", nameof(CreateAsync), newDto);

      return TypedResults.Created("{newDto.Id}", await _restComponent.CreateAsync(newDto, ToEntity, cancellationToken));
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

  /// <summary>
  /// Create an item if needed or update it from a dto
  /// </summary>
  /// <param name="newOrToUpdateDto"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException"></exception>
  [HttpPost("CreateOrUpdate")]
  public virtual async Task<Results<NoContent, Created<MyEntityDto>, BadRequest, ProblemHttpResult>> CreateOrUpdateAsync(
      [FromBody] MyEntityDto newOrToUpdateDto, CancellationToken cancellationToken = default)
  {
    try
    {
      _logger.LogDebug("Receiving request for {Method}({Dto})...", nameof(CreateOrUpdateAsync), newOrToUpdateDto);
      if (newOrToUpdateDto is null)
        throw new ArgumentNullException(nameof(newOrToUpdateDto));

      Guid id = newOrToUpdateDto.Id;
      if (id == Guid.Empty)
        throw new ArgumentNullException(nameof(newOrToUpdateDto.Id));

      var updatedDto = await _restComponent.UpdateAsync(id, newOrToUpdateDto, ToEntity, cancellationToken);
      if (updatedDto is not null)
        return TypedResults.NoContent();

      return TypedResults.Created("{newOrToUpdateDto.Id}", await _restComponent.CreateAsync(newOrToUpdateDto, ToEntity, cancellationToken));
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

  /// <summary>
  /// Update an item from an id and a dto
  /// </summary>
  /// <param name="id"></param>
  /// <param name="updatedDto"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  [HttpPut("{id:guid}")]
  public virtual async Task<Results<NoContent, NotFound, BadRequest, ProblemHttpResult>> UpdateAsync(
    Guid id,
    [FromBody] MyEntityDto updatedDto,
    CancellationToken cancellationToken = default)
  {
    try
    {
      _logger.LogDebug("Receiving request for {Method}({Id},{Dto})...", nameof(UpdateAsync), id, updatedDto);

      var updatedEntity = await _restComponent.UpdateAsync(id, updatedDto, ToEntity, cancellationToken);
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

  /// <summary>
  /// Delete an item from an id
  /// </summary>
  /// <param name="id"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  [HttpDelete("{id:guid}")]
  public virtual async Task<Results<Ok<MyEntityDto>, NotFound, BadRequest, ProblemHttpResult>> DeleteAsync(
    Guid id, 
    CancellationToken cancellationToken = default)
  {
    try
    {
      _logger.LogDebug("Receiving request for {Method}({Id})...", nameof(DeleteAsync), id);

      var deletedEntity = await _restComponent.DeleteAsync(id, ToDto, cancellationToken);
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

  /// <summary>
  /// Do a partial update of an item through an id and patched dto 
  /// </summary>
  /// <param name="id"></param>
  /// <param name="patchDto"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  [HttpPatch("{id:guid}")]
  public virtual async Task<Results<Ok<MyEntityDto>, NotFound, BadRequest, ProblemHttpResult>> PatchAsync(
    Guid id,
    [FromBody] JsonPatchDocument<MyEntityDto> patchDto, 
    CancellationToken cancellationToken = default)
  {
    try
    {
      _logger.LogDebug("Receiving request for {Method}({Id},{Patch})...", nameof(PatchAsync), id, patchDto);

      var patchedEntity = await _restComponent.PatchAsync(id, patchDto, ModelState, ToEntity, ToDto, cancellationToken);
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
