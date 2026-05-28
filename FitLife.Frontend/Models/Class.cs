namespace FitLife.Frontend.Models;

public class Class
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string PartitionKey { get; set; } = "classes";
    public string Title { get; set; } = "";
    
    // (e.g. kl. 14:00 - 16:00)
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }

    public int MemberLimit { get; set; }
    public bool ActiveClass { get; set; } = true;
    
    public bool IsUserSignedUp { get; set; }
    
    // FK
    public string CoachId { get; set; } = Guid.NewGuid().ToString();
    public string ExerciseGymId { get; set; } = Guid.NewGuid().ToString();
    public string RoomId { get; set; } = Guid.NewGuid().ToString();
    
    // Lists
    public List<Member> Registered { get; set; } = new List<Member>();
    public List<Member> Attended { get; set; } = new List<Member>();
    public List<Member> WaitingList { get; set; } = new List<Member>();
}