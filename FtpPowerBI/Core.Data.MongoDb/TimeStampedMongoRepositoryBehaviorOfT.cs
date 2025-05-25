// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.Data.MongoDb;

public class TimeStampedMongoRepositoryBehavior<TEntity, TMongoEntity> : MongoRepositoryBehavior<TEntity, TMongoEntity>
  where TEntity : IIdentifierEntity, ITimestampedEntity
  where TMongoEntity : IIdentifierMongoEntity, ITimestampedMongoEntity
{
  public TimeStampedMongoRepositoryBehavior(IMongoContext mongoContext, string collectionName)
    : base(mongoContext, collectionName)
  {
  }

  public override async Task CreateAsync(TEntity newItem, Func<TEntity, TMongoEntity> toMongoEntityFunc, CancellationToken cancellationToken = default)
  {
    if (newItem is null)
      throw new ArgumentNullException(nameof(newItem));

    if (toMongoEntityFunc is null)
      throw new ArgumentNullException(nameof(toMongoEntityFunc));

    Guid id = newItem.Id;
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    var newMongoEntity = toMongoEntityFunc(newItem);

    newMongoEntity.CreatedAt = DateTime.UtcNow;
    newMongoEntity.UpdatedAt = DateTime.UtcNow;

    await MongoSet.CreateAsync(newMongoEntity, cancellationToken);    
  }


  public override async Task UpdateAsync(TEntity updatedItem, Func<TEntity, TMongoEntity> toMongoEntityFunc, CancellationToken cancellationToken = default)
  {
    if (updatedItem is null)
      throw new ArgumentNullException(nameof(updatedItem));

    if (toMongoEntityFunc is null)
      throw new ArgumentNullException(nameof(toMongoEntityFunc));

    Guid id = updatedItem.Id;
    if (id == Guid.Empty)
      throw new ArgumentOutOfRangeException(nameof(id));

    var updatedMongoEntity = toMongoEntityFunc(updatedItem);

    updatedMongoEntity.UpdatedAt = DateTime.UtcNow;

    await MongoSet.UpdateAsync(x => x.Id == id, updatedMongoEntity, cancellationToken);    
  }
}
