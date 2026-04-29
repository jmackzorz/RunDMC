using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RunDMC.Data;
using RunDMC.DTOs;
using RunDMC.Models;

namespace RunDMC.Services;

public class WorkoutService(FitnessDbContext context, IMapper mapper) : IWorkoutService
{
    public async Task<IEnumerable<WorkoutDto>> GetByUserAsync(int userId)
    {
        var workouts = await context.Workouts
            .Include(w => w.ActivityType)
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.Date)
            .ToListAsync();

        return mapper.Map<IEnumerable<WorkoutDto>>(workouts);
    }

    public async Task<WorkoutDto?> CreateAsync(CreateWorkoutDto dto)
    {
        var userExists = await context.Users.AnyAsync(u => u.Id == dto.UserId);
        if (!userExists) return null;

        var activityTypeExists = await context.ActivityTypes.AnyAsync(a => a.Id == dto.ActivityTypeId);
        if (!activityTypeExists) return null;

        var workout = mapper.Map<Workout>(dto);
        context.Workouts.Add(workout);
        await context.SaveChangesAsync();

        await context.Entry(workout).Reference(w => w.ActivityType).LoadAsync();
        return mapper.Map<WorkoutDto>(workout);
    }
}
