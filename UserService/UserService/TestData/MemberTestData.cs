using UserServiceAPI.Models;
using UserServiceAPI.Models.Enums;

namespace UserServiceAPI.TestData
{
    public static class MemberTestData
    {
        private static readonly Guid fitnessCenterId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid fitnessCenterId2 = Guid.Parse("11111111-1111-1111-1111-111111111112");

        public static List<Member> members => new()
        {
            new Member(
                UserRole.Member,
                "Mikkel",
                "Andersen",
                new DateTime(1998, 4, 12),
                "Vestergade 12, 8000 Aarhus C",
                "22334455",
                "mikkel.andersen@mail.dk",
                fitnessCenterId1,
                true,
                MembershipType.Standard,
                MembershipOptional.Swimming
            ),

            new Member(
                UserRole.Member,
                "Sofie",
                "Nielsen",
                new DateTime(1995, 7, 2),
                "Nørrebrogade 45, 8200 Aarhus N",
                "33445566",
                "sofie.nielsen@mail.dk",
                fitnessCenterId1,
                true,
                MembershipType.Premium,
                MembershipOptional.Pizza
            ),

            new Member(
                UserRole.Member,
                "Lucas",
                "Jensen",
                new DateTime(2001, 1, 19),
                "Parkvej 8, 8260 Viby J",
                "44556677",
                "lucas.jensen@mail.dk",
                fitnessCenterId1,
                true,
                MembershipType.Standard,
                MembershipOptional.Pizza
            ),

            new Member(
                UserRole.Member,
                "Emma",
                "Christensen",
                new DateTime(1997, 9, 30),
                "Skovvej 19, 8240 Risskov",
                "55667788",
                "emma.christensen@mail.dk",
                fitnessCenterId2,
                true,
                MembershipType.Premium,
                MembershipOptional.Swimming
            ),

            new Member(
                UserRole.Member,
                "Oliver",
                "Madsen",
                new DateTime(1993, 11, 5),
                "Engtoften 3, 8381 Tilst",
                "66778899",
                "oliver.madsen@mail.dk",
                fitnessCenterId2,
                true,
                MembershipType.Standard,
                MembershipOptional.Swimming
            ),

            new Member(
                UserRole.Member,
                "Freja",
                "Larsen",
                new DateTime(1999, 6, 14),
                "Bakkevej 27, 8270 Højbjerg",
                "77889900",
                "freja.larsen@mail.dk",
                fitnessCenterId2,
                true,
                MembershipType.Premium,
                MembershipOptional.Pizza
            )
        };
    }
}