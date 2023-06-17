namespace InjectionApp.Client.Services
{
  public class MyDependency : IMyDependency
  {
    public string MyDependencyProperty { get; set; } = Guid.NewGuid().ToString();
  }
}

