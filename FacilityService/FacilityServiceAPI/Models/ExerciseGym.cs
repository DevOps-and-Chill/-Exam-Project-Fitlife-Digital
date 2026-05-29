using System.Collections.Generic;

namespace FacilityServiceAPI.Models
{
	public class ExerciseGym : Facility
	{
		public ExerciseGym(string name, string address, string telephone, string email, List<OpeningHoursSpecification> openingHours, int roomsForClasses, Guid managerId) : base(name, address, telephone, email, openingHours)
		{
			RoomsForClasses = roomsForClasses;
			ManagerId = managerId;
		}
		
		public ExerciseGym() 
		{ 
		}
		public int RoomsForClasses { get; set; }
		public Guid ManagerId { get; set; }
	}
}
