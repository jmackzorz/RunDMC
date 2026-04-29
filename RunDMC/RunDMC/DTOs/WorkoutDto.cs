namespace RunDMC.DTOs;

public record WorkoutDto(
    int Id,
    int UserId,
    int ActivityTypeId,
    string ActivityTypeName,
    double Distance,
    int Duration,
    int Calories,
    DateTime Date
);
