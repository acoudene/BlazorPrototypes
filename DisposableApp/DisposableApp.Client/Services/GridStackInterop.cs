using Alteva.Blazor.GridStack.Models;
using DisposableApp.Client.Pages;
using Microsoft.JSInterop;

namespace Alteva.Blazor.GridStack
{
    internal class GridStackInterop : IAsyncDisposable
    {
        public event EventHandler<BlazorGridStackWidgetListEventArgs>? OnAdded;
        public event EventHandler<BlazorGridStackWidgetListEventArgs>? OnChange;
        public event EventHandler? OnDisable;
        public event EventHandler<BlazorGridStackWidgetEventArgs>? OnDragStart;
        public event EventHandler<BlazorGridStackWidgetEventArgs>? OnDrag;
        public event EventHandler<BlazorGridStackWidgetEventArgs>? OnDragStop;
        public event EventHandler<BlazorGridStackDroppedEventArgs>? OnDropped;
        public event EventHandler? OnEnable;
        public event EventHandler<BlazorGridStackWidgetListEventArgs>? OnRemoved;
        public event EventHandler<BlazorGridStackWidgetEventArgs>? OnResizeStart;
        public event EventHandler<BlazorGridStackWidgetEventArgs>? OnResize;
        public event EventHandler<BlazorGridStackWidgetEventArgs>? OnResizeStop;

        private readonly Lazy<Task<IJSObjectReference>> moduleTask;
        private Func<int, IJSObjectReference, bool>? AcceptWidgetsDelegate;
        private IJSObjectReference? _gridInstance;

        private IJSObjectReference GridInstance
        {
            get
            {
                if (_gridInstance is null) throw new InvalidOperationException($"{nameof(_gridInstance)} is not initialized. Call Init(id, options) to do it.");
                return _gridInstance;
            }
            set
            {
                if (_gridInstance is not null)
                    throw new InvalidOperationException("Cannot be initialized more than once...");

                _gridInstance = value;
            }
        }
        public GridStackInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Alteva.Blazor.GridStack/gridStackInterop.js").AsTask());
        }

        public async Task Init(string Id, BlazorGridStackBodyOptions options)
        {
            var module = await moduleTask.Value;
            var interopRef = DotNetObjectReference.Create(this);

            GridInstance = await module.InvokeAsync<IJSObjectReference>("init", Id, SerializeModelToDictionary(options), interopRef);
        }

        /// <summary>
        /// est ce qu'un élément existe dans le DOM ?
        /// </summary>
        /// <param name="id">Id de l'élément</param>
        /// <returns></returns>
        public async Task<bool> IsExistingElementById(string id)
        {
            var module = await moduleTask.Value;
            var interopRef = DotNetObjectReference.Create(this);
            var res = await module.InvokeAsync<bool>("isExistingElementById", id, interopRef);
            return res;
        }
        
        
        /// <summary>
        /// créer une subgrid à partir d'un widget existant
        /// le widget deviendra enfant de la subgrid crée
        /// </summary>
        /// <param name="id">Id de l'élément</param>
        /// <returns></returns>
        public async Task MakeSubGrid(string id, BlazorGridStackBodyOptions widgetOptions)
        {
            var module = await moduleTask.Value;
            var interopRef = DotNetObjectReference.Create(this);
            await module.InvokeVoidAsync("makeSubGrid", id, widgetOptions, interopRef);

        }

        /// <summary>
        /// ajoute une subgrid
        /// </summary>
        /// <param name="widgetOptions"></param>
        /// <returns></returns>
        public async Task<BlazorGridStackWidget> AddSubGrid(BlazorGridStackWidgetOptions widgetOptions)
        {

            var res = await GridInstance.InvokeAsync<object?>("addSubGridForBlazor", widgetOptions);
            return new BlazorGridStackWidget();
        }

        /// <summary>
        /// permet d'initialiser len D&D de la liste des FieldTemplate vers le FormTemplate
        /// </summary>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public async Task SetupDragIn(string cssClass = "", bool clone = true)
        {
            await GridInstance.InvokeVoidAsync("setupDragInForBlazor", cssClass, clone);
        }
        
        public async Task<BlazorGridStackWidget> AddWidget(BlazorGridStackWidgetOptions widgetOptions)
        {
            var res = await GridInstance.InvokeAsync<object?>("addWidgetForBlazor", widgetOptions);
            return new BlazorGridStackWidget();
        }

