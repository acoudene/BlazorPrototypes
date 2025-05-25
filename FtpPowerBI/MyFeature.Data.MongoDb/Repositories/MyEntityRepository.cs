// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;
using System.Linq.Expressions;

namespace MyFeature.Data.MongoDb.Repositories;

public class MyEntityRepository : IMyEntityRepository
{
  public const string CollectionName = "myEntity";

  protected TimeStampedMongoRepositoryBehavior<MyEntity, MyEntityMongo> Behavior { get => _behavior; }
  private readonly TimeStampedMongoRepositoryBehavior<MyEntity, MyEntityMongo> _behavior;

  public MyEntityRepository(IMongoContext mongoContext)
  {
    _behavior = new TimeStampedMongoRepositoryBehavior<MyEntity, MyEntityMongo>(mongoContext, CollectionName);
    _behavior.SetUniqueIndex(entity => entity.Id);
  }

  // This commented part could be used to have benefits of mongo entity typing
  //protected virtual MyEntityBase ToEntity(MyEntityMongoBase mongoEntity)
  //{
  //  return mongoEntity.ToInheritedEntity();
  //}

  // This commented part could be used to have benefits of mongo entity typing
  //protected virtual MyEntityMongoBase ToMongoEntity(MyEntityBase entity)
  //{
  //  return entity.ToInheritedMongo();
  //}

  protected virtual MyEntity ToEntity(MyEntityMongo mongoEntity)
  {
    return mongoEntity.ToEntity();
  }

  protected virtual MyEntityMongo ToMongoEntity(MyEntity entity)
  {
    return entity.ToMongo();
  }

  public virtual async Task<List<MyEntity>> GetAllAsync(CancellationToken cancellationToken = default) 
    => await _behavior.GetAllAsync(ToEntity, cancellationToken);

  public virtual async Task<MyEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) 
    => await _behavior.GetByIdAsync(id, ToEntity, cancellationToken);

  public virtual async Task<List<MyEntity>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default) 
    => await _behavior.GetByIdsAsync(ids, ToEntity, cancellationToken);

  public virtual async Task CreateAsync(MyEntity newItem, CancellationToken cancellationToken = default) 
    => await _behavior.CreateAsync(newItem, ToMongoEntity, cancellationToken);

  public virtual async Task UpdateAsync(MyEntity updatedItem, CancellationToken cancellationToken = default) 
    => await _behavior.UpdateAsync(updatedItem, ToMongoEntity, cancellationToken);

  public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default) 
    => await _behavior.RemoveAsync(id, cancellationToken);

  public virtual void SetUniqueIndex(params Expression<Func<MyEntityMongo, object>>[] fields)
    => _behavior.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(params string[] fields)
    => _behavior.SetUniqueIndex(fields);

  public virtual void SetUniqueIndex(params IndexKeysDefinition<MyEntityMongo>[] fields)
    => _behavior.SetUniqueIndex(fields);
}
