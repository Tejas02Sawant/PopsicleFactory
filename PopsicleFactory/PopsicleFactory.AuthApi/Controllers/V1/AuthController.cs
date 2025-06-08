using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopsicleFactory.AuthApi.Models;
using PopsicleFactory.AuthApi.Services;

namespace PopsicleFactory.AuthApi.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
    {
        var result = await authService.LoginAsync(request);
        if (result is null)
            return BadRequest("Invalid username or password.");

        return Ok(result);
    }

    [Authorize]
    [HttpGet("authenticated-only")]
    public IActionResult AuthenticatedOnlyEndpoint()
    {
        return Ok("You are authenticated");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("You are authenticated with an admin role!");
    }
}
