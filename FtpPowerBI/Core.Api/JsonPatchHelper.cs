// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Core.Api;

public static class JsonPatchHelper
{
  public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
  {
    var builder = new ServiceCollection()
        .AddLogging()
        .AddMvc()
        .AddNewtonsoftJson()
        .Services.BuildServiceProvider();

    return builder
        .GetRequiredService<IOptions<MvcOptions>>()
        .Value
        .InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>()
        .First();
  }

  //// Don't use it for polymorphic issues and lack of precision

  //public static JsonPatchDocument GeneratePatch<TDocument>(TDocument originalObject, TDocument modifiedObject, IContractResolver? contractResolver = null)
  //  where TDocument : class
  //{
  //  if (originalObject is null)
  //    throw new ArgumentNullException(nameof(originalObject));
  //  if (modifiedObject is null)
  //    throw new ArgumentNullException(nameof(modifiedObject));

  //  var original = JObject.FromObject(originalObject);
  //  var modified = JObject.FromObject(modifiedObject);

  //  var patch = new JsonPatchDocument() 
  //  { 
  //    ContractResolver = contractResolver ?? (new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() }) 
  //  };

  //  FillPatch(original, modified, string.Empty, patch);

  //  return patch;
  //}

  //private static void FillPatch(JObject source, JObject target, string path, JsonPatchDocument patchToFill)
  //{
  //  if (source is null)
  //    throw new ArgumentNullException(nameof(source));
  //  if (target is null)
  //    throw new ArgumentNullException(nameof(target));
  //  if (patchToFill is null)
  //    throw new ArgumentNullException(nameof(patchToFill));

  //  // Take source and target properties
  //  var sourceProperties = source.Properties();
  //  var targetProperties = target.Properties();
  //  Func<JProperty, string> byPropertyNameSelector = property => property.Name;
  //  Func<JProperty, string> byPropertyNamePath = property => $"{path}/{property.Name}";

  //  // Adding - Target properties are patched as added if not exist by name in source properties
  //  targetProperties
  //    .ExceptBy(sourceProperties.Select(byPropertyNameSelector), byPropertyNameSelector)
  //    .ToList()
  //    .ForEach(property => patchToFill.Add(byPropertyNamePath(property), property.Value));

  //  // Removing - Source properties are patched as removed if not exist by name in target properties
  //  sourceProperties
  //    .ExceptBy(targetProperties.Select(byPropertyNameSelector), byPropertyNameSelector)      
  //    .ToList()
  //    .ForEach(property => patchToFill.Remove(byPropertyNamePath(property)));

  //  // Updating - Manage intersection by comparing type then value
  //  sourceProperties
  //    .IntersectBy(targetProperties.Select(byPropertyNameSelector), byPropertyNameSelector)
  //    .ToList()
  //    .ForEach(currentSourceProperty =>
  //    {
  //      string currentSourcePropertyPath = byPropertyNamePath(currentSourceProperty);
  //      string currentSourcePropertyName = currentSourceProperty.Name;
  //      var currentSourcePropertyValue = currentSourceProperty.Value;
  //      var currentSourcePropertyValueType = currentSourceProperty.Value.Type;

  //      // Get associated target property
  //      var targetProperty = target.Property(currentSourcePropertyName);
  //      if (targetProperty is null)
  //        throw new InvalidOperationException($"{nameof(targetProperty)} is not found by {currentSourcePropertyName} as expected");
  //      var targetPropertyValue = targetProperty.Value;
  //      var targetPropertyValueType = targetProperty.Value.Type;

  //      if (currentSourcePropertyValueType != targetPropertyValueType)
  //      {
  //        // Updating - If value types are different then directly replace source value by target one
  //        patchToFill.Replace(currentSourcePropertyPath, targetPropertyValue);
  //      }
  //      else
  //      {
  //        // If value types are equal then check if value type is object or scalar
  //        string currentJsonSourcePropertyValue = currentSourcePropertyValue.ToString(Newtonsoft.Json.Formatting.None);
  //        string jsonTargetPropertyValue = targetPropertyValue.ToString(Newtonsoft.Json.Formatting.None);

  //        if (!string.Equals(currentJsonSourcePropertyValue, jsonTargetPropertyValue))
  //        {
  //          if (currentSourcePropertyValueType == JTokenType.Object)
  //          {
  //            var currentSourceJObject = currentSourcePropertyValue as JObject;
  //            if (currentSourceJObject is null)
  //              throw new InvalidCastException($"{nameof(currentSourceJObject)} cannot be cast as {nameof(JObject)} as expected for path {currentSourcePropertyPath}");

  //            var targetJObject = targetPropertyValue as JObject;
  //            if (targetJObject is null)
  //              throw new InvalidCastException(nameof(targetJObject));

  //            // If value types are object then graph recursion
  //            FillPatch(currentSourceJObject, targetJObject, currentSourcePropertyPath, patchToFill);
  //          }
  //          else
  //          {
  //            // Updating scalar - Replace source value directly by target value
  //            patchToFill.Replace(currentSourcePropertyPath, targetPropertyValue);
  //          }
  //        }
  //        else
  //        {
  //          // Do nothing because equal
  //        }
  //      }

  //    });
  //}
}