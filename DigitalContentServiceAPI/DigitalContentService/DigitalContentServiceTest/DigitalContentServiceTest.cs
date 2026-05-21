using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories.Interfaces;

namespace DigitalContentServiceTest
{
    [TestClass]
    public sealed class DigitalContentServiceTest
    {
        private List<WorkoutProgram> workoutPrograms;
        private IWorkoutProgramRepository workoutProgramRepository;

        [TestInitialize]
        public void InitializeTest()
        {
            workoutPrograms = new List<WorkoutProgram>();
            
        }

        [TestMethod]
        public void TestInsertWrokoutProgramSucess()
        {
            //Arrange
            WorkoutProgram workoutProgram = new WorkoutProgram();
            //Act
            workoutProgramRepository.InsertWorkoutProgram(workoutProgram);

			//Assert
            Assert.IsTrue(workoutPrograms.Any());
		}
	}
}
