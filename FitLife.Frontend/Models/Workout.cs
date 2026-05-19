public class Workout
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string Goal { get; set; } = "";

    public int DurationInMinutes { get; set; }

    public List<ExerciseAction> Exercises { get; set; } = new();
}