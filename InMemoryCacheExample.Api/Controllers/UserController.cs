using InMemoryCacheExample.Service.Contracts;
using InMemoryCacheExample.Service.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace InMemoryCacheExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    //to check
    [HttpGet("count")]
    public async Task<IActionResult> GetUserCount()
    {
        var count = await userService.GetUserCountAsync();
        return Ok(new { count });
    }

}
