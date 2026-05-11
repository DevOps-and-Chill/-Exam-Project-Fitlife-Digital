namespace FacilityServiceAPI.Models
{
	public class OpeningHoursSpecification
	{
		public DayOfWeek DayOfWeek { get; set; }
		public string Opens { get; set; }
		public string Closes { get; set; }
	}
}
