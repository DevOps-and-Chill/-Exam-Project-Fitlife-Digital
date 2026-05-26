using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UserServiceAPI.Controllers;
using UserServiceAPI.Models;
using UserServiceAPI.Models.Enums;
using UserServiceAPI.Repositories.Interfaces;

namespace UserService_Tests.Controller_UnitTest
{
    [TestClass]
    public class UserControllerTest
    {
        private Mock<IUserRepository> repo;
        private Mock<ILogger<UserController>> logger;

        private UserController controller;

        [TestInitialize]
        public void Initialize()
        {
            repo =
                new Mock<IUserRepository>();

            logger =
                new Mock<ILogger<UserController>>();

            controller =
                new UserController(
                    repo.Object,
                    logger.Object);
        }

        [TestMethod]
        public async Task GetAllUsers_ReturnsOk()
        {
            repo
                .Setup(
                    x =>
                    x.GetAllUsers())
                .ReturnsAsync(
                    new List<User>());

            var result =
                await controller
                    .GetAllUsers();

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        OkObjectResult));
        }

        [TestMethod]
        public async Task GetUserById_ReturnsOk()
        {
            var user =
                new Mock<User>();

            repo
                .Setup(
                    x =>
                    x.GetUserById(
                        It.IsAny<string>()))
                .ReturnsAsync(
                    user.Object);

            var result =
                await controller
                    .GetUserById(
                        "1");

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        OkObjectResult));
        }

        [TestMethod]
        public async Task GetUserIdByEmail_ReturnsNotFound()
        {
            repo
                .Setup(
                    x =>
                    x.GetUserIdByEmail(
                        It.IsAny<string>()))
                .ReturnsAsync(
                    (string)null);

            var result =
                await controller
                    .GetUserIdByEmail(
                        "missing@mail.dk");

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        NotFoundResult));
        }

        [TestMethod]
        public async Task AddData_ReturnsOk()
        {
            repo
                .Setup(
                    x =>
                    x.LoadTestData())
                .ReturnsAsync(
                    true);

            var result =
                await controller
                    .AddData();

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        OkResult));
        }
    }
}