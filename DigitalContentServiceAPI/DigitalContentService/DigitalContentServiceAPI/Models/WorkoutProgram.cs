using DigitalContentServiceAPI.Models.Enums;

namespace DigitalContentServiceAPI.Models;

public class WorkoutProgram : DigitalContent
{
    public DifficultyLevel GeneralEducationLevel { get; set; }
    public int TimeRequired {get; set;}
    public TrainingGoal ProgramGoal { get; set; }
    public List<Workout> Workouts { get; set; }
}