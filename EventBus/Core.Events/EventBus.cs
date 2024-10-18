using Core.Events.Extensions;

namespace Core.Events;

/// <summary>
/// Définit un bus d'événements
/// </summary>
/// <remarks>toutes les références sont des références faibles</remarks>
public class EventBus : IEventBus
{
  #region Attributs

  private readonly Dictionary<Type, List<Subscription>> _subscribees = new Dictionary<Type, List<Subscription>>();

  #endregion

  #region IEventBus Members

  /// <summary>
  /// Publie un événement sur le bus
  /// </summary>
  /// <typeparam name="TEvent">type d'événement à publier</typeparam>
  /// <param name="item">événement à publier</param>
  /// <returns>l'instance du bus pour une écriture fluide</returns>
  public IEventBus Publish<TEvent>(TEvent item) where TEvent : EventBase
  {
    if (item == null)
      throw new ArgumentNullException("item");

    // Récupère les callbacks
    var actions = GetCallbacks<TEvent>(item);

    // Les exécutent s'il y en a
    if (actions != null)
      actions.ForEach(a => a(item));

    return this;
  }

  /// <summary>
  /// S'abonne à un événement publié sur le bus
  /// </summary>
  /// <typeparam name="TEvent">type d'événement pour lequel l'abonnement est fait</typeparam>
  /// <param name="callback">handler utilisé lors de la levée de l'événement</param>
  /// <param name="throwOnce">indique si l'abonnement est maintenu ou non après le premier déclenchement</param>
  /// <param name="target">cible de l'abonnement</param>
  /// <returns>l'instance du bus pour une écriture fluide</returns>
  /// <remarks>Attention en cas d'utilisation de lambda à ce que ce ne soit pas un champs statique (=> la lambda doit faire référence à la classe qui la contient</remarks>
  public IEventBus Subscribe<TEvent>(Action<TEvent> callback, bool throwOnce = false, object? target = null) where TEvent : EventBase
  {
    return Subscribe<TEvent>(callback, null, ThreadingStrategy.Publisher, throwOnce: throwOnce, target: target);
  }

  /// <summary>
  /// S'abonne à un événement publié sur le bus
  /// </summary>
  /// <typeparam name="TEvent">type d'événement pour lequel l'abonnement est fait</typeparam>
  /// <param name="callback">handler utilisé lors de la levée de l'événement</param>
  /// <param name="filter">filtre permettant de savoir si le handler doit être exécuté ou non</param>
  /// <param name="throwOnce">indique si l'abonnement est maintenu ou non après le premier déclenchement</param>
  /// <param name="target">cible de l'abonnement</param>
  /// <returns>l'instance du bus pour une écriture fluide</returns>
  /// <remarks>Attention en cas d'utilisation de lambda à ce que ce ne soit pas un champs statique (=> la lambda doit faire référence à la classe qui la contient</remarks>
  public IEventBus Subscribe<TEvent>(Action<TEvent> callback, Predicate<TEvent> filter, bool throwOnce = false, object? target = null) where TEvent : EventBase
  {
    return Subscribe<TEvent>(callback, filter, ThreadingStrategy.Publisher, throwOnce: throwOnce, target: target);
  }

  /// <summary>
  /// S'abonne à un événement publié sur le bus
  /// </summary>
  /// <typeparam name="TEvent">type d'événement pour lequel l'abonnement est fait</typeparam>
  /// <param name="callback">handler utilisé lors de la levée de l'événement</param>
  /// <param name="strategy">stratégie de threading lors de l'exécution du callback</param>
  /// <param name="throwOnce">indique si l'abonnement est maintenu ou non après le premier déclenchement</param>
  /// <param name="target">cible de l'abonnement</param>
  /// <returns>l'instance du bus pour une écriture fluide</returns>
  /// <remarks>Attention en cas d'utilisation de lambda à ce que ce ne soit pas un champs statique (=> la lambda doit faire référence à la classe qui la contient</remarks>
  public IEventBus Subscribe<TEvent>(Action<TEvent> callback, ThreadingStrategy strategy, bool throwOnce = false, object? target = null) where TEvent : EventBase
  {
    return Subscribe<TEvent>(callback, null, strategy, throwOnce: throwOnce, target: target);
  }

  /// <summary>
  /// S'abonne à un événement publié sur le bus
  /// </summary>
  /// <typeparam name="TEvent">type d'événement pour lequel l'abonnement est fait</typeparam>
  /// <param name="callback">handler utilisé lors de la levée de l'événement</param>
  /// <param name="filter">filtre permettant de savoir si le handler doit être exécuté ou non</param>
  /// <param name="strategy">stratégie de threading lors de l'exécution du callback</param>
  /// <param name="throwOnce">indique si l'abonnement est maintenu ou non après le premier déclenchement</param>
  /// <param name="target">cible de l'abonnement</param>
  /// <returns>l'instance du bus pour une écriture fluide</returns>
  /// <remarks>Attention en cas d'utilisation de lambda à ce que ce ne soit pas un champs statique (=> la lambda doit faire référence à la classe qui la contient</remarks>
  public IEventBus Subscribe<TEvent>(Action<TEvent> callback, Predicate<TEvent>? filter, ThreadingStrategy strategy, bool throwOnce = false, object? target = null) where TEvent : EventBase
  {
    if (callback == null)
      throw new ArgumentNullException("callback");

    if (target == null && callback.Target == null)
      throw new ArgumentException("Callback's target must have a reference, it cannot be a static method unless a target is provided", "callback");

    lock (_subscribees)
    {
      Type eventType = typeof(TEvent);

      if (!_subscribees.ContainsKey(eventType))
        _subscribees[eventType] = new List<Subscription>(1);

      _subscribees[eventType].Add(new Subscription(callback, filter, strategy, throwOnce, target ?? callback.Target));
    }

    return this;
  }

