namespace FacilityServiceAPI.Models
{
	public class OpeningHoursSpecification
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public DayOfWeek DayOfWeek { get; set; }
		public string Opens { get; set; }
		public string Closes { get; set; }
	}
}
