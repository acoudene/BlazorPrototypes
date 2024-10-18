namespace Core.Events;

/// <summary>
/// Classe de base des événements du bus d'événements
/// </summary>
public abstract class EventBase
{
  #region Propriétés

  /// <summary>
  /// Obtient l'élément à l'origine de l'événement
  /// </summary>
  public object Sender { get; internal set; }

  #endregion

  #region Contructeur

  /// <summary>
  /// Constructeur
  /// </summary>
  /// <param name="sender">objet à l'origine de l'événement</param>
  protected EventBase(object sender)
  {
    if (sender == null)
      throw new ArgumentNullException("sender");

    Sender = sender;
  }

  #endregion
}
