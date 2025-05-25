// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels.BffProxying;
using MyFeature.ViewObjects;

namespace MyFeature.ViewModels.BffProxying;

/// <summary>
/// Interface to manage client/server interaction as a REST proxy
/// </summary>
public interface IMyEntityRestBffClient : IRestBffClient<MyEntityVo>
{
}
