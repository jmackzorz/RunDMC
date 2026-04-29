namespace RunDMC.DTOs;

public record PersonalRecordDto(
    PersonalRecordEntry? LongestDistance,   // km
    PersonalRecordEntry? FastestPace,       // min/km
    PersonalRecordEntry? LongestDuration    // minutes
);
