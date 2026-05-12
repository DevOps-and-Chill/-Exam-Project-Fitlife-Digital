using FacilityServiceAPI.Controllers;
using FacilityServiceAPI.Models;
using FacilityServiceAPI.Repositories;
using FacilityServiceAPI.TestData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;

namespace FacilityServiceTest
{
	[TestClass]
	public sealed class FacilityRepositoryAndControllerTest
	{
		private List<Facility> facilitiesForTest;

		[TestInitialize]
		public void Setup()
		{
			facilitiesForTest = FacilityTestData.Facilities;
		}

		[TestMethod(DisplayName = "GetFacility")]
		public void TestGetFacilitySucessScenario()
		{
			//Arange
			var mockRepo = new Mock<IFacilityRepository>();
			mockRepo.Setup(r => r.GetFacilities()).Returns(Task.FromResult(facilitiesForTest));

			var controller = new FacilityController(mockRepo.Object);

			//Act
			var result = controller.GetFacilities().Result as OkObjectResult;
			var facilities = result.Value as IEnumerable<Facility>;

			//Assert
			Assert.IsNotNull(facilities);
			Assert.AreEqual(200, result.StatusCode);
			Assert.AreEqual(facilitiesForTest.Count, facilities.Count());
		}

		[TestMethod(DisplayName = "InsertFacility")]
		public void FacilityInsertedCorrectly()
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

			var controller = new FacilityController(mockRepo.Object);

			//Act 
			var insertResult = controller.InsertFacility(facilityToAdd).Result as OkObjectResult;

			var getResult = controller.GetFacilities().Result as OkObjectResult;
			var data = getResult.Value as IEnumerable<Facility>;

			//Assert
			Assert.AreEqual(facilityToAdd, data.Last());
			Assert.AreEqual(7, data.Count());
		}
	}
}
