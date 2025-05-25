// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coud�ne       | Creation

using Core.Api.JsonPatchGenerator.Handlers;

namespace Core.Api.JsonPatchGenerator;

public class JsonPatchOptions
{
  public List<IPatchTypeHandler> Handlers { get; set; } = new()
          {new ArrayPatchTypeHandler(), new ObjectPatchTypeHandler(), new ReplaceValuePatchTypeHandler()};
}