namespace Core.Events.Extensions;

public static class ICollectionExtensions
{
    /// <summary>
    /// Ajoute tous les éléments à la collection
    /// </summary>
    /// <typeparam name="T">type d'éléments</typeparam>
    /// <param name="value">collection à modifier</param>
    /// <param name="items">éléments à ajouter à la collection</param>
    public static ICollection<T> AddRange<T>(this ICollection<T> value, IEnumerable<T> items)
    {
        if (value is null)
            throw new ArgumentNullException("value");
        if (items is null)
            return value;

        foreach (var item in items)
            value.Add(item);

        return value;
    }

    /// <summary>
    /// Supprime tous les items de la collection
    /// </summary>
    /// <typeparam name="T">type d'éléments</typeparam>
    /// <param name="value">collection à modifier</param>
    /// <param name="items">éléments à supprimer de la collection</param>
    public static ICollection<T> RemoveRange<T>(this ICollection<T> value, IEnumerable<T> items)
    {
        if (value is null)
            throw new ArgumentNullException("value");
        if (items is null)
            return value;

        foreach (var item in items)
            value.Remove(item);

        return value;
    }

    /// <summary>
    /// Supprime les éléments répondant au prédicat
    /// </summary>
    /// <typeparam name="T">type d'éléments</typeparam>
    /// <param name="value">collection à modifier</param>
    /// <param name="predicate">prédicat de suppression</param>        
    public static ICollection<T> RemoveWhere<T>(this ICollection<T> value, Func<T, bool> predicate)
    {
        if (predicate is null)
            throw new ArgumentNullException("predicate");

        return value.RemoveRange(value.Where(predicate).ToList());
    }

    /// <summary>
    /// Supprime le premier élément répondant au prédicat
    /// </summary>
    /// <typeparam name="T">type d'éléments</typeparam>
    /// <param name="value">collection à modifier</param>
    /// <param name="predicate">prédicat de suppression</param>        
    public static ICollection<T> RemoveFirst<T>(this ICollection<T> value, Func<T, bool> predicate)
    {
        if (predicate is null)
            throw new ArgumentNullException("predicate");

        T? element = value.FirstOrDefault(predicate);

        if (element is not null)
            value.Remove(element);

        return value;
    }

    /// <summary>
    /// Remplace les éléments de la collection par ceux passés en paramètre
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="items">Éléments à ajouter à la collection</param>
    /// <returns>Collection</returns>
    public static ICollection<T> ReplaceAll<T>(this ICollection<T> value, IEnumerable<T> items)
    {
        if (value is null)
            throw new ArgumentNullException("value");

        value.Clear();

        return value.AddRange(items);
    }
}