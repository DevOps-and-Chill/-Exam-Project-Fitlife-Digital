using FacilityServiceAPI.Controllers;
using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories.Interfaces;
using FacilityServiceAPI.TestData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FacilityServiceTest
{
    [TestClass]
    public sealed class FacilityRepositoryAndControllerTest
    {
        private List<Facility> facilitiesForTest;
        private ILogger<FacilityController> _facilityLogger;
        private ILogger<ExerciseGymController> _exerciseGymLogger;

        [TestInitialize]
        public void Setup()
        {
            var loggerFactory =
                LoggerFactory.Create(c => c.AddConsole());

            _facilityLogger =
                loggerFactory.CreateLogger<FacilityController>();

            _exerciseGymLogger =
                loggerFactory.CreateLogger<ExerciseGymController>();

            facilitiesForTest = new List<Facility>();

            facilitiesForTest.AddRange(
                FacilityTestData.ExerciseGyms);

            facilitiesForTest.AddRange(
                FacilityTestData.SwimmingPools);
        }

        [TestMethod(DisplayName = "Get all facilities successfully")]
        public void TestGetFacilitySucessScenario()
        {
            //Arrange
            var mockRepo =
                new Mock<IFacilityRepository>();

            mockRepo.Setup(r => r.GetFacilities())
                .Returns(Task.FromResult(
                    facilitiesForTest));

            var controller =
                new FacilityController(
                    _facilityLogger,
                    mockRepo.Object);

            //Act
            var result =
                controller.GetFacilities()
                    .WaitAsync(CancellationToken.None)
                    .Result as OkObjectResult;

            var facilities =
                result.Value as IEnumerable<Facility>;

            //Assert
            Assert.IsNotNull(facilities);

            Assert.AreEqual(
                200,
                result.StatusCode);

            Assert.AreEqual(
                facilitiesForTest.Count,
                facilities.Count());
        }

        [TestMethod(DisplayName = "Insert exercise gym successfully")]
        public async Task FacilityInsertedCorrectly()
        {
            //Arrange
            var facilityToAdd =
                new ExerciseGym(
                    "Test",
                    "Test",
                    "Test",
                    "Test",
                    new List<OpeningHoursSpecification>()
                    {
                        new OpeningHoursSpecification()
                        {
                            Closes = "",
                            DayOfWeek = DayOfWeek.Monday,
                            Opens = ""
                        }
                    },
                    1,
                    Guid.NewGuid());

            var mockRepo =
                new Mock<IExerciseGymRepository>();

            mockRepo.Setup(r =>
                    r.InsertExerciseGym(
                        It.IsAny<ExerciseGym>()))
                .Callback<ExerciseGym>(f =>
                    facilitiesForTest.Add(f))
                .Returns(Task.CompletedTask);

            var controller =
                new ExerciseGymController(
                    _exerciseGymLogger,
                    mockRepo.Object);

            //Act
            await controller.InsertExerciseGym(
                facilityToAdd);

            var gyms =
                facilitiesForTest
                    .OfType<ExerciseGym>()
                    .ToList();

            //Assert
            Assert.AreEqual(
                facilityToAdd,
                gyms.Last());

            Assert.AreEqual(
                3,
                gyms.Count);
        }

        [TestMethod(DisplayName = "Delete facility successfully")]
        public async Task FacilityRemovedCorrectly()
        {
            //Arrange
            var facilityToRemove =
                facilitiesForTest.First();

            var mockRepo =
                new Mock<IFacilityRepository>();

            mockRepo.Setup(r =>
                    r.DeleteFacility(
                        facilityToRemove.Id.ToString()))
                .Callback<string>(f =>
                    facilitiesForTest.Remove(
                        facilitiesForTest.Single(
                            x => x.Id.ToString()
                                 == facilityToRemove.Id.ToString())))
                .Returns(Task.CompletedTask);

            var controller =
                new FacilityController(
                    _facilityLogger,
                    mockRepo.Object);

            //Act
            await controller.DeleteFacility(
                facilityToRemove.Id.ToString());

            //Assert
            Assert.AreEqual(
                3,
                facilitiesForTest.Count());

            Assert.AreEqual(
                false,
                facilitiesForTest.Contains(
                    facilityToRemove));
        }

        [TestMethod(DisplayName = "Update exercise gym successfully")]
        public async Task FacilityUpdatedCorrectly()
        {
            //Arrange
            var facilityToUpdate =
                facilitiesForTest
                    .OfType<ExerciseGym>()
                    .First();

            facilityToUpdate.Name =
                "UpdateTest";

            var mockRepo =
                new Mock<IExerciseGymRepository>();

            mockRepo.Setup(r =>
                    r.UpdateExerciseGym(
                        It.IsAny<ExerciseGym>()))
                .Returns(Task.CompletedTask);

            var controller =
                new ExerciseGymController(
                    _exerciseGymLogger,
                    mockRepo.Object);

            //Act
            await controller.UpdateExerciseGym(
                facilityToUpdate);

            //Assert
            var updatedGym =
                facilitiesForTest
                    .OfType<ExerciseGym>()
                    .Single(g => g.Id == facilityToUpdate.Id);

            Assert.AreEqual(
                "UpdateTest",
                updatedGym.Name);
        }

        [TestMethod(DisplayName = "Get single facility successfully")]
        public async Task GetSingleFacilityCorrectly()
        {
            //Arrange
            var expectedFacility =
                facilitiesForTest.Last();

            var mockRepo =
                new Mock<IFacilityRepository>();

            mockRepo.Setup(r =>
                    r.GetFacility(
                        expectedFacility.Id.ToString()))
                .Returns(Task.FromResult(
                    expectedFacility));

            var controller =
                new FacilityController(
                    _facilityLogger,
                    mockRepo.Object);

            //Act
            var actualFacility =
                controller.GetFacility(
                    expectedFacility.Id.ToString())
                .Result as ObjectResult;

            //Assert
            Assert.IsNotNull(actualFacility);

            Assert.AreEqual(
                expectedFacility,
                actualFacility.Value);
        }
    }
}