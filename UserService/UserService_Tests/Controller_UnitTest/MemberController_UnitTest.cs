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
    public class MemberControllerTest
    {
        private Mock<UserServiceAPI.Repositories.Interfaces.IMemberRepository> memberRepo;
        private Mock<ILogger<MemberController>> logger;

        private MemberController controller;

        [TestInitialize]
        public void Initialize()
        {
            memberRepo =
                new Mock<IMemberRepository>();

            logger =
                new Mock<ILogger<MemberController>>();

            controller =
                new MemberController(
                    memberRepo.Object,
                    logger.Object);
        }

        private Member CreateMember()
        {
            return new Member(
                UserRole.Member,
                "Anders",
                "Test",
                new DateTime(
                    1995,
                    1,
                    1),
                "Street",
                "12345678",
                "member@test.dk",
                Guid.NewGuid(),
                true,
                MembershipType.Standard,
                MembershipOptional.Pizza);
        }

        // ----------------------
        // GET MEMBER
        // ----------------------

        [TestMethod]
        public async Task GetMemberById_ReturnsOk()
        {
            // Arrange

            var member =
                CreateMember();

            memberRepo
                .Setup(
                    x =>
                    x.GetMemberById(
                        member.Id))
                .ReturnsAsync(
                    member);

            // Act

            var result =
                await controller
                    .GetMemberById(
                        member.Id);

            // Assert

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        OkObjectResult));

            var ok =
                (OkObjectResult)
                result;

            Assert
                .AreEqual(
                    member,
                    ok.Value);
        }

        [TestMethod]
        public async Task GetMemberById_ReturnsNotFound()
        {
            // Arrange

            memberRepo
                .Setup(
                    x =>
                    x.GetMemberById(
                        It.IsAny<string>()))
                .ReturnsAsync(
                    (Member)null);

            // Act

            var result =
                await controller
                    .GetMemberById(
                        "missing");

            // Assert

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        NotFoundObjectResult));
        }

        // ----------------------
        // UPSERT
        // ----------------------

        [TestMethod]
        public async Task UpsertMember_ReturnsOk()
        {
            // Arrange

            var member =
                CreateMember();

            memberRepo
                .Setup(
                    x =>
                    x.UpsertMember(
                        member))
                .ReturnsAsync(
                    member);

            // Act

            var result =
                await controller
                    .UpsertMember(
                        member);

            // Assert

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        OkObjectResult));
        }

        [TestMethod]
        public async Task UpsertMember_ReturnsBadRequest()
        {
            // Arrange

            var member =
                CreateMember();

            memberRepo
                .Setup(
                    x =>
                    x.UpsertMember(
                        member))
                .ThrowsAsync(
                    new InvalidOperationException(
                        "Duplicate"));

            // Act

            var result =
                await controller
                    .UpsertMember(
                        member);

            // Assert

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        BadRequestObjectResult));
        }

        // ----------------------
        // DELETE
        // ----------------------

        [TestMethod]
        public async Task DeleteMember_ReturnsOk()
        {
            // Arrange

            var member =
                CreateMember();

            memberRepo
                .Setup(
                    x =>
                    x.DeleteMember(
                        member.Id))
                .ReturnsAsync(
                    member);

            // Act

            var result =
                await controller
                    .DeleteMember(
                        member.Id);

            // Assert

            Assert
                .IsInstanceOfType(
                    result,
                    typeof(
                        OkObjectResult));
        }
    }
}