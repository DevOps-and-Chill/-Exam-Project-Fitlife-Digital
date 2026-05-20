using RapportServiceAPI.Models.Enums;

namespace RapportServiceAPI.Models
{
    public class Statistik
    {
        public Statistik(string name, string description, Guid facilityId, Guid roomId)
        {
            Name = name;
            Description = description;
            FacilityId = facilityId;
            RoomId = roomId;
        }

        public Guid Id { get; init; } = Guid.NewGuid();
        public string PartitionKey { get; set; } = "statistikker";
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime DataCreated { get; init; } = DateTime.UtcNow;
        public DateTime DateModified { get; private set; } = DateTime.UtcNow;
        public Guid FacilityId { get; private set; }
        public Guid RoomId { get; private set; }
        public bool ActiveClass { get; private set; }
        public List<Guid> Registrered { get; private set; } = new();
        public List<Guid> Attended { get; private set; } = new();
        public List<Guid> WaitingList { get; private set; } = new();
        public List<Lagring> Lagrings { get; private set; } = new();
        public List<Deling> Delings { get; private set; } = new();
        public List<Analyse> Analyses { get; private set; } = new();    
    }
}