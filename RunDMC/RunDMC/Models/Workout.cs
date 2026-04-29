namespace RunDMC.Models;

public class Workout
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ActivityTypeId { get; set; }
    public double Distance { get; set; }
    public int Duration { get; set; }
    public int Calories { get; set; }
    public DateTime Date { get; set; }

    public User User { get; set; } = null!;
    public ActivityType ActivityType { get; set; } = null!;
}
