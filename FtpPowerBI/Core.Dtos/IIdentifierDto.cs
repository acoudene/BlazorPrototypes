// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.Dtos;

public interface IIdentifierDto : IDto
{
    Guid Id { get; set; }
}
