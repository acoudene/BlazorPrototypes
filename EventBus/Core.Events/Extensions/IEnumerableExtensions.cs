namespace Core.Events.Extensions;

public static class IEnumerableExtensions
{
    /// <summary>
    /// Applique un délégué à chacun des éléments
    /// </summary>
    /// <typeparam name="T">type d'éléments</typeparam>
    /// <param name="value">IEnumerable étendu</param>
    /// <param name="action">délégué à appliquer sur tous les éléments</param>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> value, Action<T> action)
    {
        Assert.NotNull(value);
        Assert.NotNull(action);

        foreach (var item in value)
            action(item);

        return value;
    }

    /// <summary>
    /// Applique un délégué aux éléments répondant au prédicat
    /// </summary>
    /// <typeparam name="T">type d'éléments</typeparam>
    /// <param name="value">IEnumerable étendu</param>
    /// <param name="action">délégué à appliquer sur tous les éléments</param>
    /// <param name="predicate">prédicat de recherche</param>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> value, Action<T> action, Func<T, bool> predicate)
    {
        Assert.NotNull(value);
        Assert.NotNull(action);

        if (predicate == null)
            predicate = t => true;

        value.Where(predicate).ForEach(action);

        return value;
    }

    /// <summary>
    /// Met à plat un arbre
    /// </summary>
    /// <typeparam name="T">type d'objet</typeparam>
    /// <param name="value">enumerable à mettre à plat</param>
    /// <param name="childrenSelector">méthode récupérant les enfants</param>
    /// <returns>la liste mise à plat</returns>
    public static IEnumerable<T> Flatten<T>(this IEnumerable<T> value, Func<T, IEnumerable<T>> childrenSelector)
    {
        var result = value;

        if (value != null)
        {
            foreach (T element in value)
            {
                var children = childrenSelector(element).Flatten(childrenSelector);

                if (children != null)
                    result = result.Concat(children);
            }
        }

        return result;
    }

    /// <summary>
    /// Convertit les éléments
    /// </summary>
    /// <typeparam name="TInput">type d'objet</typeparam>
    /// <typeparam name="TOutput">type d'objet en sortie</typeparam>
    /// <param name="value">enumerable à convertir</param>
    /// <param name="action"></param>
    /// <returns>enumerable des éléments convertis</returns>
    public static IEnumerable<TOutput> Convert<TInput, TOutput>(this IEnumerable<TInput> value, Func<TInput, TOutput> action)
    {
        if (value == null)
            throw new ArgumentNullException("value");
        if (action == null)
            throw new ArgumentNullException("action");

        foreach (TInput item in value)
            yield return action(item);
    }
}