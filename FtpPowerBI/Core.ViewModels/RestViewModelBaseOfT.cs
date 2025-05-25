// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels.BffProxying;
using Core.ViewObjects;

namespace Core.ViewModels;

public abstract class RestViewModelBase<TViewObject, TRestBffClient> : IViewModel<TViewObject>
    where TViewObject : class, IIdentifierViewObject
    where TRestBffClient : IRestBffClient<TViewObject>
{
  private readonly RestViewModelBehavior<TViewObject, TRestBffClient> _behavior;

  protected RestViewModelBase(RestViewModelBehavior<TViewObject, TRestBffClient> behavior)
    => _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));

  public virtual async Task CreateAsync(TViewObject newItem, CancellationToken cancellationToken = default)
    => await _behavior.CreateAsync(newItem, cancellationToken);

  public virtual async Task CreateOrUpdateAsync(TViewObject newOrToUpdateVo, CancellationToken cancellationToken = default)
    => await _behavior.CreateOrUpdateAsync(newOrToUpdateVo, cancellationToken);

  public virtual async Task<List<TViewObject>> GetAllAsync(CancellationToken cancellationToken = default)
    => await _behavior.GetAllAsync(cancellationToken);

  public virtual async Task<TViewObject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    => await _behavior.GetByIdAsync(id, cancellationToken);

  public virtual async Task<List<TViewObject>?> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    => await _behavior.GetByIdsAsync(ids, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    => await _behavior.RemoveAsync(id, cancellationToken);

  public virtual async Task UpdateAsync(Guid id, TViewObject updatedItem, CancellationToken cancellationToken = default)
    => await _behavior.UpdateAsync(id, updatedItem, cancellationToken);

}
