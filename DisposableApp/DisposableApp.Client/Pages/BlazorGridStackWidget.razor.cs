using Alteva.Blazor.GridStack.Models;
using Microsoft.AspNetCore.Components;

namespace DisposableApp.Client.Pages
{
    partial class BlazorGridStackWidget : ComponentBase
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public BlazorGridStackWidgetOptions? WidgetOptions { get; set; }

        [Parameter] public string CssClass { get; set; } = "";
    }
}
