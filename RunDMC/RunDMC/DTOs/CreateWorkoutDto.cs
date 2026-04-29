using System.ComponentModel.DataAnnotations;

namespace RunDMC.DTOs;

public record CreateWorkoutDto(
    [property: Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
    int UserId,

    [property: Range(1, int.MaxValue, ErrorMessage = "ActivityTypeId must be a positive integer.")]
    int ActivityTypeId,

    [property: Range(0, double.MaxValue, ErrorMessage = "Distance must be non-negative.")]
    double Distance,

    [property: Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes.")]
    int Duration,

    [property: Range(0, int.MaxValue, ErrorMessage = "Calories must be non-negative.")]
    int Calories,

    DateTime Date
);
