using Microsoft.JSInterop;

namespace Alteva.Blazor.JsEvent.Helpers
{
    internal static class Helper
    {
        public static Lazy<Task<IJSObjectReference>> GetJavascriptModule(IJSRuntime jsRuntime, string src)
        {            
            string cacheNumber = ((IJSInProcessRuntime)jsRuntime).Invoke<string>("getCacheNumber");

            return new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", $"{src}{cacheNumber}").AsTask());
        }
    }
}
