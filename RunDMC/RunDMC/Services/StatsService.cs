using Microsoft.EntityFrameworkCore;
using RunDMC.Data;
using RunDMC.DTOs;

namespace RunDMC.Services;

public class StatsService(FitnessDbContext context) : IStatsService
{
    public async Task<IEnumerable<WeeklyStatsDto>> GetWeeklyStatsAsync(int userId, DateOnly from, DateOnly to)
    {
        var fromDate = from.ToDateTime(TimeOnly.MinValue);
        var toDate = to.ToDateTime(TimeOnly.MaxValue);

        var workouts = await context.Workouts
            .Where(w => w.UserId == userId && w.Date >= fromDate && w.Date <= toDate)
            .ToListAsync();

        return workouts
            .GroupBy(w => StartOfWeek(w.Date))
            .Select(g =>
            {
                var totalDistance = Math.Round(g.Sum(w => w.Distance), 2);
                var totalDuration = g.Sum(w => w.Duration);
                double? avgPace = totalDistance > 0
                    ? Math.Round(totalDuration / totalDistance, 2)
                    : null;

                return new WeeklyStatsDto(
                    DateOnly.FromDateTime(g.Key),
                    totalDistance,
                    totalDuration,
                    g.Sum(w => w.Calories),
                    g.Count(),
                    avgPace
                );
            })
            .OrderByDescending(s => s.WeekStart);
    }

    public async Task<PersonalRecordDto> GetPersonalRecordsAsync(int userId)
    {
        // Three targeted queries — each lets SQL Server sort and return one row.
        var longestDistanceWorkout = await context.Workouts
            .Include(w => w.ActivityType)
            .Where(w => w.UserId == userId && w.Distance > 0)
            .OrderByDescending(w => w.Distance)
            .FirstOrDefaultAsync();

        var longestDurationWorkout = await context.Workouts
            .Include(w => w.ActivityType)
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.Duration)
            .FirstOrDefaultAsync();

        // Pace = duration / distance (min/km). Ascending = fastest first.
        var fastestPaceWorkout = await context.Workouts
            .Include(w => w.ActivityType)
            .Where(w => w.UserId == userId && w.Distance > 0)
            .OrderBy(w => w.Duration / w.Distance)
            .FirstOrDefaultAsync();

        return new PersonalRecordDto(
            longestDistanceWorkout is null ? null : new PersonalRecordEntry(
                Math.Round(longestDistanceWorkout.Distance, 2),
                "km",
                longestDistanceWorkout.ActivityType.Name,
                DateOnly.FromDateTime(longestDistanceWorkout.Date)),

            fastestPaceWorkout is null ? null : new PersonalRecordEntry(
                Math.Round(fastestPaceWorkout.Duration / fastestPaceWorkout.Distance, 2),
                "min/km",
                fastestPaceWorkout.ActivityType.Name,
                DateOnly.FromDateTime(fastestPaceWorkout.Date)),

            longestDurationWorkout is null ? null : new PersonalRecordEntry(
                longestDurationWorkout.Duration,
                "minutes",
                longestDurationWorkout.ActivityType.Name,
                DateOnly.FromDateTime(longestDurationWorkout.Date))
        );
    }

    public async Task<TrendAnalysisDto> GetTrendAnalysisAsync(int userId)
    {
        var today = DateTime.UtcNow.Date;
        var recentStart = today.AddDays(-28);
        var previousStart = today.AddDays(-56);

        var workouts = await context.Workouts
            .Where(w => w.UserId == userId && w.Date >= previousStart && w.Date < today.AddDays(1))
            .ToListAsync();

        var recentWorkouts = workouts.Where(w => w.Date >= recentStart).ToList();
        var previousWorkouts = workouts.Where(w => w.Date < recentStart).ToList();

        var recentDistance = recentWorkouts.Sum(w => w.Distance);
        var previousDistance = previousWorkouts.Sum(w => w.Distance);

        TrendMetricDto? distanceTrend = null;
        if (previousDistance > 0)
        {
            var pct = Math.Round((recentDistance - previousDistance) / previousDistance * 100, 1);
            distanceTrend = new TrendMetricDto(GetDirection(pct, higherIsBetter: true), pct);
        }

        var recentPaced = recentWorkouts.Where(w => w.Distance > 0).ToList();
        var previousPaced = previousWorkouts.Where(w => w.Distance > 0).ToList();

        TrendMetricDto? paceTrend = null;
        if (previousPaced.Count > 0 && recentPaced.Count > 0)
        {
            var recentAvgPace = recentPaced.Average(w => w.Duration / w.Distance);
            var previousAvgPace = previousPaced.Average(w => w.Duration / w.Distance);
            var pct = Math.Round((recentAvgPace - previousAvgPace) / previousAvgPace * 100, 1);
            paceTrend = new TrendMetricDto(GetDirection(pct, higherIsBetter: false), pct);
        }

        return new TrendAnalysisDto(distanceTrend, paceTrend);
    }

    private static string GetDirection(double percentageChange, bool higherIsBetter)
    {
        if (Math.Abs(percentageChange) <= 2.0) return "Stable";
        return (percentageChange > 0) == higherIsBetter ? "Improving" : "Declining";
    }

    private static DateTime StartOfWeek(DateTime date)
    {
        int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-diff).Date;
    }
}
