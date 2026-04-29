namespace RunDMC.Models;

public class ActivityType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Workout> Workouts { get; set; } = [];
}
