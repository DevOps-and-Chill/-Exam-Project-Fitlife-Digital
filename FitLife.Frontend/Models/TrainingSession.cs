namespace FitLife.Frontend.Models;

public class TrainingSession
{
    public string Title { get; set; } = "";
    public string InstructorName { get; set; } = "";
    public DateTime StartTime { get; set; }
    public int Capacity { get; set; }
    public int RegisteredCount { get; set; }

    public int AvailableSpots => Capacity - RegisteredCount;
    public bool IsUserSignedUp { get; set; }
}