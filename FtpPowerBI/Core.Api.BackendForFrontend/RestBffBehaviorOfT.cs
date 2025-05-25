// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Core.Proxying;
using Core.ViewObjects;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.Api.BackendForFrontend;

/// <summary>
/// Component used to manage proxy interaction with a backend API
/// </summary>
/// <typeparam name="TViewObject"></typeparam>
/// <typeparam name="TDto"></typeparam>
/// <typeparam name="TClient"></typeparam>
public class RestBffBehavior<TViewObject, TDto, TClient>
  where TViewObject : class, IIdentifierViewObject
  where TDto : class, IIdentifierDto
  where TClient : IRestClient<TDto>
{
  private readonly TClient _client;

  public TClient Client { get => _client; }

  public RestBffBehavior(TClient client)
  {
    _client = client ?? throw new ArgumentNullException(nameof(client));
  }

  public virtual async Task<List<TViewObject>> GetAllAsync(
    Func<TDto, TViewObject> toVoFunc, 
    CancellationToken cancellationToken = default)
  {
    if (toVoFunc is null) throw new ArgumentNullException(nameof(toVoFunc));

    var dtos = await _client.GetAllAsync(cancellationToken);

    return dtos
      .Select(dto => toVoFunc(dto))
      .ToList();
  }

  public virtual async Task<TViewObject?> GetByIdAsync(Guid id, Func<TDto, TViewObject> toVoFunc, CancellationToken cancellationToken = default)
  {
    if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
    if (toVoFunc is null) throw new ArgumentNullException(nameof(toVoFunc));

    var entity = await _client.GetByIdAsync(id, cancellationToken);
    if (entity is null)
      return null;

    return toVoFunc(entity);
  }

  public virtual async Task<List<TViewObject>> GetByIdsAsync(List<Guid> ids, Func<TDto, TViewObject> toVoFunc, CancellationToken cancellationToken = default)
  {
    if (ids is null) throw new ArgumentNullException(nameof(ids));
    if (!ids.Any()) throw new ArgumentOutOfRangeException(nameof(ids));
    if (toVoFunc is null) throw new ArgumentNullException(nameof(toVoFunc));

    var entities = await _client.GetByIdsAsync(ids, cancellationToken);

    return entities
      .Select(entity => toVoFunc(entity))
      .ToList();
  }

  public virtual async Task<TViewObject> CreateAsync(TViewObject newDto, Func<TViewObject, TDto> toDtoFunc, CancellationToken cancellationToken = default)
  {
    if (newDto is null) throw new ArgumentNullException(nameof(newDto));
    if (newDto.Id == Guid.Empty) throw new ArgumentNullException(nameof(newDto.Id));
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));

    var toCreateEntity = toDtoFunc(newDto);

    await _client.CreateAsync(toCreateEntity, cancellationToken);

    return newDto; // Don't read the inserted value because the Dto should be read and checked in the API.
  }

  public virtual async Task<TViewObject?> UpdateAsync(Guid id, TViewObject updatedDto, Func<TViewObject, TDto> toDtoFunc, CancellationToken cancellationToken = default)
  {
    if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
    if (id != updatedDto.Id) throw new ArgumentOutOfRangeException(nameof(updatedDto.Id));
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));

    var existingEntity = await _client.GetByIdAsync(id, cancellationToken);
    if (existingEntity is null)
      return null;

    var toUpdateEntity = toDtoFunc(updatedDto);
    await _client.UpdateAsync(id, toUpdateEntity, cancellationToken);

    return updatedDto; // Don't read the updated value because the Dto should be read and checked in the API.
  }

  public virtual async Task<TViewObject?> DeleteAsync(Guid id, Func<TDto, TViewObject> toVoFunc, CancellationToken cancellationToken = default)
  {
    if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
    if (toVoFunc is null) throw new ArgumentNullException(nameof(toVoFunc));

    var beforeRemoveEntity = await _client.GetByIdAsync(id, cancellationToken);
    if (beforeRemoveEntity is null)
      return null;

    await _client.DeleteAsync(id, cancellationToken);

    return toVoFunc(beforeRemoveEntity); // Don't read the inserted value because the Dto should be read and checked in the API.
  }

  public virtual async Task<TViewObject?> PatchAsync(
    Guid id,
    JsonPatchDocument<TViewObject> patchDto,
    ModelStateDictionary modelState,
    Func<TViewObject, TDto> toDtoFunc,
    Func<TDto, TViewObject> toVoFunc,
    CancellationToken cancellationToken = default)
  {
    if (patchDto is null) throw new ArgumentNullException(nameof(patchDto));
    if (modelState is null) throw new ArgumentNullException(nameof(modelState));
    if (toDtoFunc is null) throw new ArgumentNullException(nameof(toDtoFunc));
    if (toVoFunc is null) throw new ArgumentNullException(nameof(toVoFunc));

    var existingDto = await _client.GetByIdAsync(id, cancellationToken);
    if (existingDto is null)
      return null;

    var toUpdateDto = toVoFunc(existingDto);
    patchDto.ApplyTo(toUpdateDto, modelState);

    if (!modelState.IsValid)
      throw new ArgumentOutOfRangeException(nameof(modelState));

    var toUpdateEntity = toDtoFunc(toUpdateDto);
    await _client.UpdateAsync(id, toUpdateEntity, cancellationToken);

    return toUpdateDto; // Don't read the inserted value because the Dto should be read and checked in the API.
  }
}
