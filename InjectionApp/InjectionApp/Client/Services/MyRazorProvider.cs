using Microsoft.AspNetCore.Components;

namespace InjectionApp.Client.Services
{
  public class MyRazorProvider : IMyProvider
  {
    [Inject]
    public IMyDependency MyDependency { get; private set; } = default!;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name"></param>
    public MyRazorProvider()
    {
    }
  }
}
