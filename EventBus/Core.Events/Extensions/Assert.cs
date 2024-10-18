using System.Diagnostics;

namespace Core.Events.Extensions;

/// <summary>
/// Defines assertion methods
/// </summary>
public static class Assert
{
    // #warning FDE Voir l'impact de récupérer le nom du paramètre par reflection posant problème lors de la levée de l'exception

    #region Public methods

    /// <summary>
    /// Checks if value is null
    /// </summary>
    /// <typeparam name="T">type of value parameter</typeparam>
    /// <param name="value">object to check</param>
    [DebuggerStepThrough]
    public static void NotNull<T>(T value, string? strDescriptionTxt_P = null)
        where T : class
    {
        if (value == null)
        {
            throw strDescriptionTxt_P.IsNotNullNorEmpty() ? new ArgumentException(strDescriptionTxt_P) : new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Checks if value is zero
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    [DebuggerStepThrough]
    public static void NotZero(long value, string? strDescriptionTxt_P = null)
    {
        if (value == 0)
        {
            throw strDescriptionTxt_P.IsNotNullNorEmpty() ? new ArgumentException(strDescriptionTxt_P) : new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Checks if value is positive.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    [DebuggerStepThrough]
    public static void IsPositive(long value, string? strDescriptionTxt_P = null)
    {
        if (value <= 0)
        {
            throw strDescriptionTxt_P.IsNotNullNorEmpty() ? new ArgumentException(strDescriptionTxt_P) : new ArgumentNullException(nameof(value), "Value must be strictly positive.");
        }
    }

    /// <summary>
    /// Checks if value is positive.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    [DebuggerStepThrough]
    public static void IsPositive(long? value, string? strDescriptionTxt_P = null)
    {
        if (!value.HasValue)
        {
            throw strDescriptionTxt_P.IsNotNullNorEmpty() ? new ArgumentException(strDescriptionTxt_P) : new ArgumentNullException(nameof(value), "Value is undefined.");
        }

        IsPositive(value.Value, strDescriptionTxt_P);
    }

    /// <summary>
    /// Checks if a string is null or empty
    /// </summary>
    /// <param name="value">string to check</param>
    [DebuggerStepThrough]
    public static void NotNullOrEmpty(string value, string? strDescriptionTxt_P = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw strDescriptionTxt_P.IsNotNullNorEmpty() ? new ArgumentException(strDescriptionTxt_P) : new ArgumentException("Value cannot be null or empty");
        }
    }

    /// <summary>
    /// Checks if a string is null or empty or contains only whitespaces
    /// </summary>
    /// <param name="value">string to check</param>
    [DebuggerStepThrough]
    public static void NotNullOrWhiteSpace(string value, string? strDescriptionTxt_P = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw strDescriptionTxt_P.IsNotNullNorEmpty() ? new ArgumentException(strDescriptionTxt_P) : new ArgumentException("Value cannot be null or empty or contain only whitespaces");
        }
    }

    /// <summary>
    /// Checks if the collection is not null and if it does not contain any null elements
    /// </summary>
    /// <typeparam name="T">type of elements in the collection</typeparam>
    /// <param name="values">collection to check</param>
    [DebuggerStepThrough]
    public static void NotNullOrNullElements<T>(IEnumerable<T> values)
        where T : class
    {
        NotNull(values);
        NotNullElements(values);
    }

    /// <summary>
    /// Checks if the collection does not contain any null elements
    /// </summary>
    /// <typeparam name="TKey">type of the keys</typeparam>
    /// <typeparam name="TValue">type of the values</typeparam>
    /// <param name="values">collection to check</param>
    [DebuggerStepThrough]
    public static void NotNullElements<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> values)
      where TKey : class
      where TValue : class
    {
        if (values != null)
            CheckNotNullElements(values);
    }

    /// <summary>
    /// Checks if the collection does not contain any null elements
    /// </summary>
    /// <typeparam name="T">type of elements of the collection</typeparam>
    /// <param name="values">collection to check</param>
    [DebuggerStepThrough]
    public static void NotNullElements<T>(IEnumerable<T> values)
        where T : class
    {
        if (values != null)
            CheckNotNullElements(values);
    }

    /// <summary>
    /// Checks if a condition is true
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <param name="parameterName"></param>
    [DebuggerStepThrough]
    public static void IsTrue(bool bCondition, string descriptionTxt)
    {
        if (bCondition != true)
            throw new InvalidOperationException(descriptionTxt);
    }

    public static void IsTrue(bool bCondition)
    {
        if (bCondition != true)
            throw new InvalidOperationException("assertion failed");
    }

    /// <summary>
    /// Checks if a collection is not null and if it has elements
    /// </summary>
    /// <typeparam name="T">type of elements of the collection</typeparam>
    /// <param name="values">collection to check</param>
    [DebuggerStepThrough]
    public static void HasItems<T>(ICollection<T> values)
        where T : class
    {
        if (values == null || values.Count == 0)
            throw new ArgumentNullException(nameof(values));
    }

    #endregion Public methods

    #region Private methods

    private static void CheckNotNullElements<T>(IEnumerable<T> values)
        where T : class
    {
        if (values.Any(v => v == null))
            throw new ArgumentNullException(nameof(values), "Value cannot contain null values");
    }

    private static void CheckNotNullElements<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> values)
      where TKey : class
      where TValue : class
    {
        if (values.Any(kv => kv.Key == null || kv.Value == null))
            throw new ArgumentNullException(nameof(values), "Value cannot contain null values");
    }

    #endregion Private methods
}