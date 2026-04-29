using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunDMC.Data;
using RunDMC.Models;

namespace RunDMC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(FitnessDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll() =>
        Ok(await context.Users.ToListAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await context.Users.FindAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }
}
