// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Data.MongoDb;

public interface IMongoSet<TMongoEntity> where TMongoEntity : IIdentifierMongoEntity
{
  IMongoCollection<TMongoEntity> GetCollection();

  Task<List<TMongoEntity>> GetAllAsync(CancellationToken cancellationToken = default);

  Task CreateAsync(TMongoEntity newItem, CancellationToken cancellationToken = default);

  Task UpdateAsync(Expression<Func<TMongoEntity, bool>> filter, TMongoEntity updatedItem, CancellationToken cancellationToken = default);

  Task RemoveAsync(Expression<Func<TMongoEntity, bool>> filter, CancellationToken cancellationToken = default);

  Task<TMongoEntity?> GetByFilterAsync(Expression<Func<TMongoEntity, bool>> filter, CancellationToken cancellationToken = default);

  Task<List<TMongoEntity>> GetItemsByFilterAsync(Expression<Func<TMongoEntity, bool>> filter, CancellationToken cancellationToken = default);

  Task<List<TMongoEntity>> GetItemsInAsync<TField>(Expression<Func<TMongoEntity, TField>> field, IEnumerable<TField> values, CancellationToken cancellationToken = default);
}