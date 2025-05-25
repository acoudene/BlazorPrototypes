// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.Data;

public interface ITrackedRepository<TEntity, TTrackedMetadata> : IRepository
  where TEntity : IEntity
  where TTrackedMetadata : ITrackedMetadata
{
  Task<List<TEntity>> GetAllAsync(TTrackedMetadata? trackedMetadata = default, CancellationToken cancellationToken = default);

  Task<TEntity?> GetByIdAsync(Guid id, TTrackedMetadata? trackedMetadata = default, CancellationToken cancellationToken = default);

  Task<List<TEntity>> GetByIdsAsync(List<Guid> ids, TTrackedMetadata? trackedMetadata = default, CancellationToken cancellationToken = default);

  Task CreateAsync(TEntity newItem, TTrackedMetadata? trackedMetadata = default, CancellationToken cancellationToken = default);

  Task UpdateAsync(TEntity updatedItem, TTrackedMetadata? trackedMetadata = default, CancellationToken cancellationToken = default);

  Task RemoveAsync(Guid id, TTrackedMetadata? trackedMetadata = default, CancellationToken cancellationToken = default);
}