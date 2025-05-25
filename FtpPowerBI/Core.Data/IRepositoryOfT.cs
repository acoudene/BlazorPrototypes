// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.Data;

public interface IRepository<TEntity> : IRepository where TEntity : IEntity
{
  Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

  Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

  Task<List<TEntity>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);

  Task CreateAsync(TEntity newItem, CancellationToken cancellationToken = default);

  Task UpdateAsync(TEntity updatedItem, CancellationToken cancellationToken = default);
  
  Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}