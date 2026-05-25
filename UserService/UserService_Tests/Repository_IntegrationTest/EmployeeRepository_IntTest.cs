using Microsoft.EntityFrameworkCore;
using UserServiceAPI.Data;
using UserServiceAPI.Models;
using UserServiceAPI.Models.Enums;
using UserServiceAPI.Repositories;
using UserServiceAPI.Repositories.Interfaces;
using UserServiceAPI.TestData;

namespace UserService_Tests.Repository_IntegrationTesting
{
    [TestClass]
    public sealed class EmployeeRepositoryDBTest
    {
        private IEmployeeRepository employeeRepository;

        [TestInitialize]
        public void InitializeTest()
        {
            var options =
                new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

            var context =
                new UserDbContext(options);

            context.Users.AddRange(
                EmployeeTestData.employees);

            context.SaveChanges();

            employeeRepository =
                new EmployeeRepositoryDB(
                    context);
        }

        // -------------------
        // GET
        // -------------------

        [TestMethod]
        public async Task TestGetAllEmployeesSuccess()
        {
            var result =
                await employeeRepository
                    .GetAllEmployees();

            Assert.IsNotNull(result);

            Assert.AreEqual(
                EmployeeTestData.employees.Count,
                result.Count);
        }

        [TestMethod]
        public async Task TestGetEmployeeByIdSuccess()
        {
            var expected =
                (await employeeRepository
                    .GetAllEmployees())
                    .First();

            var result =
                await employeeRepository
                    .GetEmployeeById(
                        expected.Id);

            Assert.IsNotNull(result);

            Assert.AreEqual(
                expected.Email,
                result.Email);
        }

        [TestMethod]
        public async Task TestGetEmployeeByIdReturnsNull()
        {
            var result =
                await employeeRepository
                    .GetEmployeeById(
                        Guid.NewGuid()
                            .ToString());

            Assert.IsNull(result);
        }

        // -------------------
        // UPDATE
        // -------------------

        [TestMethod]
        public async Task TestEndEmploymentSuccess()
        {
            var employee =
                (await employeeRepository
                    .GetAllEmployees())
                    .First();

            var result =
                await employeeRepository
                    .EndEmploymentForEmployee(
                        employee.Id);

            Assert.IsFalse(
                result.ActiveEmployment);
        }

        [TestMethod]
        public async Task TestSetEmployeeAsManagerSuccess()
        {
            var employee =
                (await employeeRepository
                    .GetAllEmployees())
                    .First(
                        x =>
                        x.EmployeeRoleName
                        != EmployeeRole.Manager);

            var result =
                await employeeRepository
                    .SetEmployeeAsManager(
                        employee.Id);

            Assert.AreEqual(
                EmployeeRole.Manager,
                result.EmployeeRoleName);
        }

        [TestMethod]
        public async Task TestSetAccountAsInactiveSuccess()
        {
            var employee =
                (await employeeRepository
                    .GetAllEmployees())
                    .First();

            var result =
                await employeeRepository
                    .SetAccountAsInactive(
                        employee.Id);

            Assert.IsFalse(
                result.ActiveUser);
        }

        // -------------------
        // UPSERT
        // -------------------

        [TestMethod]
        public async Task TestCreateEmployeeSuccess()
        {
            var employee =
                new Employee(
                    UserRole.Employee,
                    "Test",
                    "Employee",
                    new DateTime(
                        1990,
                        1,
                        1),
                    "Street",
                    "12345678",
                    "new@employee.dk",
                    Guid.NewGuid(),
                    true,
                    EmployeeRole.Staffmember,
                    false);

            var result =
                await employeeRepository
                    .UpsertEmployee(
                        employee);

            Assert.IsNotNull(
                result);

            Assert.AreEqual(
                employee.Email,
                result.Email);
        }

        [TestMethod]
        public async Task TestUpdateEmployeeSuccess()
        {
            var employee =
                (await employeeRepository
                    .GetAllEmployees())
                    .First();

            employee.Email =
                "updated@test.dk";

            var result =
                await employeeRepository
                    .UpsertEmployee(
                        employee);

            Assert.AreEqual(
                "updated@test.dk",
                result.Email);
        }

        [TestMethod]
        public async Task TestCreateEmployeeDuplicateEmailThrows()
        {
            // Arrange

            var existing =
                (await employeeRepository
                    .GetAllEmployees())
                    .First();

            var employee =
                new Employee(
                    UserRole.Employee,
                    "Duplicate",
                    "User",
                    DateTime.Now,
                    "Address",
                    "11111111",
                    existing.Email,
                    Guid.NewGuid(),
                    true,
                    EmployeeRole.Staffmember,
                    false);

            // Act + Assert

            try
            {
                await employeeRepository
                    .UpsertEmployee(
                        employee);

                Assert.Fail(
                    "Expected InvalidOperationException");
            }
            catch (InvalidOperationException)
            {
                // success
            }
        }

        // -------------------
        // DELETE
        // -------------------

        [TestMethod]
        public async Task TestDeleteEmployeeSuccess()
        {
            var employee =
                (await employeeRepository
                    .GetAllEmployees())
                    .First();

            await employeeRepository
                .DeleteEmployee(
                    employee.Id);

            var result =
                await employeeRepository
                    .GetEmployeeById(
                        employee.Id);

            Assert.IsNull(
                result);
        }

        // -------------------
        // FILTER
        // -------------------

        [TestMethod]
        public async Task TestGetEmployeesByAffiliationSuccess()
        {
            var employee =
                (await employeeRepository
                    .GetAllEmployees())
                    .First();

            var result =
                await employeeRepository
                    .GetEmployeesByAffiliation(
                        employee.Affiliation);

            Assert.IsTrue(
                result.Count > 0);
        }
    }
}