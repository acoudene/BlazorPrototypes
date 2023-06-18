using InjectionApp.Client.Services;
using Microsoft.AspNetCore.Components;

namespace InjectionApp.Client.Pages;

public partial class Index
{

  [Inject]
  public IMyDependency MyRazorProperty { get; set; } = default!;

  protected override void OnInitialized()
  {
    // No null value here
    Console.Write($"Razor property injected: {MyRazorProperty.MyDependencyProperty}");
    base.OnInitialized();
  }
}
