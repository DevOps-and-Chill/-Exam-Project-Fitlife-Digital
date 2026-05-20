namespace FitLife.Frontend.Models;

public class Trainer
{
    public string Id { get; set; } = "";

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public bool IsAvailable { get; set; }

    public string Specialty { get; set; } = "";
}