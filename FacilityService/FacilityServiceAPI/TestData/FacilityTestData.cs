using FacilityServiceAPI.Models;

namespace FacilityServiceAPI.TestData
{
	public static class FacilityTestData
	{
		public static List<ExerciseGym> ExerciseGyms => new()
	{
		new ExerciseGym(
			name: "fitlife 8000",
			address: "123 jagtvej, Aarhus",
			telephone: "+45 12 34 56 78",
			email: "info@fitlife.dk",
			openingHours: new List<OpeningHoursSpecification>
			{
				new() { DayOfWeek = DayOfWeek.Monday, Opens = "06:00", Closes = "22:00" },
				new() { DayOfWeek = DayOfWeek.Tuesday, Opens = "06:00", Closes = "22:00" },
				new() { DayOfWeek = DayOfWeek.Wednesday, Opens = "06:00", Closes = "22:00" },
				new() { DayOfWeek = DayOfWeek.Thursday, Opens = "06:00", Closes = "22:00" },
				new() { DayOfWeek = DayOfWeek.Friday, Opens = "06:00", Closes = "20:00" },
				new() { DayOfWeek = DayOfWeek.Saturday, Opens = "08:00", Closes = "18:00" },
				new() { DayOfWeek = DayOfWeek.Sunday, Opens = "08:00", Closes = "18:00" },
			},
			roomsForClasses: 4,
			managerId: Guid.Parse("0d79b539-f1e5-4487-b7fd-e9caa3939728")
		),

		new ExerciseGym(
			name: "fitlife 8210",
			address: "45 blommevej, Brabraand",
			telephone: "+45 87 65 43 21",
			email: "info@fitlife.dk",
			openingHours: new List<OpeningHoursSpecification>
			{
				new() { DayOfWeek = DayOfWeek.Monday, Opens = "05:30", Closes = "23:00" },
				new() { DayOfWeek = DayOfWeek.Tuesday, Opens = "05:30", Closes = "23:00" },
				new() { DayOfWeek = DayOfWeek.Wednesday, Opens = "05:30", Closes = "23:00" },
				new() { DayOfWeek = DayOfWeek.Thursday, Opens = "05:30", Closes = "23:00" },
				new() { DayOfWeek = DayOfWeek.Friday, Opens = "05:30", Closes = "21:00" },
				new() { DayOfWeek = DayOfWeek.Saturday, Opens = "07:00", Closes = "19:00" },
				new() { DayOfWeek = DayOfWeek.Sunday, Opens = "07:00", Closes = "19:00" },
			},
			roomsForClasses: 6,
			managerId: Guid.Parse("fb08c3c4-12c2-4274-99db-a2203eb9d4d9")
		)

	};

		public static List<SwimmingPool> SwimmingPools => new()
		{
			new SwimmingPool(
			name: "Din lokale pool",
			address: "5 solskinsvej, aarhus",
			telephone: "+45 44 55 66 77",
			email: "info@dlp.dk",
			openingHours: new List<OpeningHoursSpecification>
			{
				new() { DayOfWeek = DayOfWeek.Monday, Opens = "06:00", Closes = "20:00" },
				new() { DayOfWeek = DayOfWeek.Tuesday, Opens = "06:00", Closes = "20:00" },
				new() { DayOfWeek = DayOfWeek.Wednesday, Opens = "06:00", Closes = "20:00" },
				new() { DayOfWeek = DayOfWeek.Thursday, Opens = "06:00", Closes = "20:00" },
				new() { DayOfWeek = DayOfWeek.Friday, Opens = "06:00", Closes = "18:00" },
				new() { DayOfWeek = DayOfWeek.Saturday, Opens = "08:00", Closes = "17:00" },
				new() { DayOfWeek = DayOfWeek.Sunday, Opens = "08:00", Closes = "17:00" },
			},
			swimLanes: 8
		),

			new SwimmingPool(
			name: "Swim city",
			address: "15 badevej, Aarhus",
			telephone: "+45 77 88 99 00",
			email: "contact@sc.dk",
			openingHours: new List<OpeningHoursSpecification>
			{
				new() { DayOfWeek = DayOfWeek.Monday, Opens = "07:00", Closes = "21:00" },
				new() { DayOfWeek = DayOfWeek.Tuesday, Opens = "07:00", Closes = "21:00" },
				new() { DayOfWeek = DayOfWeek.Wednesday, Opens = "07:00", Closes = "21:00" },
				new() { DayOfWeek = DayOfWeek.Thursday, Opens = "07:00", Closes = "21:00" },
				new() { DayOfWeek = DayOfWeek.Friday, Opens = "07:00", Closes = "19:00" },
				new() { DayOfWeek = DayOfWeek.Saturday, Opens = "09:00", Closes = "18:00" },
				new() { DayOfWeek = DayOfWeek.Sunday, Opens = "09:00", Closes = "18:00" },
			},
			swimLanes: 10
		)
		};
	}
}
