namespace RunDMC.DTOs;

public record TrendMetricDto(string Direction, double PercentageChange);

public record TrendAnalysisDto(
    TrendMetricDto? Distance,
    TrendMetricDto? Pace
);
