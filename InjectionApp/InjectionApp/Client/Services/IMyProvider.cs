namespace InjectionApp.Client.Services
{
  public interface IMyProvider
  {
    IMyDependency MyDependency { get; }
  }
}
