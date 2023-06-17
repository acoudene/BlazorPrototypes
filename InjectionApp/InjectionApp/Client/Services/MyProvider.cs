using Microsoft.AspNetCore.Components;

namespace InjectionApp.Client.Services
{
  public class MyProvider : IMyProvider
  {
    public IMyDependency MyDependency { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name"></param>
    public MyProvider(IMyDependency myDependency)
    {
      MyDependency = myDependency;
    }
  }
}
