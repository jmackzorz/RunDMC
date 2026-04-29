using RunDMC.DTOs;

namespace RunDMC.Services;

public interface IStatsService
{
    Task<IEnumerable<WeeklyStatsDto>> GetWeeklyStatsAsync(int userId, DateOnly from, DateOnly to);
    Task<PersonalRecordDto> GetPersonalRecordsAsync(int userId);
    Task<TrendAnalysisDto> GetTrendAnalysisAsync(int userId);
}
