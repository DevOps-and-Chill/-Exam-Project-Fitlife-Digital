using System;
using System.Collections.Generic;
using DigitalContentServiceAPI.Models;
using DigitalContentServiceAPI.Models.Enums;


namespace DigitalContentServiceAPI.Testdata
{
	public static class TestData
	{
		public static List<WorkoutVideo> workoutVideos = new List<WorkoutVideo>
		{
	new WorkoutVideo
	{
		Name = "Full Body Beginner Warmup",
		Description = "A light full body warmup suitable for beginners.",
		DateCreated = DateTime.UtcNow,
		DateModified = DateTime.UtcNow,
		CreatedBy = Guid.NewGuid(),

		ContentUrl = "https://videos.fitlife.com/fullbody-warmup.mp4",
		Duration = 10,
		UploadDate = DateTime.UtcNow.AddDays(-10),
		ThumbnailUrl = "https://images.fitlife.com/warmup-thumb.jpg",

		ExerciseEquipment = ExerciseEquipment.None
	},

	new WorkoutVideo
	{
		Name = "Kettlebell Fat Burn Workout",
		Description = "An intense kettlebell workout focused on fat burning.",
		DateCreated = DateTime.UtcNow,
		DateModified = DateTime.UtcNow,
		CreatedBy = Guid.NewGuid(),

		ContentUrl = "https://videos.fitlife.com/kettlebell-fatburn.mp4",
		Duration = 22,
		UploadDate = DateTime.UtcNow.AddDays(-5),
		ThumbnailUrl = "https://images.fitlife.com/kettlebell-thumb.jpg",

		ExerciseEquipment = ExerciseEquipment.Kettlebell
	},

	new WorkoutVideo
	{
		Name = "Bench Press Technique Guide",
		Description = "Learn correct bench press form and execution.",
		DateCreated = DateTime.UtcNow,
		DateModified = DateTime.UtcNow,
		CreatedBy = Guid.NewGuid(),

		ContentUrl = "https://videos.fitlife.com/benchpress-guide.mp4",
		Duration = 18,
		UploadDate = DateTime.UtcNow.AddDays(-20),
		ThumbnailUrl = "https://images.fitlife.com/bench-thumb.jpg",

		ExerciseEquipment = ExerciseEquipment.Bench
	},

	new WorkoutVideo
	{
		Name = "Threadmill Cardio Session",
		Description = "Moderate cardio workout using a threadmill.",
		DateCreated = DateTime.UtcNow,
		DateModified = DateTime.UtcNow,
		CreatedBy = Guid.NewGuid(),

		ContentUrl = "https://videos.fitlife.com/threadmill-cardio.mp4",
		Duration = 30,
		UploadDate = DateTime.UtcNow.AddDays(-3),
		ThumbnailUrl = "https://images.fitlife.com/threadmill-thumb.jpg",

		ExerciseEquipment = ExerciseEquipment.Threadmill
	}
};

		public static List<WorkoutProgram> workoutPrograms = new List<WorkoutProgram>
		{
	new WorkoutProgram
	{
		Name = "Beginner Weight Loss Program",
		Description = "A beginner-friendly program focused on calorie burning and endurance.",
		DateCreated = DateTime.UtcNow,
		DateModified = DateTime.UtcNow,
		CreatedBy = Guid.NewGuid(),

		GeneralEducationLevel = DifficultyLevel.Light,
		TimeRequired = 240,
		ProgramGoal = TrainingGoal.WeightLoss,

		Workouts = new List<Workout>
		{
			new Workout
			{
				EducationLevel = DifficultyLevel.Light,
				TimeRequired = 45,
				WorkoutGoal = TrainingGoal.WeightLoss,

				ExerciseActions = new List<ExerciseAction>
				{
					new ExerciseAction
					{
						Description = "Light cardio warmup on threadmill",
						EquipmentRequired = ExerciseEquipment.Threadmill,
						AmountOfReps = 1,
						AmountOfSets = 1,

						RelatedVideos = new List<WorkoutVideo>
						{
							workoutVideos[3]
						}
					},

					new ExerciseAction
					{
						Description = "Kettlebell swings",
						EquipmentRequired = ExerciseEquipment.Kettlebell,
						AmountOfReps = 15,
						AmountOfSets = 3,

						RelatedVideos = new List<WorkoutVideo>
						{
							workoutVideos[1]
						}
					}
				}
			}
		}
	},

	new WorkoutProgram
	{
		Name = "Upper Body Strength Program",
		Description = "Intermediate strength training program targeting upper body muscles.",
		DateCreated = DateTime.UtcNow,
		DateModified = DateTime.UtcNow,
		CreatedBy = Guid.NewGuid(),

		GeneralEducationLevel = DifficultyLevel.Moderate,
		TimeRequired = 360,
		ProgramGoal = TrainingGoal.Strength,

		Workouts = new List<Workout>
		{
			new Workout
			{
				EducationLevel = DifficultyLevel.Moderate,
				TimeRequired = 60,
				WorkoutGoal = TrainingGoal.Strength,

				ExerciseActions = new List<ExerciseAction>
				{
					new ExerciseAction
					{
						Description = "Bench press",
						EquipmentRequired = ExerciseEquipment.Bench,
						AmountOfReps = 8,
						AmountOfSets = 5,

						RelatedVideos = new List<WorkoutVideo>
						{
							workoutVideos[2]
						}
					},

					new ExerciseAction
					{
						Description = "Push-ups",
						EquipmentRequired = ExerciseEquipment.None,
						AmountOfReps = 20,
						AmountOfSets = 4,

						RelatedVideos = new List<WorkoutVideo>
						{
							workoutVideos[0]
						}
					}
				}
			}
		}
	},

	new WorkoutProgram
	{
		Name = "Expert Muscle Gain Program",
		Description = "High intensity muscle gain program for experienced athletes.",
		DateCreated = DateTime.UtcNow,
		DateModified = DateTime.UtcNow,
		CreatedBy = Guid.NewGuid(),

		GeneralEducationLevel = DifficultyLevel.Expert,
		TimeRequired = 540,
		ProgramGoal = TrainingGoal.MuscleGain,

		Workouts = new List<Workout>
		{
			new Workout
			{
				EducationLevel = DifficultyLevel.Expert,
				TimeRequired = 90,
				WorkoutGoal = TrainingGoal.MuscleGain,

				ExerciseActions = new List<ExerciseAction>
				{
					new ExerciseAction
					{
						Description = "Heavy machine chest press",
						EquipmentRequired = ExerciseEquipment.Machine,
						AmountOfReps = 10,
						AmountOfSets = 5,

						RelatedVideos = new List<WorkoutVideo>
						{
							workoutVideos[2]
						}
					},

					new ExerciseAction
					{
						Description = "Advanced kettlebell circuit",
						EquipmentRequired = ExerciseEquipment.Kettlebell,
						AmountOfReps = 20,
						AmountOfSets = 4,

						RelatedVideos = new List<WorkoutVideo>
						{
							workoutVideos[1]
						}
					}
				}
			}
		}
	}
};
	}
}
