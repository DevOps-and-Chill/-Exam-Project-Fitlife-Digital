using UserServiceAPI.Models;

namespace UserServiceAPI.TestData
{
    public static class UserTestData
    {
        public static List<User> users =>
            EmployeeTestData.employees.Cast<User>()
            .Concat(MemberTestData.members.Cast<User>())
            .ToList();
    }
}
