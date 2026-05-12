using FacilityServiceAPI.Models;

namespace FacilityServiceAPI.TestData
{
    public static class FacilityTestData
    {
        public static List<ExerciseGym> ExerciseGyms => new()
    {
        new ExerciseGym(
            name: "Downtown Fitness Center",
            address: "123 Main Street, Copenhagen",
            telephone: "+45 12 34 56 78",
            email: "contact@downtownfitness.dk",
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
            managerId: Guid.Parse("11111111-1111-1111-1111-111111111111")
        ),

        new ExerciseGym(
            name: "Nordic Strength Gym",
            address: "45 Harbor Road, Aarhus",
            telephone: "+45 87 65 43 21",
            email: "info@nordicstrength.dk",
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
            managerId: Guid.Parse("22222222-2222-2222-2222-222222222222")
        ),

        new ExerciseGym(
            name: "Pulse Training Studio",
            address: "78 Green Avenue, Odense",
            telephone: "+45 98 76 54 32",
            email: "hello@pulsetraining.dk",
            openingHours: new List<OpeningHoursSpecification>
            {
                new() { DayOfWeek = DayOfWeek.Monday, Opens = "07:00", Closes = "21:00" },
                new() { DayOfWeek = DayOfWeek.Tuesday, Opens = "07:00", Closes = "21:00" },
                new() { DayOfWeek = DayOfWeek.Wednesday, Opens = "07:00", Closes = "21:00" },
                new() { DayOfWeek = DayOfWeek.Thursday, Opens = "07:00", Closes = "21:00" },
                new() { DayOfWeek = DayOfWeek.Friday, Opens = "07:00", Closes = "19:00" },
                new() { DayOfWeek = DayOfWeek.Saturday, Opens = "09:00", Closes = "16:00" },
                new() { DayOfWeek = DayOfWeek.Sunday, Opens = "09:00", Closes = "16:00" },
            },
            roomsForClasses: 2,
            managerId: Guid.Parse("33333333-3333-3333-3333-333333333333")
        )
    };

        public static List<SwimmingPool> SwimmingPools => new()
    {
        new SwimmingPool(
            name: "AquaLife Pool Center",
            address: "200 Ocean Drive, Copenhagen",
            telephone: "+45 44 55 66 77",
            email: "info@aqualife.dk",
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
            name: "BlueWave Swimming Arena",
            address: "15 Lakeview Street, Aarhus",
            telephone: "+45 77 88 99 00",
            email: "contact@bluewave.dk",
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
        ),

        new SwimmingPool(
            name: "Family Splash Pool",
            address: "89 Riverside Blvd, Aalborg",
            telephone: "+45 66 77 88 99",
            email: "hello@familysplash.dk",
            openingHours: new List<OpeningHoursSpecification>
            {
                new() { DayOfWeek = DayOfWeek.Monday, Opens = "08:00", Closes = "18:00" },
                new() { DayOfWeek = DayOfWeek.Tuesday, Opens = "08:00", Closes = "18:00" },
                new() { DayOfWeek = DayOfWeek.Wednesday, Opens = "08:00", Closes = "18:00" },
                new() { DayOfWeek = DayOfWeek.Thursday, Opens = "08:00", Closes = "18:00" },
                new() { DayOfWeek = DayOfWeek.Friday, Opens = "08:00", Closes = "17:00" },
                new() { DayOfWeek = DayOfWeek.Saturday, Opens = "10:00", Closes = "16:00" },
                new() { DayOfWeek = DayOfWeek.Sunday, Opens = "10:00", Closes = "16:00" },
            },
            swimLanes: 5
        )
    };
    }
}
