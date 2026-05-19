using Castle.Core.Logging;
using FacilityServiceAPI.Controllers;
using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories;
using FacilityServiceAPI.TestData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;
using System.Threading.Tasks;

namespace FacilityServiceTest
{
	[TestClass]
	public sealed class FacilityRepositoryAndControllerTest
	{
		private List<Facility> facilitiesForTest;
		private ILogger<FacilityController> _logger;

		[TestInitialize]
		public void Setup()
		{
			var loggerFactory = LoggerFactory.Create(c => c.AddConsole());

			_logger = loggerFactory.CreateLogger<FacilityController>();

			facilitiesForTest = FacilityTestData.Facilities;
		}

		[TestMethod(DisplayName = "GetFacility")]
		public void TestGetFacilitySucessScenario()
		{
			//Arange
			var mockRepo = new Mock<IFacilityRepository>();
			mockRepo.Setup(r => r.GetFacilities()).Returns(Task.FromResult(facilitiesForTest));

			var controller = new FacilityController(_logger, mockRepo.Object);

			//Act
			var result = controller.GetFacilities().WaitAsync(CancellationToken.None).Result as OkObjectResult;
			var facilities = result.Value as IEnumerable<Facility>;

			//Assert
			Assert.IsNotNull(facilities);
			Assert.AreEqual(200, result.StatusCode);
			Assert.AreEqual(facilitiesForTest.Count, facilities.Count());
		}

		[TestMethod(DisplayName = "InsertFacility")]
		public async Task FacilityInsertedCorrectly()
		{
			//Arrange
			var facilityToAdd = new ExerciseGym("Test", "Test", "Test", "Test",
				new List<OpeningHoursSpecification>() {
					new OpeningHoursSpecification() {
						Closes = "",
						DayOfWeek = DayOfWeek.Monday,
						Opens = "" }},
				1, Guid.NewGuid());

			var mockRepo = new Mock<IFacilityRepository>();
			mockRepo.Setup(r => r.InsertFacility(It.IsAny<Facility>()))
				.Callback<Facility>(f => facilitiesForTest.Add(f));

			mockRepo.Setup(r => r.GetFacilities()).Returns(Task.FromResult(facilitiesForTest));

			var controller = new FacilityController(_logger,mockRepo.Object);

			//Act 
			var insertResult = controller.InsertFacility(facilityToAdd).WaitAsync(CancellationToken.None).Result as OkObjectResult;

			var getResult = controller.GetFacilities().WaitAsync(CancellationToken.None).Result as OkObjectResult;

			var data = getResult.Value as IEnumerable<Facility>;

			//Assert
			Assert.AreEqual(facilityToAdd, data.Last());
			Assert.AreEqual(7, data.Count());
		}

		[TestMethod]
		public async Task FacilityRemovedCorrectly()
		{
			//Arrange
			var facilityToRemove = facilitiesForTest.First();

			var mockRepo = new Mock<IFacilityRepository>();

			mockRepo.Setup(r => r.DeleteFacility(facilityToRemove.Id.ToString()))
				.Callback<string>(f => facilitiesForTest.Remove(facilitiesForTest.Single(x => x.Id.ToString() == facilityToRemove.Id.ToString())));

			var controller = new FacilityController(_logger ,mockRepo.Object);

			//Act
			await controller.DeleteFacility(facilityToRemove.Id.ToString());

			Assert.AreEqual(5, facilitiesForTest.Count());
			Assert.AreEqual(false, facilitiesForTest.Contains(facilityToRemove));
		}

		[TestMethod]
		public async Task FacilityUpdatedCorrectly()
		{
			//Arrange
			var facilityToUpdate = facilitiesForTest.First();

			facilityToUpdate.Name = "UpdateTest";

			var mockRepo = new Mock<IFacilityRepository>();

			mockRepo.Setup(r => r.UpdateFacility(facilityToUpdate)).Callback(() =>
			{
				facilitiesForTest.Remove(facilityToUpdate);
				facilitiesForTest.Add(facilityToUpdate);
			});

			var controller = new FacilityController(_logger,mockRepo.Object);

			//Act

			await controller.UpdateFacility(facilityToUpdate);

			//Assert

			Assert.AreEqual(facilityToUpdate, facilitiesForTest.Last());
		}

		[TestMethod]
		public async Task GetSingleFacilityCorrectly()
		{
			//Arrange

			var expectedFacility = facilitiesForTest.Last();

			var mockRepo = new Mock<IFacilityRepository>();

			mockRepo.Setup(r => r.GetFacility(expectedFacility.Id.ToString())).Returns(Task.FromResult(expectedFacility));

			var controller = new FacilityController(_logger, mockRepo.Object);

			//Act

			var actualFacility = controller.GetFacility(expectedFacility.Id.ToString()).Result as ObjectResult;

			//Assert

			Assert.IsNotNull(actualFacility);
			Assert.AreEqual(expectedFacility, actualFacility.Value);
		}
	}
}
