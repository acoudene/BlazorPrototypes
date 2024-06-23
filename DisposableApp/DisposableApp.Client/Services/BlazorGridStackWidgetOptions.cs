namespace Alteva.Blazor.GridStack.Models
{
    /// <summary>
    /// https://github.com/gridstack/gridstack.js/tree/master/doc#item-options
    /// </summary>
    public class BlazorGridStackWidgetOptions
    {
        // id du widget dans gridstack.js
        public object? Id { get; set; }

        // id du fieldtemplate affiché dans le widget
        public object? FieldTemplateId { get; set; }

        /// <summary>
        ///  est ce que le widget est un widget de type "FieldTemplate", c'est à dire qu'il est issu de la liste des widget de fieldtemplate ?
        ///  (cf FieldsTemplatesList.razor avec ModeDragAndDrop=true)
        /// </summary>
        public bool IsFieldTemplate { get; set; }

        // tells to ignore x and y attributes and to place element to the first available position. Having either one missing will also do that.
        public bool? AutoPosition { get; set; }
        // element position in column/row. Note: if one is missing this will autoPosition the item
        public int? X { get; set; }
        public int? Y { get; set; }
        // element size in column/row (default 1x1)
        public int? W { get; set; }
        public int? H { get; set; }
        //  element constraints in column/row (default none)
        public int? MaxW { get; set; }
        public int? MinW { get; set; }
        public int? MaxH { get; set; }
        public int? MinH { get; set; }
        // locked - means another widget wouldn't be able to move it during dragging or resizing.
        // The widget can still be dragged or resized by the user. You need to add noResize and noMove attributes to completely lock the widget.
        public bool? Locked { get; set; }
        // disable element resizing
        public bool? NoResize { get; set; }
        // disable element moving
        public bool? NoMove { get; set; }
        public string? ResizeHandles { get; set; }
        public string? Content { get; set; }
        // GridStackOptions - optional nested grid options and list of children
        public BlazorGridStackBodyOptions? SubGridOpts { get; set; }

        public void Assign(BlazorGridStackWidgetData item)
        {
            if (item is null) return;
            X = item.X; Y = item.Y; W = item.W; H = item.H;
        }
    }
}
