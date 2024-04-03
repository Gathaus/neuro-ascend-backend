using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Neuro.Api.Controllers;


[AllowAnonymous]
public class HomeController : BaseController
{
    private readonly IHostEnvironment _env;

    public HomeController(IHostEnvironment env)
    {
        _env = env;
    }

    [HttpGet("/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Get()
    {
        if (!_env.IsDevelopment())
        {
            return BadRequest("Please use '/api/v1/ControllerName/ActionName' to access the API.");
        }

        return Redirect("/swagger/index.html");
    }

    [HttpGet("Version")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult GetVersion()
    {
        return Ok("1.0.0");
    }
    [HttpGet("Environment")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult GetEnvironment()
    {
        return Ok(_env.EnvironmentName);
    }
}