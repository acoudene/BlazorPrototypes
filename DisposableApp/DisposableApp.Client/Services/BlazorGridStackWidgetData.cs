namespace Alteva.Blazor.GridStack.Models
{
    public class BlazorGridStackWidgetData
    {

        public string Content { get; set; } = string.Empty;
        public int H { get; set; }
        public int W { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string? ClassName { get; set; }

        // id du widget dans gridstack.js
        public string Id { get; set; } = string.Empty;

        // id du fieldtemplate affiché dans le widget
        public string FieldTemplateId { get; set; } = string.Empty;

        /// <summary>
        ///  est ce que le widget est un widget de type "FieldTemplate", c'est à dire qu'il est issu de la liste des widget de fieldtemplate ?
        ///  (cf FieldsTemplatesList.razor avec ModeDragAndDrop=true)
        /// </summary>
        public bool IsFieldTemplate { get; set; }

        public override string ToString()
        {
            //return $"Content={Content} H={H} " +
            //    $"W={W}    X={X}    Y={Y}    ClassName={ClassName}    " +
            //    $"Id={Id}";

            return $"H={H} " +
              $"W={W}    X={X}    Y={Y} " +
              $"Id={Id}" + $"FieldTemplateId={FieldTemplateId}";
        }
    }
}
