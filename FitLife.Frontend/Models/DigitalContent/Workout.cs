using FitLife.Frontend.Models.DigitalContent.Enums;

namespace FitLife.Frontend.Models.DigitalContent
{
	public class Workout
	{
		public DifficultyLevel EducationLevel { get; set; }
		public int TimeRequired { get; set; }
		public TrainingGoal WorkoutGoal { get; set; }
		public List<ExerciseAction> ExerciseActions { get; set; } = new List<ExerciseAction>();

		public void AddExerciseAction(ExerciseAction action)
		{
			ExerciseActions.Add(action);
		}
	}
}
