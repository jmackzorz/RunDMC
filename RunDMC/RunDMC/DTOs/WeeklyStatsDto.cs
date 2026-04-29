namespace RunDMC.DTOs;

public record WeeklyStatsDto(
    DateOnly WeekStart,
    double TotalDistance,
    int TotalDuration,
    int TotalCalories,
    int WorkoutCount,
    double? AveragePace  // minutes per km; null when week has no distance (e.g. all strength)
);
