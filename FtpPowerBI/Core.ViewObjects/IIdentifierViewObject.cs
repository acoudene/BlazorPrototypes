// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.ViewObjects;

public interface IIdentifierViewObject : IViewObject
{
    Guid Id { get; }
}