        public async Task<BlazorGridStackWidget> AddWidget(string id)
        {
            return await GridInstance.InvokeAsync<BlazorGridStackWidget>("addWidgetById", id);
        }

        public async Task BatchUpdate(bool? flag = null)
        {
            await GridInstance.InvokeVoidAsync("batchUpdate", flag);
        }

        public async Task Compact()
        {
            await GridInstance.InvokeVoidAsync("compact");
        }

        public async Task CellHeight(int val, bool? update = null)
        {
            await GridInstance.InvokeVoidAsync("cellHeight", val, update);
        }

        public async Task<int> CellWidth()
        {
            return await GridInstance.InvokeAsync<int>("cellWidth");
        }

        public async Task Column(int column, string? layout = null)
        {
            await GridInstance.InvokeVoidAsync("column", column, layout);
        }

        public async Task Destroy(bool? removeDom = null)
        {
            await GridInstance.InvokeVoidAsync("destroy", removeDom);
        }

        public async Task Disable()
        {
            await GridInstance.InvokeVoidAsync("disable");
        }

        public async Task Enable()
        {
            await GridInstance.InvokeVoidAsync("enable");
        }

        public async Task EnableMove(bool doEnable)
        {
            await GridInstance.InvokeVoidAsync("enableMove", doEnable);
        }

        public async Task EnableResize(bool doEnable)
        {
            await GridInstance.InvokeVoidAsync("enableResize", doEnable);
        }

        public async Task SetFloat(bool? val = null)
        {
            await GridInstance.InvokeVoidAsync("float", val);
        }

        public async Task<bool> GetFloat()
        {
            return await GridInstance.InvokeAsync<bool>("float");
        }

        public async Task<int> GetCellHeight()
        {
            return await GridInstance.InvokeAsync<int>("getCellHeight");
        }

        public async Task<BlazorGridCoordinates> GetCellFromPixel(int top, int left, bool? useOffset = null)
        {
            return await GridInstance.InvokeAsync<BlazorGridCoordinates>("getCellFromPixel", new { top, left }, useOffset);
        }

        public async Task<int> GetColumn()
        {
            return await GridInstance.InvokeAsync<int>("getColumn");
        }

        public async Task<IEnumerable<BlazorGridStackWidgetData>> GetGridItems()
        {
            var res = await GridInstance.InvokeAsync<IEnumerable<BlazorGridStackWidgetData>>("getGridItemsForBlazor");
            return res; 
        }

        public async Task<int> GetMargin()
        {
            return await GridInstance.InvokeAsync<int>("getMargin");
        }

        public async Task<bool> IsAreaEmpty(int x, int y, int width, int height)
        {
            return await GridInstance.InvokeAsync<bool>("isAreaEmpty", x, y, width, height);
        }

        public async Task Load(IEnumerable<BlazorGridStackWidgetOptions> layout, bool? addAndRemove = null)
        {
            await GridInstance.InvokeVoidAsync("load", layout, addAndRemove);
        }

        public async Task MakeWidget(string id)
        {
            await GridInstance.InvokeVoidAsync("makeWidgetById", id);
        }

        public async Task Margin(int value)
        {
            await GridInstance.InvokeVoidAsync("margin", value);
        }

        public async Task Margin(string value)
        {
            await GridInstance.InvokeVoidAsync("margin", value);
        }

        public async Task Movable(string id, bool val)
        {
            await GridInstance.InvokeVoidAsync("movabletById", id, val);
        }

        public async Task RemoveWidget(string id, bool? removeDOM = null, bool? triggerEvent = true)
        {
            await GridInstance.InvokeVoidAsync("removeWidgetById", id, removeDOM, triggerEvent);
        }

        public async Task RemoveAll(bool? removeDOM = null)
        {
            await GridInstance.InvokeVoidAsync("removeAll", removeDOM);
        }

        public async Task Resizable(string id, bool val)
        {
            await GridInstance.InvokeVoidAsync("resizableById", id, val);
        }

        public async Task Save(bool? saveContent)
        {
            await GridInstance.InvokeVoidAsync("save", saveContent);
        }

        public async Task SetAnimation(bool doAnimate)
        {
            await GridInstance.InvokeVoidAsync("setAnimation", doAnimate);
        }

