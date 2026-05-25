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
    public sealed class MemberRepositoryDBTest
    {
        private IMemberRepository memberRepository;

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
                MemberTestData.members);

            context.SaveChanges();

            memberRepository =
                new MemberRepositoryDB(context);
        }

        // -------------------
        // GET
        // -------------------

        [TestMethod]
        public async Task TestGetAllMembersSuccess()
        {
            var result =
                await memberRepository
                    .GetAllMembers();

            Assert.IsNotNull(result);

            Assert.AreEqual(
                MemberTestData.members.Count,
                result.Count);
        }

        [TestMethod]
        public async Task TestGetMemberByIdSuccess()
        {
            var expected =
                (await memberRepository
                    .GetAllMembers())
                    .First();

            var result =
                await memberRepository
                    .GetMemberById(
                        expected.Id);

            Assert.IsNotNull(result);

            Assert.AreEqual(
                expected.Email,
                result.Email);
        }

        [TestMethod]
        public async Task TestGetMemberByIdReturnsNull()
        {
            var result =
                await memberRepository
                    .GetMemberById(
                        Guid.NewGuid()
                            .ToString());

            Assert.IsNull(result);
        }

        // -------------------
        // UPDATE
        // -------------------

        [TestMethod]
        public async Task TestCancelMembershipSuccess()
        {
            var member =
                (await memberRepository
                    .GetAllMembers())
                    .First();

            var result =
                await memberRepository
                    .CancelMembershipForMember(
                        member.Id);

            Assert.IsFalse(
                result.ActiveMembership);
        }

        [TestMethod]
        public async Task TestSetAccountAsInactiveSuccess()
        {
            var member =
                (await memberRepository
                    .GetAllMembers())
                    .First();

            var result =
                await memberRepository
                    .SetAccountAsInactive(
                        member.Id);

            Assert.IsFalse(
                result.ActiveUser);
        }

        // -------------------
        // UPSERT
        // -------------------

        [TestMethod]
        public async Task TestCreateMemberSuccess()
        {
            var member =
                new Member(
                    UserServiceAPI.Models.Enums.UserRole.Member,
                    "Test",
                    "User",
                    new DateTime(
                        2000,
                        1,
                        1),
                    "Street",
                    "123",
                    "new@test.dk",
                    Guid.NewGuid(),
                    true,
                    UserServiceAPI.Models.Enums.MembershipType.Standard,
                    UserServiceAPI.Models.Enums.MembershipOptional.Pizza);

            var result =
                await memberRepository
                    .UpsertMember(
                        member);

            Assert.IsNotNull(
                result);

            Assert.AreEqual(
                member.Email,
                result.Email);
        }

        [TestMethod]
        public async Task TestUpdateMemberSuccess()
        {
            var member =
                (await memberRepository
                    .GetAllMembers())
                    .First();

            member.Email =
                "updated@test.dk";

            var result =
                await memberRepository
                    .UpsertMember(
                        member);

            Assert.AreEqual(
                "updated@test.dk",
                result.Email);
        }

        [TestMethod]
        public async Task TestCreateMemberDuplicateEmailThrows()
        {
            // Arrange

            var existing =
                (await memberRepository
                    .GetAllMembers())
                    .First();

            var member =
                new Member(
                    UserRole.Member,
                    "Duplicate",
                    "User",
                    DateTime.Now,
                    "Address",
                    "11111111",
                    existing.Email,
                    Guid.NewGuid(),
                    true,
                    MembershipType.Standard,
                    MembershipOptional.Pizza);

            // Act + Assert

            try
            {
                await memberRepository
                    .UpsertMember(
                        member);

                Assert.Fail(
                    "Expected InvalidOperationException");
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(
                    ex.Message.Contains(
                        "already in use"));
            }
        }

        // -------------------
        // DELETE
        // -------------------

        [TestMethod]
        public async Task TestDeleteMemberSuccess()
        {
            var member =
                (await memberRepository
                    .GetAllMembers())
                    .First();

            await memberRepository
                .DeleteMember(
                    member.Id);

            var result =
                await memberRepository
                    .GetMemberById(
                        member.Id);

            Assert.IsNull(
                result);
        }

        // -------------------
        // FILTER
        // -------------------

        [TestMethod]
        public async Task TestGetMembersByAffiliationSuccess()
        {
            var member =
                (await memberRepository
                    .GetAllMembers())
                    .First();

            var result =
                await memberRepository
                    .GetMembersByAffiliation(
                        member.Affiliation);

            Assert.IsTrue(
                result.Count > 0);
        }
    }
}