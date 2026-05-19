namespace ClassServiceAPI.Models;

public class ExerciseGym
{
    public Guid Id { get; set; }
    
    // Lists
    public List<Room> Rooms { get; set; } = new List<Room>();
}