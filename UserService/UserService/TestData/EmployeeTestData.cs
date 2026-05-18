using UserServiceAPI.Models;
using UserServiceAPI.Models.Enums;

namespace UserServiceAPI.TestData
{
    public static class EmployeeTestData
    {
        private static readonly Guid fitnessCenterId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid fitnessCenterId2 = Guid.Parse("11111111-1111-1111-1111-111111111112");

        public static List<Employee> employees => new()
        {
            new Employee(
                UserRole.Employee,
                "Andreas",
                "Pedersen",
                new DateTime(1985, 3, 12),
                "Søndergade 5, 8000 Aarhus C",
                "11223344",
                "andreas.pedersen@fitlife.dk",
                fitnessCenterId1,
                true,
                EmployeeRole.Manager,
                false
            ),

            new Employee(
                UserRole.Employee,
                "Julie",
                "Mortensen",
                new DateTime(1992, 7, 21),
                "Kirkegade 14, 8230 Åbyhøj",
                "22334411",
                "julie.mortensen@fitlife.dk",
                fitnessCenterId1,
                true,
                EmployeeRole.Staffmember,
                true
            ),

            new Employee(
                UserRole.Employee,
                "Kasper",
                "Hansen",
                new DateTime(1990, 11, 5),
                "Møllevej 9, 8250 Egå",
                "33441122",
                "kasper.hansen@fitlife.dk",
                fitnessCenterId2,
                true,
                EmployeeRole.Staffmember,
                false
            ),

            new Employee(
                UserRole.Employee,
                "Laura",
                "Thomsen",
                new DateTime(1995, 1, 17),
                "Lindevej 21, 8520 Lystrup",
                "44552233",
                "laura.thomsen@fitlife.dk",
                fitnessCenterId2,
                true,
                EmployeeRole.Staffmember,
                true
            )
        };

    }
}