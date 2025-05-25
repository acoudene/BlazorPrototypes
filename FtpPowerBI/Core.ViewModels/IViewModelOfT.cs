// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewObjects;

namespace Core.ViewModels;

public interface IViewModel<TViewObject> : IViewModel where TViewObject : IViewObject
{
  Task<List<TViewObject>> GetAllAsync(CancellationToken cancellationToken = default);

  Task<TViewObject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

  Task<List<TViewObject>?> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);

  Task CreateAsync(TViewObject newItem, CancellationToken cancellationToken = default);

  Task CreateOrUpdateAsync(TViewObject newOrToUpdateVo, CancellationToken cancellationToken = default);

  Task UpdateAsync(Guid id, TViewObject updatedItem, CancellationToken cancellationToken = default);

  Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}
