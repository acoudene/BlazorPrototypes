// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Data.MongoDb;

public class MongoRepositoryBehavior<TEntity, TMongoEntity>
  where TEntity : IIdentifierEntity
  where TMongoEntity : IIdentifierMongoEntity
{
  public IMongoContext MongoContext { get => _mongoContext; }
  public IMongoSet<TMongoEntity> MongoSet { get => _mongoSet; }

  private readonly IMongoContext _mongoContext;
  private readonly IMongoSet<TMongoEntity> _mongoSet;

  public MongoRepositoryBehavior(IMongoContext mongoContext, string collectionName)
  {
    if (mongoContext is null)
      throw new ArgumentNullException(nameof(mongoContext));
    if (string.IsNullOrWhiteSpace(collectionName))
      throw new ArgumentNullException(nameof(collectionName));

    _mongoContext = mongoContext;
    _mongoSet = new MongoSet<TMongoEntity>(mongoContext, collectionName);
  }

  public virtual async Task<List<TEntity>> GetAllAsync(
    Func<TMongoEntity, TEntity> toEntityFunc, 
    CancellationToken cancellationToken = default)
  {
    if (toEntityFunc is null)
      throw new ArgumentNullException(nameof(toEntityFunc));

    return (await _mongoSet.GetAllAsync(cancellationToken))    
    .Select(mongoEntity => toEntityFunc(mongoEntity))
    .ToList();
  }

  public virtual async Task<TEntity?> GetByIdAsync(
    Guid id, 
    Func<TMongoEntity, TEntity> toEntityFunc, 
    CancellationToken cancellationToken = default)
  {
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    if (toEntityFunc is null)
      throw new ArgumentNullException(nameof(toEntityFunc));

    var mongoEntity = await _mongoSet.GetByFilterAsync(x => x.Id == id, cancellationToken);
    if (mongoEntity is null)
      return default;

    return toEntityFunc(mongoEntity);
  }

  public virtual async Task<List<TEntity>> GetByIdsAsync(
    List<Guid> ids, 
    Func<TMongoEntity, TEntity> toEntityFunc, 
    CancellationToken cancellationToken = default)
  {
    // db.getCollection("<CollectionName>").find({id: {$in: [UUID("3FA85F64-5717-4562-B3FC-2C963F66AFA1"),UUID("3FA85F64-5717-4562-B3FC-2C963F66AFA2")]}})

    if (toEntityFunc is null)
      throw new ArgumentNullException(nameof(toEntityFunc));

    return (await MongoSet.GetItemsInAsync(x => x.Id, ids, cancellationToken))
            .Select(mongoEntity => toEntityFunc(mongoEntity))
            .ToList();
  }

  public virtual async Task CreateAsync(
    TEntity newItem, 
    Func<TEntity, TMongoEntity> toMongoEntityFunc, 
    CancellationToken cancellationToken = default)
  {
    if (newItem is null)
      throw new ArgumentNullException(nameof(newItem));

    if (toMongoEntityFunc is null)
      throw new ArgumentNullException(nameof(toMongoEntityFunc));

    Guid id = newItem.Id;
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    var newMongoEntity = toMongoEntityFunc(newItem);
    await _mongoSet.CreateAsync(newMongoEntity, cancellationToken);
  }

  public virtual async Task UpdateAsync(
    TEntity updatedItem, 
    Func<TEntity, TMongoEntity> toMongoEntityFunc, 
    CancellationToken cancellationToken = default)
  {
    if (updatedItem is null)
      throw new ArgumentNullException(nameof(updatedItem));

    if (toMongoEntityFunc is null)
      throw new ArgumentNullException(nameof(toMongoEntityFunc));

    Guid id = updatedItem.Id;
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    var updatedMongoEntity = toMongoEntityFunc(updatedItem);
    await _mongoSet.UpdateAsync(x => x.Id == id, updatedMongoEntity, cancellationToken);
  }

  public virtual async Task RemoveAsync(
    Guid id, 
    CancellationToken cancellationToken = default)
  {
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    await _mongoSet.RemoveAsync(x => x.Id == id, cancellationToken);
  }

  public virtual void SetUniqueIndex(params Expression<Func<TMongoEntity, object>>[] fields)
      => SetUniqueIndex(fields.Select(f => Builders<TMongoEntity>.IndexKeys.Ascending(f)).ToArray());

  public virtual void SetUniqueIndex(params string[] fields)
      => SetUniqueIndex(fields.Select(f => Builders<TMongoEntity>.IndexKeys.Ascending(f)).ToArray());

  public virtual void SetUniqueIndex(params IndexKeysDefinition<TMongoEntity>[] fields)
  {
    if (!fields.Any())
      return;

    var indexKeysDefinition = Builders<TMongoEntity>.IndexKeys.Combine(fields);

    var indexModel = new CreateIndexModel<TMongoEntity>(indexKeysDefinition, new CreateIndexOptions() { Unique = true });

    MongoSet.GetCollection()
      .Indexes
      .CreateOne(indexModel);
  }
}
