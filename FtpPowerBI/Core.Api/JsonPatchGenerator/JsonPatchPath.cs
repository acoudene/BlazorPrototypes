// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coud�ne       | Creation

namespace Core.Api.JsonPatchGenerator;

public class JsonPatchPath
{
  private readonly IReadOnlyList<string> _segments;

  public JsonPatchPath(IReadOnlyList<string> segments)
  {
    _segments = segments;
  }

  public static JsonPatchPath Root { get; } = new(Array.Empty<string>());

  public JsonPatchPath AddSegment(string s)
  {
    var newSegments = _segments.ToList();
    newSegments.Add(EscapeJsonPath(s));

    return new JsonPatchPath(newSegments);
  }

  public JsonPatchPath AtIndex(int i)
  {
    return AddSegment(i.ToString());
  }

  public override string ToString()
  {
    if (_segments.Count == 0) return string.Empty;

    var result = "/";
    result += string.Join("/", _segments);

    return result;
  }

  private static string EscapeJsonPath(string s)
  {
    return s.Replace("~", "~0").Replace("/", "~1");
  }
}