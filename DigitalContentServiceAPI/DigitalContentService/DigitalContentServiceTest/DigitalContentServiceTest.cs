using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories;
using DigitalContentServiceAPI.Repositories.Interfaces;
using DigitalContentServiceAPI.Testdata;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace DigitalContentServiceTest
{
    [TestClass]
    public sealed class DigitalContentServiceTest
    {
        private List<WorkoutProgram> workoutPrograms;
        private IWorkoutProgramRepository workoutProgramRepository;
        private IWorkoutVideoRepository workoutVideoRepository;

        [TestInitialize]
        public void InitializeTest()
        {
            workoutProgramRepository = new WorkoutProgramRepository();
            workoutVideoRepository = new WorkoutVideoRepository();
        }

        
        // POST
        
        [TestMethod]
        public void TestInsertWorkoutProgramSuccess()
        {
            //Arrange
            WorkoutProgram workoutProgram = new WorkoutProgram();
            
            //Act
            workoutProgramRepository.InsertWorkoutProgram(workoutProgram);

			//Assert
            Assert.IsTrue(TestData.workoutPrograms.Any());
		}

        [TestMethod]
        public void TestInsertWorkoutVideoSuccess()
        {
            //Arrange
            WorkoutVideo workoutVideo = new WorkoutVideo();
            
            //Act
            workoutVideoRepository.InsertWorkoutVideo(workoutVideo);
            
            //Assert
            Assert.IsTrue(TestData.workoutVideos.Contains(workoutVideo));
        }
        
        
        // GET
        
        [TestMethod]
        public void TestGetWorkoutProgramSuccess()
        {
          //Arrange
          WorkoutProgram workoutProgram = new WorkoutProgram();
          
          //Act
          var insertProgram = workoutProgramRepository.InsertWorkoutProgram(workoutProgram);
          var getProgram = workoutProgramRepository.GetWorkoutProgram(workoutProgram.Id);
          
          //Assert
          Assert.AreEqual(insertProgram, getProgram);
        }
        
        [TestMethod]
        public async Task TestGetWorkoutVideoSuccess()
        {
            // Arrange
            WorkoutVideo workoutVideo = new WorkoutVideo();

            await workoutVideoRepository.InsertWorkoutVideo(workoutVideo);

            // Act
            var getVideo = await workoutVideoRepository.GetWorkoutVideo(workoutVideo.Id);

            // Assert
            Assert.AreEqual(workoutVideo.Id, getVideo.Id);
        }
        
        // PUT

        [TestMethod]
        public void TestChangeSetsAndRepsSuccess()
        {
            //Arrange
            var program = TestData.workoutPrograms.First();
            
            var workouts = program.Workouts.First();
            var actions = workouts.ExerciseActions.First();
            
            var changedReps = actions.AmountOfReps + 1;
            var changedSets = actions.AmountOfSets + 1;
            
            //Act
            actions.ChangeSetsAndReps(changedReps, changedSets);
            
            //Assert
            Assert.AreEqual(changedReps, actions.AmountOfReps);
            Assert.AreEqual(changedSets, actions.AmountOfSets);
        }

        
        
        
	}
}
