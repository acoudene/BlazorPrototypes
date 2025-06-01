// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MyFeature.ViewObjects;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace MyFeature.ViewModels.BffProxying;

public class HttpMyEntityRestBffClient : IMyEntityRestBffClient
{
  private readonly ILogger<HttpMyEntityRestBffClient> _logger;
  private readonly HttpMyEntityRestBffClientBehavior _behavior;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpMyEntityRestBffClient(ILogger<HttpMyEntityRestBffClient> logger, IHttpClientFactory httpClientFactory)
    : this(logger, new HttpMyEntityRestBffClientBehavior(httpClientFactory))
  {
  }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpMyEntityRestBffClient(ILogger<HttpMyEntityRestBffClient> logger, HttpMyEntityRestBffClientBehavior behavior)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
  }

  public const string ConfigurationName = nameof(HttpMyEntityRestBffClient);
  public virtual string GetConfigurationName() => ConfigurationName;

  public virtual async Task<List<MyEntityVo>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}...", nameof(GetAllAsync));
    return await _behavior.GetAllAsync(GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<MyEntityVo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id})...", nameof(GetByIdAsync), id);
    return await _behavior.GetByIdAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task<List<MyEntityVo>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Ids})...", nameof(GetByIdsAsync), string.Join(',', ids));
    return await _behavior.GetByIdsAsync(ids, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task CreateAsync(
      MyEntityVo vo,
      CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Vo})...", nameof(CreateAsync), vo);

    await _behavior.CreateAsync(vo, GetConfigurationName(), true, cancellationToken);
  }

  public virtual async Task CreateOrUpdateAsync(
      MyEntityVo vo,
      CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Vo})...", nameof(CreateOrUpdateAsync), vo);

    await _behavior.CreateOrUpdateAsync(vo, GetConfigurationName(), true, cancellationToken);
  }

  public virtual async Task UpdateAsync(
    Guid id,
    MyEntityVo vo,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id},{Vo})...", nameof(UpdateAsync), id, vo);

    await _behavior.UpdateAsync(id, vo, GetConfigurationName(), true, cancellationToken);
  }

  public virtual async Task<MyEntityVo?> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id})...", nameof(DeleteAsync), id);
    return await _behavior.DeleteAsync(id, GetConfigurationName(), cancellationToken);
  }

  public virtual async Task PatchAsync(
    Guid id,
    JsonPatchDocument<MyEntityVo> patch,
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Id},{Patch})...", nameof(PatchAsync), id, patch);

    await _behavior.PatchAsync(id, patch, GetConfigurationName(), true, cancellationToken);
  }

  public virtual async Task ExportAsync(
    List<MyEntityVo> toExportVos, 
    CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Processing call to {Method}({Vos})...", nameof(ExportAsync), toExportVos);

    await _behavior.ExportAsync(toExportVos, GetConfigurationName(), true, cancellationToken);
  }
}


