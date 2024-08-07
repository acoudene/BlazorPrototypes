using FieldValidationBlazorApp.Client.ViewObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace FieldValidationBlazorApp.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public class StarshipValidationController : ControllerBase
{
  private readonly ILogger<StarshipValidationController> logger;

  public StarshipValidationController(
      ILogger<StarshipValidationController> logger)
  {
    this.logger = logger;
  }

  static readonly string[] scopeRequiredByApi = new[] { "API.Access" };

  [HttpPost]
  public async Task<IActionResult> Post(Starship model)
  {
    //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

    try
    {
      if (model.Classification == "Defense" &&
          string.IsNullOrEmpty(model.Description))
      {
        ModelState.AddModelError(nameof(model.Description),
            "For a 'Defense' ship " +
            "classification, 'Description' is required.");
      }
      else
      {
        logger.LogInformation("Processing the form asynchronously");

        // async ...

        return Ok(ModelState);
      }
    }
    catch (Exception ex)
    {
      logger.LogError("Validation Error: {Message}", ex.Message);
    }

    return BadRequest(ModelState);
  }
}
