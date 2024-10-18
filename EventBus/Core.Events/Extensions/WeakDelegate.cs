using System.Reflection;

namespace Core.Events.Extensions;

/// <summary>
/// Permet de stocker un délégué associé à une référence faible, donc collectable par le GC
/// </summary>
public class WeakDelegate
{
  #region Attributs

  private readonly Type _delegateType;
  private readonly MethodInfo _method;
  private readonly WeakReference _targetRef;

  #endregion

  #region Constructeur

  /// <summary>
  /// Constructeur
  /// </summary>
  /// <param name="item">délégué pour lequel la référence est créé</param>
  /// <param name="target">cible du délégué</param>
  public WeakDelegate(Delegate item, object? target)
  {
    if (item == null)
      throw new ArgumentNullException("item");

    _targetRef = new WeakReference(target);
    _method = item.Method;
    _delegateType = item.GetType();
  }

  /// <summary>
  /// Constructeur
  /// </summary>
  /// <param name="item">délégué pour lequel la référence est créé</param>
  public WeakDelegate(Delegate item)
    : this(item, item.Target)
  {
  }

  #endregion

  #region Propriétés

  /// <summary>
  /// Obtient l'instance de la classe sur lequel le délégué est invoqué
  /// </summary>
  /// <remarks>peut être null, alors soit la référence n'existe plus soit il s'agissait d'une méthode statique</remarks>
  public object? Target
  {
    get
    {
      if (_targetRef is null || !_targetRef.IsAlive)
        return null;

      return _targetRef.Target;
    }
  }

  #endregion

  #region Méthodes publiques

  /// <summary>
  /// Retourne le délégué à invoquer
  /// </summary>
  /// <returns>le délégué à invoquer ou null si la référence faible n'a plus lieue d'être</returns>
  public Delegate? GetDelegate()
  {
    // S'il s'agit d'une méthode statique, on crée le délégué sans référence particulière
    if (_method.IsStatic)
      return Delegate.CreateDelegate(_delegateType, null, _method);
    // Si la référence existe encore, on crée un délégué lui étant associé
    else if (Target is not null)
      return Delegate.CreateDelegate(_delegateType, _targetRef.Target, _method);

    // En cas de référence null, on ne renvoie rien
    return null;
  }

  #endregion
}