  /// <summary>
  /// Permet de se désabonner de tous les événements du bus
  /// </summary>
  /// <param name="target">cible à supprimer des listes d'invocation</param>
  public void Unsubscribe(object target)
  {
    if (target == null)
      throw new ArgumentNullException("target");

    lock (_subscribees)
    {
      // Supprime toutes les actions référençant la cible                    
      _subscribees.ForEach(kv => kv.Value.RemoveWhere(s => s.Target != null && s.Target.Equals(target)));

      // Supprime les événements pour lesquels il n'y a plus d'actions;
      _subscribees.RemoveWhere(kv => kv.Value.Count == 0);
    }
  }

  /// <summary>
  /// Permet de se désabonner d'un événement particulier du bus
  /// </summary>
  /// <typeparam name="TEvent">type d'événement pour lequel l'abonnement est défait</typeparam>
  /// <param name="callback">callback à supprimer de la liste d'invocations du bus</param>
  public void Unsubscribe<TEvent>(Action<TEvent> callback)
  {
    if (callback == null)
      throw new ArgumentNullException("callback");

    lock (_subscribees)
    {
      List<Subscription>? subscriptions;

      if (_subscribees.TryGetValue(typeof(TEvent), out subscriptions))
      {
        // Supprime le callback
        subscriptions.RemoveFirst(s => (Action<TEvent>?)s.Action == callback);

        // Supprime l'événement s'il n'y a plus d'actions
        if (subscriptions.Count == 0)
          _subscribees.Remove(typeof(TEvent));
      }
    }
  }

  /// <summary>
  /// Permet de se désabonner d'un événement particulier du bus pour une cible donnée
  /// </summary>
  /// <typeparam name="TEvent">type d'événement pour lequel l'abonnement est défait</typeparam>
  /// <param name="target">cible à supprimer de la liste d'invocations du bus</param>
  public void Unsubscribe<TEvent>(object target)
  {
    if (target == null)
      throw new ArgumentNullException("target");

    lock (_subscribees)
    {
      List<Subscription>? subscriptions;

      if (_subscribees.TryGetValue(typeof(TEvent), out subscriptions))
      {
        // Supprime le callback de la cible
        subscriptions.RemoveFirst(s => s.Target == target);

        // Supprime l'événement s'il n'y a plus d'actions
        if (subscriptions.Count == 0)
          _subscribees.Remove(typeof(TEvent));
      }
    }
  }

  #endregion

  #region Méthodes privées

  /// <summary>
  /// Retourne la liste des callbacks à invoquer en fonction de la stratégie de threading
  /// </summary>
  /// <typeparam name="TEvent">type de l'événement</typeparam>
  /// <param name="item">instance de l'événement</param>
  /// <returns>la liste des callbacks à invoquer</returns>
  private List<Action<TEvent>>? GetCallbacks<TEvent>(TEvent item) where TEvent : EventBase
  {
    if (item == null)
      throw new ArgumentNullException("item");

    Type eventType = typeof(TEvent);
    List<Action<TEvent>> actions;

    lock (_subscribees)
    {
      // On coupe court s'il n'y a aucune souscription
      if (!_subscribees.ContainsKey(eventType))
        return null;

      List<Subscription> subscriptions = _subscribees[eventType];
      actions = new List<Action<TEvent>>(subscriptions.Count);

      for (int i = subscriptions.Count - 1; i > -1; --i)
      {
        Subscription sub = subscriptions[i];

        Delegate? action = sub.Action;
        // Teste pour savoir si le délégué d'action est utilisable => la référence est encore en vie
        if (action != null)
        {
          Delegate? filter = sub.Filter;

          // Teste l'existence d'un filtre, et s'il existe on l'exécute pour savoir si le callback doit être appelé
          if (filter == null || ((Predicate<TEvent>)filter)(item))
          {
            if (sub.ThreadingStrategy == ThreadingStrategy.Publisher)
              actions.Add(new Action<TEvent>(evt => action.DynamicInvoke(evt)));
            else if (sub.ThreadingStrategy == ThreadingStrategy.Background)
              actions.Add(new Action<TEvent>(evt => System.Threading.ThreadPool.QueueUserWorkItem(o => action.DynamicInvoke(o), evt)));
            else if (sub.ThreadingStrategy == ThreadingStrategy.UI)
              actions.Add(new Action<TEvent>(evt => action.DynamicInvoke(evt)) /*DispatcherHelper.SafeInvoke(action, evt))*/); // Warning here

            // Supprime l'abonnement s'il est configuré pour n'être déclenché qu'une fois
            if (sub.ThrowOnce)
              subscriptions.RemoveAt(i);
          }
        }
        else
          // La référence est morte, on supprime la souscription
          subscriptions.RemoveAt(i);
      }

      // S'il n'y a plus de souscription, on supprime l'événement
      if (subscriptions.Count == 0)
        _subscribees.Remove(eventType);
    }

    // Retourne les éléments pour qu'ils soient appelés dans leur ordre d'ajout initial
    actions.Reverse();

    return actions;
  }

  #endregion
}
