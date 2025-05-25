// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels.Offline;
using MyFeature.ViewModels.BffProxying;
using MyFeature.ViewObjects;

namespace MyFeature.ViewModels.Offline;

/// <summary>
/// ViewModel associated to a dedicated entity
/// </summary>
public class OfflineMyEntityViewModel : IMyEntityViewModel
{
  private readonly ICachingStorage _cachingStorage;
  private readonly MyEntityRestViewModelBehavior _behavior;

  public OfflineMyEntityViewModel(ICachingStorage cachingStorage, IMyEntityRestBffClient client)
  {
    _cachingStorage = cachingStorage ?? throw new ArgumentNullException(nameof(cachingStorage));

    ArgumentNullException.ThrowIfNull(client);

    _behavior = new MyEntityRestViewModelBehavior(client);
    Items = Enumerable.Empty<MyEntityVo>().ToList();
    SelectedItems = Enumerable.Empty<MyEntityVo>().ToHashSet();
  }

  public IEnumerable<MyEntityVo> Items { get; set; }
  public HashSet<MyEntityVo> SelectedItems { get; set; }
  public MyEntityVo? SelectedItem { get; set; }

  protected virtual string GetCacheKey(MyEntityVo item)
  {
    ArgumentNullException.ThrowIfNull(item);

    Guid id = item.Id;
    if (id == Guid.Empty)
      throw new InvalidOperationException($"{nameof(id)} is empty!");

    return id.ToString();
  }

  public virtual async Task CreateAsync(MyEntityVo newItem, CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(newItem);

    string cacheKey = GetCacheKey(newItem);
    var container = new ViewObjectStateContainer<MyEntityVo>(newItem, ViewObjectState.Added);
    
    

    await _behavior.CreateAsync(newItem, cancellationToken);
  }

  public virtual async Task CreateOrUpdateAsync(MyEntityVo newOrToUpdateVo, CancellationToken cancellationToken = default)
    => await _behavior.CreateOrUpdateAsync(newOrToUpdateVo, cancellationToken);

  public virtual async Task<List<MyEntityVo>> GetAllAsync(CancellationToken cancellationToken = default)
    => await _behavior.GetAllAsync(cancellationToken);

  public virtual async Task<MyEntityVo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    => await _behavior.GetByIdAsync(id, cancellationToken);

  public virtual async Task<List<MyEntityVo>?> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    => await _behavior.GetByIdsAsync(ids, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    => await _behavior.RemoveAsync(id, cancellationToken);

  public virtual async Task UpdateAsync(Guid id, MyEntityVo updatedItem, CancellationToken cancellationToken = default)
   => await _behavior.UpdateAsync(id, updatedItem, cancellationToken);
}
