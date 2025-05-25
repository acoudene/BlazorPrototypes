using Core.ViewObjects;

namespace Core.ViewModels.Offline;

public record ViewObjectStateContainer<TViewObject>
  where TViewObject : IViewObject
{
  public ViewObjectState State { get; }
  
  public TViewObject ViewObject { get; }

  public ViewObjectStateContainer(TViewObject viewObject, ViewObjectState state = ViewObjectState.Unchanged)
  {
    ViewObject = viewObject ?? throw new ArgumentNullException(nameof(viewObject));
    State = state;
  }
}
