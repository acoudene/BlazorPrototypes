// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.Data;

public interface IIdentifierEntity : IEntity
{
    Guid Id { get; }
}
