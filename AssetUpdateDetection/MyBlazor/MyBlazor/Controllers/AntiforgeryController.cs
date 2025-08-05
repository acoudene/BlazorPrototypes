using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace MyBlazor.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AntiforgeryController : ControllerBase
{
  private readonly IAntiforgery _antiforgery;

  public AntiforgeryController(IAntiforgery antiforgery)
  {
    _antiforgery = antiforgery;
  }

  [HttpGet]
  public IActionResult Get()
  {
    var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
    return Ok(new { token = tokens.RequestToken });
  }
}
