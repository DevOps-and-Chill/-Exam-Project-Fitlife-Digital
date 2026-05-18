namespace FitLife.Frontend.Models;

public class PersonalTrainingSession
{
    public int Id { get; set; }

    public string TrainerName { get; set; } = "";
    public DateTime Date { get; set; }

    public string Status { get; set; } = "";
}