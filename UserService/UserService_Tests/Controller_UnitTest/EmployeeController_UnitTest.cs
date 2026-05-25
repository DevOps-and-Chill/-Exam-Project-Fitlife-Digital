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
    public class EmployeeControllerTest
    {
        private Mock<IEmployeeRepository> repo;

        private Mock<
            ILogger<
                EmployeeController>>
            logger;

        private EmployeeController controller;

        [TestInitialize]
        public void Initialize()
        {
            repo =
                new Mock<IEmployeeRepository>();

            logger =
                new Mock<
                    ILogger<
                        EmployeeController>>();

            controller =
                new EmployeeController(
                    repo.Object,
                    logger.Object);
        }

        private Employee CreateEmployee()
        {
            return new Employee(
                UserRole.Employee,
                "Test",
                "User",
                DateTime.Now,
                "Street",
                "123",
                "test@test.dk",
                Guid.NewGuid(),
                true,
                EmployeeRole.Staffmember,
                false);
        }

        [TestMethod]
        public async Task GetEmployeeById_ReturnsOk()
        {
            var employee =
                CreateEmployee();

            repo
                .Setup(
                    x =>
                    x.GetEmployeeById(
                        employee.Id))
                .ReturnsAsync(
                    employee);

            var result =
                await controller
                    .GetEmployeeById(
                        employee.Id);

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        OkObjectResult));
        }

        [TestMethod]
        public async Task GetEmployeeById_ReturnsNotFound()
        {
            repo
                .Setup(
                    x =>
                    x.GetEmployeeById(
                        It.IsAny<string>()))
                .ReturnsAsync(
                    (Employee)null);

            var result =
                await controller
                    .GetEmployeeById(
                        "missing");

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        NotFoundObjectResult));
        }

        [TestMethod]
        public async Task UpsertEmployee_ReturnsBadRequest()
        {
            var employee =
                CreateEmployee();

            repo
                .Setup(
                    x =>
                    x.UpsertEmployee(
                        employee))
                .ThrowsAsync(
                    new InvalidOperationException());

            var result =
                await controller
                    .UpsertEmployee(
                        employee);

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        BadRequestObjectResult));
        }

        [TestMethod]
        public async Task DeleteEmployee_ReturnsNotFound()
        {
            repo
                .Setup(
                    x =>
                    x.DeleteEmployee(
                        It.IsAny<string>()))
                .ReturnsAsync(
                    (Employee)null);

            var result =
                await controller
                    .DeleteEmployee(
                        "missing");

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        NotFoundObjectResult));
        }
    }
}