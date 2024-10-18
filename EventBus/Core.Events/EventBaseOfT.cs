namespace Core.Events;

/// <summary>
/// Classe d'événement générique du bus d'événements
/// </summary>
public abstract class EventBase<TData> : EventBase
{
  /// <summary>
  /// Obtient la donnée associée à l'événement
  /// </summary>
  public TData Data { get; private set; }

  /// <summary>
  /// Constructeur
  /// </summary>
  /// <param name="sender">objet à l'origine de l'événement</param>
  /// <param name="data">données de l'événement</param>
  protected EventBase(object sender, TData data)
      : base(sender)
  {
    Data = data;
  }
}