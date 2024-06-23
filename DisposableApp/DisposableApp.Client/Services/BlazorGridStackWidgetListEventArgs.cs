namespace Alteva.Blazor.GridStack.Models
{
    public class BlazorGridStackWidgetListEventArgs : EventArgs
    {
        public IEnumerable<BlazorGridStackWidgetData> Items { get; set; } = new List<BlazorGridStackWidgetData>();
    }
}
