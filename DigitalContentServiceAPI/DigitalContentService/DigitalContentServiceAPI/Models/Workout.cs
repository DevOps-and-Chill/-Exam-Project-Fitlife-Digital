using DigitalContentServiceAPI.Models.Enums;

namespace DigitalContentServiceAPI.Models;

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