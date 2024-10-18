namespace Core.Events.Extensions;

/// <summary>
/// Defines extension methods to the String type
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Indique si une chaîne n'est ni nulle ni vide
    /// </summary>
    /// <param name="value">chaîne à tester</param>
    /// <returns>true si la chaîne n'est ni nulle ni vide, false sinon</returns>
    public static bool IsNotNullNorEmpty(this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }

    //$2=======================================================================
    //$2 Purpose: return true if string is null or empty
    //$2 Author : CBO                                       Date : 2012/01/23
    //$2=======================================================================
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Concatène une chaîne avec une autre
    /// </summary>
    /// <param name="value">chaîne à concaténer</param>
    /// <param name="str1">chaine à concaténer</param>
    /// <returns>chaîne concaténée</returns>
    public static string Concatenate(this string value, string str1)
    {
        return string.Concat(value, str1);
    }

    /// <summary>
    /// Concatène une chaîne avec d'autres
    /// </summary>
    /// <param name="value">chaîne à concaténer</param>
    /// <param name="values">chaines à concaténer</param>
    /// <returns>chaîne concaténée</returns>
    public static string Concatenate(this string value, params string[] values)
    {
        return string.Concat(values);
    }

    /// <summary>
    /// Concatène une chaîne avec des objets
    /// </summary>
    /// <param name="value">chaîne à concaténer</param>
    /// <param name="values">objets à concaténer</param>
    /// <returns>chaîne concaténée</returns>
    public static string Concatenate(this string value, params object[] values)
    {
        return string.Concat(values);
    }

    /// <summary>
    /// Concatène une chaîne avec un objet
    /// </summary>
    /// <param name="value">chaîne à concaténer</param>
    /// <param name="obj1">chaine à concaténer</param>
    /// <returns>chaîne concaténée</returns>
    public static string Concatenate(this string value, object obj1)
    {
        return string.Concat(obj1);
    }

    /// <summary>
    /// Formate la chaîne avec les éléments fournis
    /// </summary>
    /// <param name="value">chaîne de formatage</param>
    /// <param name="values">objets à insérer dans la chaîne de formatage</param>
    /// <returns>la chaîne formatée</returns>
    public static string FormatString(this string value, params object[] values)
    {
        return string.Format(value, values);
    }

    /// <summary>
    /// get chars at left
    /// </summary>
    public static string Left(this string value, int iCount)
    {
        Assert.IsTrue(iCount >= 0, "count must >= 0");
        if (iCount == 0) return string.Empty;
        string t = value.Substring(0, iCount);
        return t;
    }

    /// <summary>
    /// get chars at right
    /// </summary>
    public static string Right(this string value, int iCount)
    {
        Assert.IsTrue(iCount >= 0, "count must >= 0");
        if (iCount == 0) return string.Empty;
        string t = value.Substring(Math.Max(0, value.Length - iCount), iCount);
        return t;
    }

}