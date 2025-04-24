using InMemoryCacheExample.Api.Models;
using InMemoryCacheExample.Service.Contracts;
using InMemoryCacheExample.Service.DTOs;
using InMemoryCacheExample.Service.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace InMemoryCacheExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAllAsync();
        return Ok(users);
    }

    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await userService.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRequestModel requestModel)
    {
        var user = new UserRequestDTO
        {
            Username = requestModel.Username,
            Fullname = requestModel.Fullname
        };
        var newId = await userService.CreateAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = newId }, requestModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserRequestModel requestModel)
    {
        var user = new UserRequestDTO
        {
            Username = requestModel.Username,
            Fullname = requestModel.Fullname
        };
        var success = await userService.UpdateAsync(id, user);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await userService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }

}
