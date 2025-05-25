// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace Core.Proxying;

public interface IRestClient<TDto> where TDto : class, IIdentifierDto
{
  Task<List<TDto>> GetAllAsync(CancellationToken cancellationToken = default);

  Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

  Task<List<TDto>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);

  Task CreateAsync(
    TDto dto,
    CancellationToken cancellationToken = default);

  Task CreateOrUpdateAsync(
      TDto newOrToUpdateDto,
      CancellationToken cancellationToken = default);

  Task UpdateAsync(
    Guid id,
    TDto dto,
    CancellationToken cancellationToken = default);

  Task<TDto?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

  Task PatchAsync(
    Guid id,
    JsonPatchDocument<TDto> patch,
    CancellationToken cancellationToken = default);
}