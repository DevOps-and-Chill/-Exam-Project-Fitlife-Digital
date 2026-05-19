namespace FacilityServiceAPI.Models
{
	public class SwimmingPool : Facility
	{
		public SwimmingPool(string name, string address, string telephone, string email, List<OpeningHoursSpecification> openingHours, int swimLanes) : base(name, address, telephone, email, openingHours)
		{
			Swimlanes = swimLanes;
		}
		public int Swimlanes { get; set; }
	}
}
