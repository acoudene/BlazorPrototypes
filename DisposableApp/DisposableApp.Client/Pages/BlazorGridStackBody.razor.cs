using Alteva.Blazor.GridStack;
using Alteva.Blazor.GridStack.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DisposableApp.Client.Pages
{
    public partial class BlazorGridStackBody : ComponentBase, IDisposable, IAsyncDisposable
    {
#pragma warning disable CS8618
        private GridStackInterop GridJs { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }
#pragma warning restore CS8618

        [Parameter] public string Id { get; set; } = "grid1";

        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public BlazorGridStackBodyOptions? GridStackOptions { get; set; }

        /// <summary>
        /// Called when widgets are being added to a grid
        /// </summary>
        [Parameter] public EventCallback<BlazorGridStackWidgetListEventArgs> OnAdded { get; set; }

        /// <summary>
        /// Occurs when widgets change their position/size due to constrain or direct changes
        /// </summary>
        [Parameter] public EventCallback<BlazorGridStackWidgetListEventArgs> OnChange { get; set; }
        [Parameter] public EventCallback OnDisabled { get; set; }

        /// <summary>
        /// called while grid item is being dragged, for each new row/column value (not every pixel)
        /// </summary>
        [Parameter] public EventCallback<BlazorGridStackWidgetEventArgs> OnDrag { get; set; }

        /// <summary>
        /// called when grid item is starting to be dragged
        /// </summary>
        [Parameter] public EventCallback<BlazorGridStackWidgetEventArgs> OnDragStart { get; set; }

        /// <summary>
        /// called after the user is done moving the item, with updated DOM attributes.
        /// </summary>
        [Parameter] public EventCallback<BlazorGridStackWidgetEventArgs> OnDragStop { get; set; }

        /// <summary>
        /// called when an item has been dropped and accepted over a grid. If the item came from another grid, the previous widget node info will also be sent (but dom item long gone).
        /// </summary>
        [Parameter] public EventCallback<BlazorGridStackDroppedEventArgs> OnDropped { get; set; }
        [Parameter] public EventCallback OnEnable { get; set; }

        /// <summary>
        /// Called when items are being removed from the grid
        /// </summary>
        [Parameter] public EventCallback<BlazorGridStackWidgetListEventArgs> OnRemoved { get; set; }

        /// <summary>
        /// called while grid item is being resized, for each new row/column value (not every pixel)
        /// </summary>
        [Parameter] public EventCallback<BlazorGridStackWidgetEventArgs> OnResize { get; set; }

        /// <summary>
        /// called before the user starts resizing an item
        /// </summary>
        [Parameter] public EventCallback<BlazorGridStackWidgetEventArgs> OnResizeStart { get; set; }

        /// <summary>
        /// called after the user is done resizing the item, with updated DOM attributes.
        /// </summary>
        [Parameter] public EventCallback<BlazorGridStackWidgetEventArgs> OnResizeStop { get; set; }



        public bool IsInitialized { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                GridJs = new GridStackInterop(_jsRuntime);
                if (GridStackOptions is null) throw new InvalidOperationException($"{nameof(GridStackOptions)} is not initialized");
                await GridJs.Init(Id, GridStackOptions);

                GridJs.OnAdded += (sender, args) => OnAdded.InvokeAsync(args);
                GridJs.OnChange += (sender, args) => OnChange.InvokeAsync(args);
                GridJs.OnDisable += (sender, args) => OnDisabled.InvokeAsync(args);
                GridJs.OnDrag += (sender, args) => OnDrag.InvokeAsync(args);
                GridJs.OnDragStart += (sender, args) => OnDragStart.InvokeAsync(args);
                GridJs.OnDragStop += (sender, args) => OnDragStop.InvokeAsync(args);
                GridJs.OnDropped += (sender, args) => OnDropped.InvokeAsync(args);
                GridJs.OnEnable += (sender, args) => OnEnable.InvokeAsync(args);
                GridJs.OnRemoved += (sender, args) => OnRemoved.InvokeAsync(args);
                GridJs.OnResize += (sender, args) => OnResize.InvokeAsync();
                GridJs.OnResizeStart += (sender, args) => OnResizeStart.InvokeAsync(args);
                GridJs.OnResizeStop += (sender, args) => OnResizeStop.InvokeAsync(args);
                IsInitialized = true;
            }
        }

        /// <summary>
        /// // permet d'initialiser len D&D de la liste des FieldTemplate vers le FormTemplate
        /// </summary>
        /// <param name="cssClass">doit contennir le "." devant le nom de classe (ex: ".MaClass" ok, mais "MaClass" pas ok)</param>
        /// <returns></returns>
        public async Task SetupDragIn(string cssClass = "", bool clone = true)
        {
            await GridJs.SetupDragIn(cssClass, clone);
        }

        /// <summary>
        /// Creates new widget and returns it. Options is an object containing the fields x,y,width,height,etc...
        /// Parameters:
        /// Widget will be always placed even if result height is more than actual grid height. You need to use willItFit method before calling addWidget for additional check.
        /// </summary>
        /// <param name="options">widget position/size options (optional, and ignore if first param is already option)</param>
        /// <returns></returns>
        public Task<BlazorGridStackWidget> AddWidget(BlazorGridStackWidgetOptions options)
        {
            return GridJs.AddWidget(options);
        }

        public Task<BlazorGridStackWidget> AddSubGrid(BlazorGridStackWidgetOptions options)
        {
            return GridJs.AddSubGrid(options);
        }

        /// <summary>
        /// Creates new widget and returns it. Options is an object containing the fields x,y,width,height,etc...
        /// Parameters:
        /// Widget will be always placed even if result height is more than actual grid height. You need to use willItFit method before calling addWidget for additional check.
        /// </summary>
        /// <param name="options">id of the element to add</param>
        /// <returns></returns>
        public Task<BlazorGridStackWidget> AddWidget(string id)
        {
            return GridJs.AddWidget(id);
        }

        /// <summary>
        /// use before calling a bunch of addWidget() to prevent un-necessary relayouts in between (more efficient) and get a single event callback. You will see no changes until batchUpdate(false) is called.
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public Task BatchUpdate(bool? flag = null)
        {
            return GridJs.BatchUpdate(flag);
        }

        /// <summary>
        /// re-layout grid items to reclaim any empty space.
        /// </summary>
        /// <returns></returns>
        public Task Compact()
        {
            return GridJs.Compact();
        }

        /// <summary>
        /// Update current cell height (see - cellHeight options format). This method rebuilds an internal CSS stylesheet (unless optional update=false). Note: You can expect performance issues if call this method too often.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public Task CellHeight(int val, bool? update = null)
        {
            return GridJs.CellHeight(val, update);
        }

        /// <summary>
        /// Gets current cell width (grid width / # of columns).
        /// </summary>
        /// <returns></returns>
        public Task<int> CellWidth()
        {
            return GridJs.CellWidth();
        }

        /// <summary>
        /// set the number of columns in the grid. Will update existing widgets to conform to new number of columns, as well as cache the original layout so you can revert back to previous positions without loss. Requires gridstack-extra.css (or minimized version) for [2-11], else you will need to generate correct CSS (see https://github.com/gridstack/gridstack.js#change-grid-columns)
        /// </summary>
        /// <param name="column">Integer > 0 (default 12)</param>
        /// <param name="layout">specify the type of re-layout that will happen (position, size, etc...). Note: items will never be outside of the current column boundaries. default ('moveScale'). Ignored for 1 column. Possible values: 'moveScale' | 'move' | 'scale' | 'none' | (column: number, oldColumn: number, nodes: GridStackNode[], oldNodes: GridStackNode[]) => void. A custom function option takes new/old column count, and array of new/old positions. Note: new list may be partially already filled if we have a partial cache of the layout at that size (items were added later). If complete cache is present this won't get called at all.</param>
        /// <returns></returns>
        public Task Column(int column, string? layout = null)
        {
            return GridJs.Column(column, layout);
        }

        /// <summary>
        /// Destroys a grid instance.
        /// </summary>
        /// <param name="removeDOM">if false nodes and grid will not be removed from the DOM (Optional. Default true).</param>
        /// <returns></returns>
        public Task Destroy(bool? removeDOM = null)
        {
            return GridJs.Destroy(removeDOM);
        }

        /// <summary>
        /// Disables widgets moving/resizing
        /// </summary>
        /// <returns></returns>
        public Task Disable()
        {
            return GridJs.Disable();
        }

        /// <summary>
        /// Enables widgets moving/resizing
        /// </summary>
        /// <returns></returns>
        public Task Enable()
        {
            return GridJs.Enable();
        }

        /// <summary>
        /// Enables/disables widget moving (default: true), and setting the disableDrag grid option
        /// </summary>
        /// <param name="doEnable"></param>
        /// <returns></returns>
        public Task EnableMove(bool doEnable)
        {
            return GridJs.EnableMove(doEnable);
        }

        /// <summary>
        /// Enables/disables widget sizing (default: true), and setting the disableResize grid option
        /// </summary>
        /// <param name="doEnable"></param>
        /// <returns></returns>
        public Task EnableResize(bool doEnable)
        {
            return GridJs.EnableResize(doEnable);
        }

        /// <summary>
        /// get floating widgets
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetFloat()
        {
            return GridJs.GetFloat();
        }

        /// <summary>
        /// set floating widgets
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public Task SetFloat(bool? val = null)
        {
            return GridJs.SetFloat(val);
        }

        /// <summary>
        /// Gets current cell height.
        /// </summary>
        /// <returns></returns>
        public Task<int> GetCellHeight()
        {
            return GridJs.GetCellHeight();
        }

        /// <summary>
        /// Get the position of the cell under a pixel on screen.
        /// </summary>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="useOffset">if true, value will be based on offset vs position (Optional. Default false). Useful when grid is within position: relative element.</param>
        /// <returns>an object with properties x and y i.e. the column and row in the grid.</returns>
        public Task<BlazorGridCoordinates> GetCellFromPixel(int top, int left, bool? useOffset = null)
        {
            return GridJs.GetCellFromPixel(top, left, useOffset);
        }

        /// <summary>
        /// returns the number of columns in the grid.
        /// </summary>
        /// <returns></returns>
        public Task<int> GetColumn()
        {
            return GridJs.GetColumn();
        }

        /// <summary>
        /// Return list of GridItem HTML elements (excluding temporary placeholder) in DOM order, wether they are node items yet or not (looks by class)
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<BlazorGridStackWidgetData>> GetGridItems()
        {
            var res = GridJs.GetGridItems();
            return res;
        }

        /// <summary>
        /// returns current margin value (undefined if all 4 sides don't match).
        /// </summary>
        /// <returns></returns>
        public Task<int> GetMargin()
        {
            return GridJs.GetMargin();
        }

        /// <summary>
        /// Checks if specified area is empty.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Task<bool> IsAreaEmpty(int x, int y, int width, int height)
        {
            return GridJs.IsAreaEmpty(x, y, width, height);
        }

        /// <summary>
        /// load the widgets from a list (see save()). This will call update() on each (matching by id) or add/remove widgets that are not there. used to restore a grid layout for a saved layout list (see save()).
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="addAndRemove">Optional addAndRemove boolean (default true) or callback method can be passed to control if and how missing widgets can be added/removed, giving the user control of insertion.</param>
        /// <returns></returns>
        public Task Load(IEnumerable<BlazorGridStackWidgetOptions> layout, bool? addAndRemove = null)
        {
            return GridJs.Load(layout, addAndRemove);
        }

        /// <summary>
        /// est ce qu'un élément existe dans le DOM ?
        /// </summary>
        /// <param name="id">Id de l'élément</param>
        /// <returns></returns>
        public async Task<bool> IsExistingElementById(string id)
        {
            var res = await GridJs.IsExistingElementById(id);
            return res;
        }

        /// <summary>
        /// If you add elements to your gridstack container by hand, you have to tell gridstack afterwards to make them widgets. If you want gridstack to add the elements for you, use addWidget instead. Makes the given element a widget and returns it.
        /// </summary>
        /// <param name="id">element to convert to a widget</param>
        /// <returns></returns>
        public Task MakeWidget(string id)
        {
            return GridJs.MakeWidget(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task MakeSubGrid(string id, BlazorGridStackBodyOptions widgetOptions)
        {
            return GridJs.MakeSubGrid(id, widgetOptions);
        }

        /// <summary>
        /// gap between grid item and content (default?: 10). This will set all 4 sides and support the CSS formats below
        /// </summary>
        /// <param name="value">an integer (px)</param>
        /// <returns></returns>
        public Task Margin(int value)
        {
            return GridJs.Margin(value);
        }

        /// <summary>
        /// gap between grid item and content (default?: 10). This will set all 4 sides and support the CSS formats below
        /// </summary>
        /// <param name="value">
        /// - a string with possible units (ex: '5', '2em', '20px', '2rem')
        /// - string with space separated values (ex: '5px 10px 0 20px' for all 4 sides, or '5em 10em' for top/bottom and left/right pairs like CSS).</param>
        /// <returns></returns>
        public Task Margin(string value)
        {
            return GridJs.Margin(value);
        }

        /// <summary>
        /// Enables/Disables dragging by the user of specific grid element. If you want all items, and have it affect future items, use enableMove() instead. No-op for static grids. IF you are looking to prevent an item from moving (due to being pushed around by another during collision) use locked property instead.
        /// </summary>
        /// <param name="id">widget to modify</param>
        /// <param name="val">if true widget will be draggable.</param>
        /// <returns></returns>
        public Task Movable(string id, bool val)
        {
            return GridJs.Movable(id, val);
        }

        /// <summary>
        /// Removes widget from the grid.
        /// </summary>
        /// <param name="id">widget to remove.</param>
        /// <param name="removeDOM">if false node won't be removed from the DOM (Optional. Default true).</param>
        /// <param name="triggerEvent">if false (quiet mode) element will not be added to removed list and no 'removed' callbacks will be called (Default true).</param>
        /// <returns></returns>
        public Task RemoveWidget(string id, bool? removeDOM = null, bool? triggerEvent = true)
        {
            return GridJs.RemoveWidget(id, removeDOM, triggerEvent);
        }

        /// <summary>
        /// Removes all widgets from the grid.
        /// </summary>
        /// <param name="removeDOM">if false nodes won't be removed from the DOM (Optional. Default true).</param>
        /// <returns></returns>
        public Task RemoveAll(bool? removeDOM = null)
        {
            return GridJs.RemoveAll(removeDOM);
        }

        /// <summary>
        /// Enables/Disables user resizing of specific grid element. If you want all items, and have it affect future items, use enableResize() instead. No-op for static grids.
        /// </summary>
        /// <param name="id">widget to modify</param>
        /// <param name="val">if true widget will be resizable.</param>
        /// <returns></returns>
        public Task Resizable(string id, bool val)
        {
            return GridJs.Resizable(id, val);
        }

        /// <summary>
        /// saves the current layout returning a list of widgets for serialization which might include any nested grids.
        /// </summary>
        /// <param name="saveContent">if true (default) the latest html inside .grid-stack-content will be saved to GridStackWidget.content field, else it will be removed.</param>
        /// <returns></returns>
        public Task Save(bool? saveContent)
        {
            return GridJs.Save(saveContent);
        }

        /// <summary>
        /// Toggle the grid animation state. Toggles the grid-stack-animate class.
        /// </summary>
        /// <param name="doAnimate">if true the grid will animate.</param>
        /// <returns></returns>
        public Task SetAnimation(bool doAnimate)
        {
            return GridJs.SetAnimation(doAnimate);
        }

        /// <summary>
        /// Toggle the grid static state. Also toggle the grid-stack-static class.
        /// </summary>
        /// <param name="staticValue">if true the grid becomes static.</param>
        /// <returns></returns>
        public Task SetStatic(bool staticValue)
        {
            return GridJs.SetStatic(staticValue);
        }

        /// <summary>
        /// Updates widget position/size and other info. Note: if you need to call this on all nodes, use load() instead which will update what changed and more.
        /// </summary>
        /// <param name="id">widget to move (element or class string)</param>
        /// <param name="opts">updates all the possible item attributes passed in the structure (x, y, h, w, etc..). Only those set will be updated.</param>
        /// <returns></returns>
        public Task Update(string id, BlazorGridStackWidgetOptions opts)
        {
            return GridJs.Update(id, opts);
        }

        /// <summary>
        /// Returns true if the height of the grid will be less the vertical constraint. Always returns true if grid doesn't have height constraint.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="autoPosition"></param>
        /// <returns></returns>
        public Task<bool> WillItFit(int x, int y, int width, int height, bool autoPosition)
        {
            return GridJs.WillItFit(x, y, width, height, autoPosition);
        }

        #region IDisposable
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Do managed disposes
                    if (GridJs is not null)
                    {
                        GridJs.OnAdded -= (sender, args) => OnAdded.InvokeAsync(args);
                        GridJs.OnChange -= (sender, args) => OnChange.InvokeAsync(args);
                        GridJs.OnDisable -= (sender, args) => OnDisabled.InvokeAsync(args);
                        GridJs.OnDrag -= (sender, args) => OnDrag.InvokeAsync(args);
                        GridJs.OnDragStart -= (sender, args) => OnDragStart.InvokeAsync(args);
                        GridJs.OnDragStop -= (sender, args) => OnDragStop.InvokeAsync(args);
                        GridJs.OnDropped -= (sender, args) => OnDropped.InvokeAsync(args);
                        GridJs.OnEnable -= (sender, args) => OnEnable.InvokeAsync(args);
                        GridJs.OnRemoved -= (sender, args) => OnRemoved.InvokeAsync(args);
                        GridJs.OnResize -= (sender, args) => OnResize.InvokeAsync();
                        GridJs.OnResizeStart -= (sender, args) => OnResizeStart.InvokeAsync(args);
                        GridJs.OnResizeStop -= (sender, args) => OnResizeStop.InvokeAsync(args);                        
                    }

                    if (!_disposedAsync)
                    {
                        if (GridJs is not null)
                        {
                            GridJs.DisposeAsync()
                                .AsTask()
                                .ConfigureAwait(false)
                                .GetAwaiter()
                                .GetResult();
                        }
                    }

                    GridJs = null!;
                }

                // Do unmanaged disposes

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #region IAsyncDisposable
        private bool _disposedAsync = false;

        public async ValueTask DisposeAsync()
        {
            if (_disposedValue)
            {
                return;
            }

            if (_disposedAsync)
            {
                return;
            }

            await DisposeAsyncCore().ConfigureAwait(false);

            _disposedAsync = true;

            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (GridJs is not null)
            {
                GridJs.OnAdded -= (sender, args) => OnAdded.InvokeAsync(args);
                GridJs.OnChange -= (sender, args) => OnChange.InvokeAsync(args);
                GridJs.OnDisable -= (sender, args) => OnDisabled.InvokeAsync(args);
                GridJs.OnDrag -= (sender, args) => OnDrag.InvokeAsync(args);
                GridJs.OnDragStart -= (sender, args) => OnDragStart.InvokeAsync(args);
                GridJs.OnDragStop -= (sender, args) => OnDragStop.InvokeAsync(args);
                GridJs.OnDropped -= (sender, args) => OnDropped.InvokeAsync(args);
                GridJs.OnEnable -= (sender, args) => OnEnable.InvokeAsync(args);
                GridJs.OnRemoved -= (sender, args) => OnRemoved.InvokeAsync(args);
                GridJs.OnResize -= (sender, args) => OnResize.InvokeAsync();
                GridJs.OnResizeStart -= (sender, args) => OnResizeStart.InvokeAsync(args);
                GridJs.OnResizeStop -= (sender, args) => OnResizeStop.InvokeAsync(args);
            
                await GridJs.DisposeAsync().ConfigureAwait(false);
            }

            GridJs = null!;
        }
        #endregion
        #endregion
    }
}
