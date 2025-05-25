// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos.Swaggers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Core.Api.Swaggers;

public class SwaggerIgnoreModelFilter : IDocumentFilter
{
  public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
  {
    // Get all models that are decorated with SwaggerExcludeAttribute
    // This will only work for models that are under current Assembly
    var excludedTypes = GetTypesWithHelpAttribute(Assembly.GetExecutingAssembly());
    // Loop through them
    foreach (var _type in excludedTypes)
    {
      // Check if that type exists in SchemaRepository
      if (context.SchemaRepository.TryLookupByType(_type, out _))
      {
        // If the type exists in SchemaRepository, check if name exists in the dictionary
        if (swaggerDoc.Components.Schemas.ContainsKey(_type.Name))
        {
          // Remove the schema
          swaggerDoc.Components.Schemas.Remove(_type.Name);
        }
      }
    }
  }

  // Get all types in assembly that contains SwaggerExcludeAttribute
  public static IEnumerable<Type> GetTypesWithHelpAttribute(Assembly assembly)
  {
    return assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(SwaggerExcludeAttribute), true).Length > 0);
  }
}