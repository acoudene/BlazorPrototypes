using Core.Events.Extensions;

namespace Core.Events;

/// <summary>
/// Définit un abonnement à un événement du bus d'événements
/// </summary>
internal class Subscription
{
  #region Attributs

  private readonly WeakDelegate _callback;
  private readonly WeakDelegate? _filter;
  private readonly ThreadingStrategy _threadingStrategy;
  private readonly bool _throwOnce;

  #endregion

  #region Constructeurs

  /// <summary>
  /// Constructeur
  /// </summary>
  /// <param name="callback">callback à invoquer</param>
  /// <param name="filter">filtre permettant de savoir si le callback doit être invoqué</param>
  /// <param name="threadingStrategy">stratégie de threading d'exécution du callback</param>
  public Subscription(Delegate callback, Delegate? filter, ThreadingStrategy threadingStrategy, bool throwOnce, object? target)
  {
    if (callback == null)
      throw new ArgumentNullException("callback");

    _callback = new WeakDelegate(callback, target);

    if (filter is not null)
      _filter = new WeakDelegate(filter, target);

    _threadingStrategy = threadingStrategy;

    _throwOnce = throwOnce;
  }

  #endregion

  #region Propriétés

  /// <summary>
  /// Obtient l'instance cible du callback
  /// </summary>
  public object? Target
  {
    get { return _callback.Target; }
  }

  /// <summary>
  /// Obtient le délégué du callback
  /// </summary>
  public Delegate? Action
  {
    get { return _callback.GetDelegate(); }
  }

  /// <summary>
  /// Obtient le délégué du filtre
  /// </summary>
  public Delegate? Filter
  {
    get { return (_filter != null) ? _filter.GetDelegate() : null; }
  }

  /// <summary>
  /// Obtient la stratégie de threading
  /// </summary>
  public ThreadingStrategy ThreadingStrategy
  {
    get { return _threadingStrategy; }
  }

  /// <summary>
  /// Indique si l'abonnement est maintenu après le premier déclenchement
  /// </summary>
  public bool ThrowOnce
  {
    get { return _throwOnce; }
  }

  #endregion
}
