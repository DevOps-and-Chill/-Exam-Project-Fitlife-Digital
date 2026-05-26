namespace FitLife.Frontend.Models
{
    public class Statistic
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime DataCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid FacilityId { get; set; }
        public Guid RoomId { get; set; }
        public bool ActiveClass { get; set; }
        public List<Guid> Registrered { get; set; } = new();
        public List<Guid> Attended { get; set; } = new();
        public List<Guid> WaitingList { get; set; } = new();
    }
}