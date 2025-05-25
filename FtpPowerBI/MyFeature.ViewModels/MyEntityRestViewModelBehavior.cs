// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels;
using MyFeature.ViewModels.BffProxying;
using MyFeature.ViewObjects;

namespace MyFeature.ViewModels;

public class MyEntityRestViewModelBehavior : RestViewModelBehavior<MyEntityVo, IMyEntityRestBffClient>
{
  public MyEntityRestViewModelBehavior(IMyEntityRestBffClient client) : base(client)
  {
  }
}
