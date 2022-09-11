using Microsoft.AspNetCore.Components;

namespace InjectionApp.Client.Services
{
  public class MyProvider : IMyProvider
  {
    private IMyOtherDependency _myOtherDependency;

    [Inject]
    public IMyDependency MyProperty { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name"></param>
    public MyProvider(IMyOtherDependency myOtherDependency)
    {
      _myOtherDependency = myOtherDependency;
    }
  }
}
