namespace InjectionApp.Client.Services;

public class MyRazorDependency : IMyDependency
{
  public string MyDependencyProperty { get; } = Guid.NewGuid().ToString();
}