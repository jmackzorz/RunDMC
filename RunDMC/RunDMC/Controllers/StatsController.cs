using Microsoft.AspNetCore.Mvc;
using RunDMC.DTOs;
using RunDMC.Services;

namespace RunDMC.Controllers;

[ApiController]
[Route("api/stats")]
public class StatsController(IStatsService statsService) : ControllerBase
{
    [HttpGet("weekly/{userId:int}")]
    public async Task<ActionResult<IEnumerable<WeeklyStatsDto>>> GetWeekly(
        int userId,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null)
    {
        var resolvedFrom = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-84)); // last 12 weeks
        var resolvedTo = to ?? DateOnly.FromDateTime(DateTime.UtcNow);

        if (resolvedFrom > resolvedTo)
            return BadRequest("'from' must not be later than 'to'.");

        var stats = await statsService.GetWeeklyStatsAsync(userId, resolvedFrom, resolvedTo);
        return Ok(stats);
    }

    [HttpGet("personal-records/{userId:int}")]
    public async Task<ActionResult<PersonalRecordDto>> GetPersonalRecords(int userId) =>
        Ok(await statsService.GetPersonalRecordsAsync(userId));

    [HttpGet("trends/{userId:int}")]
    public async Task<ActionResult<TrendAnalysisDto>> GetTrends(int userId) =>
        Ok(await statsService.GetTrendAnalysisAsync(userId));
}
