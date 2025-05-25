// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels.BffProxying;
using Core.ViewObjects;

namespace Core.ViewModels;

public class RestViewModelBehavior<TViewObject, TRestBffClient>
    where TViewObject : class, IIdentifierViewObject
    where TRestBffClient : IRestBffClient<TViewObject>
{
  private readonly TRestBffClient _restClient;
  protected TRestBffClient RestClient { get => _restClient; }

  public RestViewModelBehavior(TRestBffClient restClient) 
    => _restClient = restClient ?? throw new ArgumentNullException(nameof(restClient));

  public virtual async Task CreateAsync(TViewObject newItem, CancellationToken cancellationToken = default) 
    => await _restClient.CreateAsync(newItem, cancellationToken);

  public virtual async Task CreateOrUpdateAsync(TViewObject newOrToUpdateVo, CancellationToken cancellationToken = default)
    => await _restClient.CreateOrUpdateAsync(newOrToUpdateVo, cancellationToken);

  public virtual async Task<List<TViewObject>> GetAllAsync(CancellationToken cancellationToken = default) 
    => await _restClient.GetAllAsync(cancellationToken);

  public virtual async Task<TViewObject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) 
    => await _restClient.GetByIdAsync(id, cancellationToken);

  public virtual async Task<List<TViewObject>?> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default) 
    => await _restClient.GetByIdsAsync(ids, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default) 
    => await _restClient.DeleteAsync(id, cancellationToken);

  public virtual async Task UpdateAsync(Guid id, TViewObject updatedItem, CancellationToken cancellationToken = default) 
    => await _restClient.UpdateAsync(id, updatedItem, cancellationToken);

}
