using System.Security.Claims;

namespace BlazorApp.Areas.Identity;

/// <summary>
/// Mangage login state just for revalidating identity process
/// </summary>
public class LoginState
{
  private readonly Dictionary<string, ClaimsPrincipal> _loggedInUsers = new Dictionary<string, ClaimsPrincipal>();

  private static string? GetUserId(ClaimsPrincipal user)
  {
    if (user is null || user.Identity is null || user.Claims is null)
      return null;

    if (!user.Identity.IsAuthenticated)
      return null;

    return user.Claims
      .Where(c => c.Type.Equals("sid"))
      .Select(c => c.Value)
      .FirstOrDefault();
  }

  /// <summary>
  /// Add user
  /// </summary>
  /// <param name="user"></param>
  public void AddUser(ClaimsPrincipal user)
  {
    if (user is null)
      return;

    string? id = GetUserId(user);
    if (string.IsNullOrWhiteSpace(id))
      return;

    if (_loggedInUsers.ContainsKey(id))
      _loggedInUsers[id] = user;
    else
      _loggedInUsers.TryAdd(id, user);
  }

  /// <summary>
  /// Remove user
  /// </summary>
  /// <param name="user"></param>
  public void RemoveUser(ClaimsPrincipal user)
  {
    if (user is null)
      return;

    string? id = GetUserId(user);
    if (string.IsNullOrWhiteSpace(id))
      return;

    if (_loggedInUsers.ContainsKey(id))
      _loggedInUsers.Remove(id);
  }

  /// <summary>
  /// Is user logged in
  /// </summary>
  /// <param name="user"></param>
  /// <returns></returns>
  public bool IsUserLoggedIn(ClaimsPrincipal user)
  {
    if (user is null)
      return false;

    string? id = GetUserId(user);
    if (string.IsNullOrWhiteSpace(id))
      return false;

    return _loggedInUsers.ContainsKey(id);
  }
}