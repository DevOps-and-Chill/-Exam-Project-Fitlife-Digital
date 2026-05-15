namespace FitLife.Frontend.Models;

public class TrainingSession
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public string InstructorName { get; set; } = "";

    public DateTime StartTime { get; set; }

    public int Capacity { get; set; }

    public int RegisteredCount { get; set; }

    public bool IsUserSignedUp { get; set; }

    public int AvailableSpots => Capacity - RegisteredCount;
}