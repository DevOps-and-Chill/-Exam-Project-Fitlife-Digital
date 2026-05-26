using FitLife.Frontend.Models.DigitalContent.Enums;

namespace FitLife.Frontend.Models.DigitalContent
{
	public class WorkoutProgram : DigitalContent
	{
		public DifficultyLevel GeneralEducationLevel { get; set; }
		public int TimeRequired { get; set; }
		public TrainingGoal ProgramGoal { get; set; }
		public List<Workout> Workouts { get; set; }
	}
}
