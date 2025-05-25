// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace MyFeature.Host.Tests;

internal class MyEntitysData : TheoryData<List<MyEntityDto>>
{
  public MyEntitysData()
  {
    Add([
          new MyEntityDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          },
          new MyEntityDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          }
        ]);

    Add([
          new MyEntityDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          },
          new MyEntityDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          }
        ]);

    Add([
          new MyEntityDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          },
          new MyEntityDto()
          {
            Id = Guid.NewGuid()
            // TODO - EntityProperties - Fields to complete
          }
        ]);
  }
}

