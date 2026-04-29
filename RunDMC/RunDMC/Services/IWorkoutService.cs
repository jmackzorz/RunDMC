using RunDMC.DTOs;

namespace RunDMC.Services;

public interface IWorkoutService
{
    Task<IEnumerable<WorkoutDto>> GetByUserAsync(int userId);
    Task<WorkoutDto?> CreateAsync(CreateWorkoutDto dto);
}
