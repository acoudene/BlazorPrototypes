// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Linq;

namespace Core.Api.JsonPatchGenerator;

public interface IPatchContext
{
  JsonPatchDocument Document { get; }

  JsonPatchOptions Options { get; }

  void CreatePatch(JToken original, JToken modified, JsonPatchPath path);
}