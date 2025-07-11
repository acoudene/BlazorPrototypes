using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BlazorApp.Areas.Identity;

/// <summary>
/// Class to adapt authentication state mechanism for Identity
/// </summary>
/// <typeparam name="TUser"></typeparam>
public class RevalidatingIdentityAuthenticationStateProvider<TUser>
    : RevalidatingServerAuthenticationStateProvider where TUser : class
{
  private readonly IServiceScopeFactory _scopeFactory;
  private readonly IdentityOptions _options;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="loggerFactory"></param>
  /// <param name="scopeFactory"></param>
  /// <param name="optionsAccessor"></param>
  public RevalidatingIdentityAuthenticationStateProvider(
      ILoggerFactory loggerFactory,
      IServiceScopeFactory scopeFactory,
      IOptions<IdentityOptions> optionsAccessor)
      : base(loggerFactory)
  {
    _scopeFactory = scopeFactory;
    _options = optionsAccessor.Value;
  }

  /// <summary>
  /// Interval between revalidations
  /// </summary>
  protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

  /// <summary>
  /// Validate authentication
  /// </summary>
  /// <param name="authenticationState"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override async Task<bool> ValidateAuthenticationStateAsync(
      AuthenticationState authenticationState, CancellationToken cancellationToken)
  {
    // Get the user manager from a new scope to ensure it fetches fresh data
    var scope = _scopeFactory.CreateScope();
    try
    {
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
      return await ValidateSecurityStampAsync(userManager, authenticationState.User);
    }
    finally
    {
      if (scope is IAsyncDisposable asyncDisposable)
      {
        await asyncDisposable.DisposeAsync();
      }
      else
      {
        scope.Dispose();
      }
    }
  }
  private async Task<bool> ValidateSecurityStampAsync(UserManager<TUser> userManager, ClaimsPrincipal principal)
  {
    var user = await userManager.GetUserAsync(principal);
    if (user == null)
    {
      return false;
    }
    else if (!userManager.SupportsUserSecurityStamp)
    {
      return true;
    }
    else
    {
      var principalStamp = principal.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType);
      var userStamp = await userManager.GetSecurityStampAsync(user);
      return principalStamp == userStamp;
    }
  }
}
