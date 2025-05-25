// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Newtonsoft.Json.Linq;

namespace Core.Api.JsonPatchGenerator;

public interface IPatchTypeHandler
{
  bool CanPatch(JToken original, JToken modified);

  void CreatePatch(JToken original, JToken modified, JsonPatchPath path, IPatchContext context);
}