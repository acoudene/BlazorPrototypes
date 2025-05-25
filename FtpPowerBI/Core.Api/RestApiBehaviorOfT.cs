// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Data;
using Core.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.Api;

/// <summary>
/// Component used to interact with data layer as a CRUD
/// </summary>
/// <typeparam name="TDto"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TRepository"></typeparam>
public class RestApiBehavior<TDto, TEntity, TRepository>
  where TDto : class, IIdentifierDto
  where TEntity : class, IIdentifierEntity
  where TRepository : IRepository<TEntity>
{
  private readonly TRepository _repository;

  public TRepository Repository { get => _repository; }

  public RestApiBehavior(TRepository repository)
  {
    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
  }

  public virtual async Task<List<TDto>> GetAllAsync(Func<TEntity, TDto> toDtoFunc, CancellationToken cancellationToken = default)
  {
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));

    var entities = await _repository.GetAllAsync(cancellationToken);

    return entities
      .Select(entity => toDtoFunc(entity))
      .ToList();
  }

  public virtual async Task<TDto?> GetByIdAsync(Guid id, Func<TEntity, TDto> toDtoFunc, CancellationToken cancellationToken = default)
  {
    if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));

    var entity = await _repository.GetByIdAsync(id, cancellationToken);
    if (entity is null)
      return null;

    return toDtoFunc(entity);
  }

  public virtual async Task<List<TDto>> GetByIdsAsync(List<Guid> ids, Func<TEntity, TDto> toDtoFunc, CancellationToken cancellationToken = default)
  {
    if (ids is null) throw new ArgumentNullException(nameof(ids));
    if (!ids.Any()) throw new ArgumentOutOfRangeException(nameof(ids));
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));

    var entities = await _repository.GetByIdsAsync(ids, cancellationToken);

    return entities
      .Select(entity => toDtoFunc(entity))
      .ToList();
  }

  public virtual async Task<TDto> CreateAsync(TDto newDto, Func<TDto, TEntity> toEntityFunc, CancellationToken cancellationToken = default)
  {
    if (newDto is null) throw new ArgumentNullException(nameof(newDto));
    if (newDto.Id == Guid.Empty) throw new ArgumentNullException(nameof(newDto.Id));
    if (toEntityFunc is null) throw new ArgumentNullException(nameof(toEntityFunc));

    var toCreateEntity = toEntityFunc(newDto);

    await _repository.CreateAsync(toCreateEntity, cancellationToken);

    return newDto; // Don't read the inserted value because the Dto should be read and checked in the API.
  }

  public virtual async Task<TDto?> UpdateAsync(Guid id, TDto updatedDto, Func<TDto, TEntity> toEntityFunc, CancellationToken cancellationToken = default)
  {
    if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
    if (id != updatedDto.Id) throw new ArgumentOutOfRangeException(nameof(updatedDto.Id));
    if (toEntityFunc is null) throw new ArgumentNullException(nameof(toEntityFunc));

    var existingEntity = await _repository.GetByIdAsync(id, cancellationToken);
    if (existingEntity is null)
      return null;

    var toUpdateEntity = toEntityFunc(updatedDto);

    if (existingEntity.Equals(toUpdateEntity))
      return updatedDto;

    await _repository.UpdateAsync(toUpdateEntity, cancellationToken);

    return updatedDto; // Don't read the updated value because the Dto should be read and checked in the API.
  }

  public virtual async Task<TDto?> DeleteAsync(Guid id, Func<TEntity, TDto> toDtoFunc, CancellationToken cancellationToken = default)
  {
    if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));

    var beforeRemoveEntity = await _repository.GetByIdAsync(id, cancellationToken);
    if (beforeRemoveEntity is null)
      return null;

    await _repository.RemoveAsync(id, cancellationToken);

    return toDtoFunc(beforeRemoveEntity); // Don't read the inserted value because the Dto should be read and checked in the API.
  }

  public virtual async Task<TDto?> PatchAsync(
    Guid id,
    JsonPatchDocument<TDto> patchDto,
    ModelStateDictionary modelState,
    Func<TDto, TEntity> toEntityFunc,
    Func<TEntity, TDto> toDtoFunc,
    CancellationToken cancellationToken = default)
  {
    if (patchDto is null) throw new ArgumentNullException(nameof(patchDto));
    if (modelState is null) throw new ArgumentNullException(nameof(modelState));
    if (toEntityFunc is null) throw new ArgumentNullException(nameof(toEntityFunc));
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));

    var existingEntity = await _repository.GetByIdAsync(id, cancellationToken);
    if (existingEntity is null)
      return null;

    var toUpdateDto = toDtoFunc(existingEntity);
    patchDto.ApplyTo(toUpdateDto, modelState);

    if (!modelState.IsValid)
      throw new ArgumentOutOfRangeException(nameof(modelState));

    var toUpdateEntity = toEntityFunc(toUpdateDto);
    await _repository.UpdateAsync(toUpdateEntity, cancellationToken);

    return toUpdateDto; // Don't read the inserted value because the Dto should be read and checked in the API.
  }
}