        public async Task SetStatic(bool staticValue)
        {
            await GridInstance.InvokeVoidAsync("setStatic", staticValue);
        }

        public async Task Update(string id, BlazorGridStackWidgetOptions opts)
        {
            await GridInstance.InvokeVoidAsync("updateById", id, opts);
        }

        public async Task<bool> WillItFit(int x, int y, int width, int height, bool autoPosition)
        {
            return await GridInstance.InvokeAsync<bool>("willItFit", x, y, width, height, autoPosition);
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

        private Dictionary<string, object> SerializeModelToDictionary(object? model)
        {
            var result = new Dictionary<string, object>();

            if (model != null)
            {
                foreach (var property in model.GetType().GetProperties())
                {
                    var value = property.GetValue(model);
                    var name = property.Name.Substring(0, 1).ToLower() + property.Name.Substring(1);

                    if (value is int or bool or string or float)
                    {
                        result.Add(name, value);
                    }

                    else if (value is BlazorGridStackBodyAcceptWidgets acceptWidgets)
                    {
                        if (acceptWidgets.BoolValue != null)
                            result.Add(name, acceptWidgets.BoolValue);

                        else if (acceptWidgets.StringValue != null)
                            result.Add(name, acceptWidgets.StringValue);

                        else if (acceptWidgets.FuncValue != null)
                        {
                            result.Add("acceptWidgetsEvent", true);
                            AcceptWidgetsDelegate = acceptWidgets.FuncValue;
                        }
                    }

                    else if (value != null)
                    {
                        result.Add(name, SerializeModelToDictionary(value));
                    }
                }
            }
            return result;
        }

        //EVENTS
        [JSInvokable]
        public bool AcceptWidgetsDelegateFired(int number, IJSObjectReference element)
        {
            return AcceptWidgetsDelegate?.Invoke(number, element) ?? false;
        }

        [JSInvokable]
        public void AddedFired(BlazorGridStackWidgetData[] widgets)
        {
            OnAdded?.Invoke(this, new BlazorGridStackWidgetListEventArgs { Items = widgets });
        }

        [JSInvokable]
        public void ChangeFired(BlazorGridStackWidgetData[] widgets)
        {
            OnChange?.Invoke(this, new BlazorGridStackWidgetListEventArgs { Items = widgets });
        }

        [JSInvokable]
        public void DisableFired()
        {
            OnDisable?.Invoke(this, EventArgs.Empty);
        }

        [JSInvokable]
        public void DragStartFired(BlazorGridStackWidgetData widget)
        {
            OnDragStart?.Invoke(this, new BlazorGridStackWidgetEventArgs { Item = widget });
        }

        [JSInvokable]
        public void DragFired(BlazorGridStackWidgetData widget)
        {
            OnDrag?.Invoke(this, new BlazorGridStackWidgetEventArgs { Item = widget });
        }

        [JSInvokable]
        public void DragStopFired(BlazorGridStackWidgetData widget)
        {
            OnDragStop?.Invoke(this, new BlazorGridStackWidgetEventArgs { Item = widget });
        }

        [JSInvokable]
        public void DroppedFired(BlazorGridStackWidgetData? previousWidget, BlazorGridStackWidgetData newWidget)
        {
            OnDropped?.Invoke(this, new BlazorGridStackDroppedEventArgs { NewWidget = newWidget, PreviousWidget = previousWidget });
        }

        [JSInvokable]
        public void EnableFired()
        {
            OnEnable?.Invoke(this, EventArgs.Empty);
        }

        [JSInvokable]
        public void RemovedFired(BlazorGridStackWidgetData[] widgets)
        {
            OnRemoved?.Invoke(this, new BlazorGridStackWidgetListEventArgs { Items = widgets });
        }

        [JSInvokable]
        public void ResizeStartFired(BlazorGridStackWidgetData widget)
        {
            OnResizeStart?.Invoke(this, new BlazorGridStackWidgetEventArgs { Item = widget });
        }

        [JSInvokable]
        public void ResizeFired(BlazorGridStackWidgetData widget)
        {
            OnResize?.Invoke(this, new BlazorGridStackWidgetEventArgs { Item = widget });
        }

        [JSInvokable]
        public void ResizeStopFired(BlazorGridStackWidgetData widget)
        {
            OnResizeStop?.Invoke(this, new BlazorGridStackWidgetEventArgs { Item = widget });
        }
    }
}