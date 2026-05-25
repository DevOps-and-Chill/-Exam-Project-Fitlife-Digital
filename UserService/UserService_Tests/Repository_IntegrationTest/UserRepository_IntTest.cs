using Microsoft.EntityFrameworkCore;
using UserServiceAPI.Data;
using UserServiceAPI.Models;
using UserServiceAPI.Repositories;
using UserServiceAPI.Repositories.Interfaces;
using UserServiceAPI.TestData;

namespace UserService_Tests.Repository_IntegrationTesting
{
    [TestClass]
    public sealed class UserRepositoryDBTest
    {
        private IUserRepository userRepository;

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

            userRepository =
                new UserRepositoryDB(context);
        }

        // ------------------------
        // GET ALL
        // ------------------------

        [TestMethod]
        public async Task TestGetAllUsersSuccess()
        {
            // Arrange

            await userRepository.LoadTestData();

            // Act

            var result =
                await userRepository.GetAllUsers();

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual(
                UserTestData.users.Count,
                result.Count);
        }

        // ------------------------
        // GET BY ID
        // ------------------------

        [TestMethod]
        public async Task TestGetUserByIdSuccess()
        {
            // Arrange

            await userRepository.LoadTestData();

            const string email =
                "andreas.pedersen@fitlife.dk";

            var id =
                await userRepository
                    .GetUserIdByEmail(email);

            // Act

            var result =
                await userRepository
                    .GetUserById(id);

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual(
                email,
                result.Email);

            Assert.AreEqual(
                id,
                result.Id);
        }

        [TestMethod]
        public async Task TestGetUserByIdReturnsNull()
        {
            // Arrange

            await userRepository.LoadTestData();

            // Act

            var result =
                await userRepository
                    .GetUserById(
                        Guid.NewGuid()
                            .ToString());

            // Assert

            Assert.IsNull(result);
        }

        // ------------------------
        // GET USER ID BY EMAIL
        // ------------------------

        [TestMethod]
        public async Task TestGetUserIdByEmailSuccess()
        {
            // Arrange

            await userRepository.LoadTestData();

            const string email =
                "andreas.pedersen@fitlife.dk";

            // Act

            var id =
                await userRepository
                    .GetUserIdByEmail(email);

            var user =
                await userRepository
                    .GetUserById(id);

            // Assert

            Assert.IsNotNull(id);

            Assert.IsNotNull(user);

            Assert.AreEqual(
                email,
                user.Email);
        }

        [TestMethod]
        public async Task TestGetUserIdByEmailReturnsNull()
        {
            // Arrange

            await userRepository.LoadTestData();

            // Act

            var result =
                await userRepository
                    .GetUserIdByEmail(
                        "doesnotexist@test.dk");

            // Assert

            Assert.IsNull(result);
        }

        // ------------------------
        // LOAD TEST DATA
        // ------------------------

        [TestMethod]
        public async Task TestLoadTestDataSuccess()
        {
            // Arrange + Act

            var result =
                await userRepository
                    .LoadTestData();

            var users =
                await userRepository
                    .GetAllUsers();

            // Assert

            Assert.IsTrue(result);

            Assert.AreEqual(
                UserTestData.users.Count,
                users.Count);
        }
    }
}