// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace Core.Proxying;

public abstract class HttpRestClientBase<TDto> : IRestClient<TDto>
  where TDto : class, IIdentifierDto
{
  private readonly ILogger<HttpRestClientBase<TDto>> _logger;
  protected ILogger<HttpRestClientBase<TDto>> Logger => _logger;

  private readonly HttpRestClientBehavior<TDto> _behavior;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="logger"></param>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  protected HttpRestClientBase(
    ILogger<HttpRestClientBase<TDto>> logger, 
    IHttpClientFactory httpClientFactory)
    : this(logger, new HttpRestClientBehavior<TDto>(httpClientFactory))
  {
  }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="logger"></param>
  /// <param name="behavior"></param>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  protected HttpRestClientBase(
    ILogger<HttpRestClientBase<TDto>> logger, 
    HttpRestClientBehavior<TDto> behavior)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
  }

  public abstract string GetConfigurationName();

  public virtual async Task<List<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}...", nameof(GetAllAsync));
    return await _behavior.GetAllAsync(GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id})...", nameof(GetByIdAsync), id);
    return await _behavior.GetByIdAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<List<TDto>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Ids})...", nameof(GetByIdsAsync), string.Join(',', ids));
    return await _behavior.GetByIdsAsync(ids, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task CreateAsync(
      TDto dto,
      CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Dto})...", nameof(CreateAsync), dto);

        await _behavior.CreateAsync(dto, GetConfigurationName(), true, cancellationToken);
  }

  public virtual async Task CreateOrUpdateAsync(
      TDto newOrToUpdateDto,
      CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Dto})...", nameof(CreateOrUpdateAsync), newOrToUpdateDto);

        await _behavior.CreateOrUpdateAsync(newOrToUpdateDto, GetConfigurationName(), true, cancellationToken);
  }

  public virtual async Task UpdateAsync(
    Guid id,
    TDto dto,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id},{Dto})...", nameof(UpdateAsync), id, dto);

        await _behavior.UpdateAsync(id, dto, GetConfigurationName(), true, cancellationToken);
  }

  public virtual async Task<TDto?> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id})...", nameof(DeleteAsync), id);
    return await _behavior.DeleteAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task PatchAsync(
    Guid id,
    JsonPatchDocument<TDto> patch,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id},{Patch})...", nameof(PatchAsync), id, patch);

        await _behavior.PatchAsync(id, patch, GetConfigurationName(), true, cancellationToken);
  }
}


