// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Data.MongoDb;

public abstract class MongoRepositoryBase<TEntity, TMongoEntity> : IRepository<TEntity>
  where TEntity : IIdentifierEntity
  where TMongoEntity : IIdentifierMongoEntity
{
  protected MongoRepositoryBehavior<TEntity, TMongoEntity> Behavior { get => _behavior; }
  private readonly MongoRepositoryBehavior<TEntity, TMongoEntity> _behavior;

  protected MongoRepositoryBase(IMongoContext mongoContext, string collectionName)
    : this(new MongoRepositoryBehavior<TEntity, TMongoEntity>(mongoContext, collectionName))
  { }

  protected MongoRepositoryBase(MongoRepositoryBehavior<TEntity, TMongoEntity> behavior)
  {
    _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
    _behavior.SetUniqueIndex(entity => entity.Id);
  }

  protected abstract TEntity ToEntity(TMongoEntity mongoEntity);
  protected abstract TMongoEntity ToMongoEntity(TEntity entity);

  public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) 
    => await _behavior.GetAllAsync(ToEntity, cancellationToken);

  public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) 
    => await _behavior.GetByIdAsync(id, ToEntity, cancellationToken);

  public virtual async Task<List<TEntity>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default) 
    => await _behavior.GetByIdsAsync(ids, ToEntity, cancellationToken);

  public virtual async Task CreateAsync(TEntity newItem, CancellationToken cancellationToken = default) 
    => await _behavior.CreateAsync(newItem, ToMongoEntity, cancellationToken);

  public virtual async Task UpdateAsync(TEntity updatedItem, CancellationToken cancellationToken = default) 
    => await _behavior.UpdateAsync(updatedItem, ToMongoEntity, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default) 
    => await _behavior.RemoveAsync(id, cancellationToken);

  public virtual void SetUniqueIndex(params Expression<Func<TMongoEntity, object>>[] fields)
      => _behavior.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(params string[] fields)
      => _behavior.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(params IndexKeysDefinition<TMongoEntity>[] fields)
      => _behavior.SetUniqueIndex(fields);
}
