using Microsoft.AspNetCore.Mvc;
using RunDMC.DTOs;
using RunDMC.Services;

namespace RunDMC.Controllers;

[ApiController]
[Route("api/workouts")]
public class WorkoutsController(IWorkoutService workoutService) : ControllerBase
{
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<IEnumerable<WorkoutDto>>> GetByUser(int userId)
    {
        var workouts = await workoutService.GetByUserAsync(userId);
        return Ok(workouts);
    }

    [HttpPost]
    public async Task<ActionResult<WorkoutDto>> Create(CreateWorkoutDto dto)
    {
        var created = await workoutService.CreateAsync(dto);

        if (created is null)
            return NotFound("User or ActivityType not found.");

        return CreatedAtAction(nameof(GetByUser), new { userId = created.UserId }, created);
    }
}
