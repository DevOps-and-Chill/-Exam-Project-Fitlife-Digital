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

		public Guid Id { get; init; } = Guid.NewGuid();
		public string Name { get; private set; }
		public string Address { get; private set; }
		public string Telephone { get; private set; }
		public string Email { get; private set; }
		public List<OpeningHoursSpecification> OpeningHours { get; private set; }
	}

}
