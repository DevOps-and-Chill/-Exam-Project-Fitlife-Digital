using DigitalContentServiceAPI.Data;
using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Repositories;
using DigitalContentServiceAPI.Repositories.Interfaces;
using DigitalContentServiceAPI.Testdata;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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
            var options = new DbContextOptionsBuilder<DigitalContentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new DigitalContentDbContext(options);
            var logger = new NullLogger<WorkoutProgramRepository>();

            workoutProgramRepository = new WorkoutProgramRepository(context, new NullLogger<WorkoutProgramRepository>());
            workoutVideoRepository = new WorkoutVideoRepository(context, new NullLogger<WorkoutVideoRepository>()); 
        }
        
        // POST
        
        [TestMethod]
        public async Task TestInsertWorkoutProgramSuccess()
        {
            //Arrange
            var workoutProgram = TestData.workoutPrograms[0];
            
            //Act
            await workoutProgramRepository.InsertWorkoutProgramAsync(workoutProgram);

            var result = await workoutProgramRepository.GetWorkoutProgramAsync(workoutProgram.Id);
			//Assert
            
            Assert.IsNotNull(result);
            Assert.IsNotNull(workoutProgram.Workouts);
            Assert.AreEqual(workoutProgram.Id, result.Id);
            Assert.AreEqual(workoutProgram.Name, result.Name);
            Assert.AreEqual(workoutProgram.Description, result.Description);
            Assert.AreEqual(workoutProgram.ProgramGoal, result.ProgramGoal);
        }

        [TestMethod]
        public async Task TestInsertWorkoutVideoSuccess()
        {
            var workoutVideo = TestData.workoutVideos[0];
    
            await workoutVideoRepository.InsertWorkoutVideoAsync(workoutVideo);

            var result = await workoutVideoRepository.GetWorkoutVideoAsync(workoutVideo.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(workoutVideo.Id, result.Id);
        }
        
        
        // GET
        
        [TestMethod]
        public async Task TestGetWorkoutProgramSuccess()
        {
            var workoutProgram = TestData.workoutPrograms[0];
    
            await workoutProgramRepository.InsertWorkoutProgramAsync(workoutProgram);
            var result = await workoutProgramRepository.GetWorkoutProgramAsync(workoutProgram.Id);
    
            Assert.AreEqual(workoutProgram.Id, result.Id);
        }
        
        [TestMethod]
        public async Task TestGetWorkoutVideoSuccess()
        {
            // Arrange
            WorkoutVideo workoutVideo = new WorkoutVideo()
            {
                Name = TestData.workoutVideos[0].Name,
                Description = TestData.workoutVideos[0].Description,
                ActiveContent = TestData.workoutVideos[0].ActiveContent,
                DateModified = TestData.workoutVideos[0].DateModified,
                ContentUrl = TestData.workoutVideos[0].ContentUrl,
                Duration = TestData.workoutVideos[0].Duration,
                UploadDate = TestData.workoutVideos[0].UploadDate,
                ThumbnailUrl = TestData.workoutVideos[0].ThumbnailUrl,
                ExerciseEquipment = TestData.workoutVideos[0].ExerciseEquipment,
            };
            
            await workoutVideoRepository.InsertWorkoutVideoAsync(workoutVideo);

            // Act
            var getVideo = await workoutVideoRepository.GetWorkoutVideoAsync(workoutVideo.Id);

            // Assert
            Assert.AreEqual(workoutVideo.Id, getVideo.Id);
        }
        
        // PUT
        
        [TestMethod]
        public async Task TestUpdateWorkoutProgramSuccess()
        {
            var workoutProgram = new WorkoutProgram()
            {
                Name = TestData.workoutPrograms[0].Name,
                Description = TestData.workoutPrograms[0].Description,
                ActiveContent = TestData.workoutPrograms[0].ActiveContent,
                ProgramGoal = TestData.workoutPrograms[0].ProgramGoal,
                Workouts = TestData.workoutPrograms[0].Workouts,
                GeneralEducationLevel = TestData.workoutPrograms[0].GeneralEducationLevel,
                TimeRequired = TestData.workoutPrograms[0].TimeRequired,
                DateModified = TestData.workoutPrograms[0].DateModified
            };
            
            await workoutProgramRepository.InsertWorkoutProgramAsync(workoutProgram);
    
            var changedProgram = new WorkoutProgram()
            {
                Name = "Jeg er glad nu!",
                Description = "Opdateret",
                ActiveContent = TestData.workoutPrograms[0].ActiveContent,
                ProgramGoal = TestData.workoutPrograms[0].ProgramGoal,
                Workouts = TestData.workoutPrograms[0].Workouts,
                GeneralEducationLevel = TestData.workoutPrograms[0].GeneralEducationLevel,
                TimeRequired = TestData.workoutPrograms[0].TimeRequired,
                DateModified = TestData.workoutPrograms[0].DateModified
                
            };
            
            await workoutProgramRepository.UpdateWorkoutProgramAsync(workoutProgram.Id, changedProgram);
            var result = await workoutProgramRepository.GetWorkoutProgramAsync(workoutProgram.Id);
    
            Assert.AreEqual(changedProgram.Name, result.Name);
        }
        
        
        
        
        
	}
}
