namespace RunDMC.DTOs;

public record PersonalRecordEntry(
    double Value,
    string Unit,
    string ActivityType,
    DateOnly Date
);
