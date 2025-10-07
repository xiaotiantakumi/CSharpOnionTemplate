using Microsoft.AspNetCore.Mvc;

namespace Template.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(T result)
    {
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    protected IActionResult HandleError(string message)
    {
        return BadRequest(new { message });
    }
}
