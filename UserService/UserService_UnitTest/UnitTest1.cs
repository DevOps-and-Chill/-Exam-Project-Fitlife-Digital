using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using UserServiceAPI.Data;
using UserServiceAPI.Models;
using UserServiceAPI.Repositories;
using UserServiceAPI.Repositories.Interfaces;
using UserServiceAPI.TestData;

namespace UserServiceTest
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

            var expected =
                UserTestData.users[0];

            // Act

            var result =
                await userRepository
                    .GetUserById(expected.Id);

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual(
                expected.Id,
                result.Id);

            Assert.AreEqual(
                expected.Email,
                result.Email);

            Assert.AreEqual(
                expected.GivenName,
                result.GivenName);
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

            var expected =
                UserTestData.users[0];

            // Act

            var result =
                await userRepository
                    .GetUserIdByEmail(
                        expected.Email);

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual(
                expected.Id,
                result);
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