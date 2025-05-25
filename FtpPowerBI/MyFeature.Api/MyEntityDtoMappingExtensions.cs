// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace MyFeature.Api;

public static class MyEntityDtoMappingExtensions
{
  // This commented part could be used to have benefits of json entity typing
  //public static MyEntityDtoBase ToInheritedDto(this MyEntityBase entity)
  //{
  //  switch (entity)
  //  {
  //    case MyEntityInherited inheritedEntity: return inheritedEntity.ToDto();
  //    default:
  //      throw new NotImplementedException();
  //  }
  //}

  // This commented part could be used to have benefits of json entity typing
  //public static MyEntityBase ToInheritedEntity(this MyEntityDtoBase dto)
  //{
  //  switch (dto)
  //  {
  //    case MyEntityInheritedDto inheritedDto: return inheritedDto.ToEntity();
  //    default:
  //      throw new NotImplementedException();
  //  }
  //}

  public static MyEntityDto ToDto(this MyEntity entity)
  {
    return new MyEntityDto()
    {
      Id = entity.Id,
      CreatedAt = entity.CreatedAt,
      UpdatedAt = entity.UpdatedAt,

      // TODO - EntityMapping - Business Entity to Dto to complete

      Metadata = entity.Metadata,
    };
  }

  public static MyEntity ToEntity(this MyEntityDto dto)
  {
    return new MyEntity()
    {
      Id = dto.Id,
      CreatedAt = dto.CreatedAt,
      UpdatedAt = dto.UpdatedAt,

      // TODO - EntityMapping - Dto to Business Entity to complete

      Metadata = dto.Metadata,
    };
  }
}
