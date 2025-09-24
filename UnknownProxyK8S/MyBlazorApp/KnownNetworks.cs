namespace MyBlazorApp;

public record KnownNetworks
{
  public string[] PrefixCdrNetworks { get; set; } = Array.Empty<string>();
}
