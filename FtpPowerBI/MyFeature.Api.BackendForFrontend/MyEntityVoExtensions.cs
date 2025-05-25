// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MyFeature.Dtos;
using MyFeature.ViewObjects;

namespace MyFeature.Api.BackendForFrontend;

public static class MyEntityVoExtensions
{
  public static MyEntityVo ToViewObject(this MyEntityDto dto)
  {
    if (dto is null)
      return null!;

    switch (dto)
    {
      case MyEntityDto:
        {
          return new MyEntityVo()
          {
            Id = dto.Id,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,

            // TODO - EntityMapping - Dto to ViewObject to complete

            Metadata = dto.Metadata,
          };
        }

      default:
        throw new NotImplementedException();
    }
  }

  public static MyEntityDto ToDto(this MyEntityVo viewObject)
  {
    if (viewObject is null)
      return null!;

    switch (viewObject)
    {
      case MyEntityVo:
        {
          return new MyEntityDto()
          {
            Id = viewObject.Id,
            CreatedAt = viewObject.CreatedAt,
            UpdatedAt = viewObject.UpdatedAt,

            // TODO - EntityMapping - ViewObject to Dto to complete

            Metadata = viewObject.Metadata,
          };
        }

      default:
        throw new NotImplementedException();
    }
  }
}
