namespace ClassServiceAPI.Models;

public class ExerciseGym
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    // Lists
    public List<Room> Rooms { get; set; } = new List<Room>();
}