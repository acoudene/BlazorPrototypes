// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace MyFeature.Host.Tests;

internal class MyEntityData : TheoryData<MyEntityDto>
{
  public MyEntityData()
  {
    Add(new MyEntityDto()
    {
      Id = Guid.NewGuid()
      // TODO - EntityProperties - Fields to complete
    });

    Add(new MyEntityDto()
    {
      Id = Guid.NewGuid()
      // TODO - EntityProperties - Fields to complete
    });

    Add(new MyEntityDto()
    {
      Id = Guid.NewGuid()
      // TODO - EntityProperties - Fields to complete
    });
  }
}

