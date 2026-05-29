namespace FacilityServiceAPI.Models
{
	public abstract class Facility
	{
		public Facility(string name, string address, string telephone, string email, List<OpeningHoursSpecification> openingHours)
		{
			Name = name;
			Address = address;
			Telephone = telephone;
			Email = email;
			OpeningHours = openingHours;
		}
		public Facility()
		{

		}

		public string Id { get; init; } = Guid.NewGuid().ToString();
		public string PartitionKey { get; set; } = "Facilities";
		public string Name { get; set; }
		public string Address { get; set; }
		public string Telephone { get; set; }
		public string Email { get; set; }
		public List<OpeningHoursSpecification> OpeningHours { get; set; }
	}

}
