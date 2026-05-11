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

		public int RoomsForClasses { get; private set; }
		public Guid ManagerId { get; private set; }
	}
}
