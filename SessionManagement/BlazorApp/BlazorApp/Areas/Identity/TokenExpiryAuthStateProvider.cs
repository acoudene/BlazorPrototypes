using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace BlazorApp.Areas.Identity;

/// <summary>
/// Provider to manage authentication by cycle to ensure logout outside current tab if multiple tabs are open
/// </summary>
public class TokenExpiryAuthStateProvider : RevalidatingServerAuthenticationStateProvider
{
  /// <summary>
  /// Interval between revalidation
  /// </summary>
  protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(10);

  private readonly LoginState _loginState;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="loggerFactory"></param>
  /// <param name="loginState"></param>
  public TokenExpiryAuthStateProvider(ILoggerFactory loggerFactory, LoginState loginState)
    : base(loggerFactory)
  {
    if (loginState is null)
      throw new ArgumentNullException(nameof(loginState));

    _loginState = loginState;
  }

  /// <summary>
  /// Validation mechanism
  /// </summary>
  /// <param name="authenticationState"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
  {
    if (authenticationState is null)
      return Task.FromResult(false);

    var user = authenticationState.User;
    if (user is null)
      return Task.FromResult(false);

    return Task.FromResult(_loginState.IsUserLoggedIn(user));
  }
}