// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace MyFeature.Data.MongoDb.Entities;

public static class MyEntityMongoMappingExtensions
{
  // This commented part could be used to have benefits of mongo entity typing
  //public static MyEntityMongoBase ToInheritedMongo(this MyEntityBase entity)
  //{
  //  switch (entity)
  //  {
  //    case MyEntityInherited inheritedEntity: return inheritedEntity.ToMongo();
  //    default:
  //      throw new NotImplementedException();
  //  }
  //}

  // This commented part could be used to have benefits of mongo entity typing
  //public static MyEntityBase ToInheritedEntity(this MyEntityMongoBase mongoEntity)
  //{
  //  switch (mongoEntity)
  //  {
  //    case MyEntityInheritedMongo inheritedMongo: return inheritedMongo.ToEntity();
  //    default:
  //      throw new NotImplementedException();
  //  }
  //}

  public static MyEntityMongo ToMongo(this MyEntity entity)
  {
    return new MyEntityMongo()
    {
      Id = entity.Id,
      CreatedAt = entity.CreatedAt.UtcDateTime,
      UpdatedAt = entity.UpdatedAt.UtcDateTime,

      // TODO - EntityMapping - Business Entity to Mongo Entity to complete

      Metadata = entity.Metadata,
      
    };
  }

  public static MyEntity ToEntity(this MyEntityMongo mongoEntity)
  {
    return new MyEntity()
    {
      Id = mongoEntity.Id,
      CreatedAt = mongoEntity.CreatedAt,
      UpdatedAt = mongoEntity.UpdatedAt,
        
      // TODO - EntityMapping - Mongo Entity to Business Entity to complete

      Metadata = mongoEntity.Metadata,
    };
  }
}
