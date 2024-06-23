using Alteva.Blazor.JsEvent.Helpers;
using Alteva.Blazor.JsEvent.Model;
using Microsoft.JSInterop;

namespace Alteva.Blazor.JsEvent.Services
{
    /// <summary>
    /// Service permettant d'interagir avec javascript
    /// https://code-maze.com/wrapping-javascript-libraries-with-csharp-in-blazor-webassembly/
    /// </summary>
    public class JsUtilsService : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public JsUtilsService(IJSRuntime jsRuntime)
        {
            moduleTask = Helper.GetJavascriptModule(jsRuntime, "./JsUtils.js");
        }

        /// <summary>
        /// renvoie les dimensions de la fenetre d'affichage du browser
        /// </summary>
        /// <returns></returns>
        public async Task<WindowDimension> GetWindowDimensionAsync()
        {
            var module = await moduleTask.Value;
            var res = await module.InvokeAsync<WindowDimension>("getWindowDimensions");
            return res;
        }

        /// <summary>
        /// renvoi les dimensions d'un élément
        /// 
        /// [Inject]
        /// public JsUtilsService JsUtils { get; set; }
        /// 
        /// ex: c# : await JsUtils.GetElementDimensionsAsync("monId");
        /// html : <div id="monId" >
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        public async Task<BoundingClientRect> GetElementDimensionsAsync(string elementId)
        {
            var module = await moduleTask.Value;
            var res = await module.InvokeAsync<BoundingClientRect>("getElementDimensions", elementId);
            return res;
        }

        /// <summary>
        /// ouvre une url dans un nouvel onglet
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task OpenUrlAsync(string url)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("openUrl", url);
        }

        /// <summary>
        /// donne le focus à un elementId
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        public async Task SetFocusByElementIdAsync(string elementId)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("focusInput", elementId);
        }



        /// <summary>
        /// surcharge les liens d'un texte en html pour qu'ils s'affichent dans un autre onglet
        /// en ajoutant target="_blank" pour chaque liens
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public async Task<string> AddTargetBlankToLinks(string htmlString)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("addTargetBlankToLinks", htmlString);
        }


        /// <summary>
        /// renvoi la largeur d'un élément
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        public async Task<double> GetWidthByElementId(string elementId)
        {
            var module = await moduleTask.Value;
            var res = await module.InvokeAsync<double>("GetWidthByElementId", elementId);
            return res;
        }

        /// <summary>
        /// renvoi la hauteur d'un élément
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        public async Task<double> GetHeightByElementId(string elementId)
        {
            var module = await moduleTask.Value;
            var res = await module.InvokeAsync<double>("GetHeightByElementId", elementId);
            return res;
        }

        /// <summary>
        /// renvoi la coordonnée en X d'un élément
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        public async Task<double> GetLeftPositionByElementId(string elementId)
        {
            var module = await moduleTask.Value;
            var res = await module.InvokeAsync<double>("GetLeftPositionByElementId", elementId);
            return res;
        }

        /// <summary>
        /// renvoi la coordonnée en X d'un élément
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        public async Task<double> GetBottomPositionByElementId(string elementId)
        {
            var module = await moduleTask.Value;
            var res = await module.InvokeAsync<double>("GetBottomPositionByElementId", elementId);
            return res;
        }

        /// <summary>
        /// simule un click sur un élément du DOM
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        public async Task SimulateClickByElementId(string elementId)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("SimulateClickByElementId", elementId);
        }

        /// <summary>
        /// enregistre un fichier
        /// </summary>
        /// <param name="filename">nom complet du fichier (ex: "monfichier.xlsx")</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveAsFile(string filename, byte[] data)
        {
            if (string.IsNullOrWhiteSpace(filename)) return;
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("saveAsFile", filename, Convert.ToBase64String(data));
        }

        // <summary>
        /// enregistre un fichier
        /// </summary>
        /// <param name="filename">nom complet du fichier (ex: "monfichier.xlsx")</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveAsFile(string filename, MemoryStream data)
        {            
            if (data is null) data = new MemoryStream();
            await SaveAsFile(filename, data.ToArray());
        }


        #region IAsyncDisposable        
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {            
            if (moduleTask is not null && moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                if (module is not null)
                {
                    await module.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
        #endregion
    }
}
