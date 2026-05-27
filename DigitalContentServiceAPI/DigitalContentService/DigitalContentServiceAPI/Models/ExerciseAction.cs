using DigitalContentServiceAPI.Models.Enums;

namespace DigitalContentServiceAPI.Models;

public class ExerciseAction
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Description { get; set; }
    public ExerciseEquipment EquipmentRequired { get; set; }
    public List<WorkoutVideo> RelatedVideos { get; set; }  = new List<WorkoutVideo>();
    public int AmountOfSets { get; set; }
    public int AmountOfReps { get; set; }

    public void ChangeSetsAndReps(int amountOfReps, int amountOfSets)
    {
        if (AmountOfReps != amountOfReps)
            AmountOfReps = amountOfReps;
        
        if (AmountOfSets != amountOfSets)
            AmountOfSets = amountOfSets;
    }
}