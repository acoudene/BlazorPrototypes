// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewObjects;
using Microsoft.AspNetCore.JsonPatch;

namespace Core.ViewModels.BffProxying;

public interface IRestBffClient<TViewObject> where TViewObject : class, IIdentifierViewObject
{
    Task<List<TViewObject>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TViewObject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<TViewObject>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);

    Task CreateAsync(
      TViewObject vo,
      CancellationToken cancellationToken = default);

    Task CreateOrUpdateAsync(
        TViewObject newOrToUpdateVo,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
      Guid id,
      TViewObject vo,
      CancellationToken cancellationToken = default);

    Task<TViewObject?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task PatchAsync(
      Guid id,
      JsonPatchDocument<TViewObject> patch,
      CancellationToken cancellationToken = default);
}