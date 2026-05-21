using DigitalContentServiceAPI.Models.Enums;

namespace DigitalContentServiceAPI.Models;

public class WorkoutVideo
{
    public string ContentUrl { get; set; }
    public int Duration  { get; set; }
    public DateTime UploadDate { get; set; }
    public string ThumbnailUrl { get; set; }
    public ExerciseEquipment ExerciseEquipment { get; set; } = ExerciseEquipment.None;
}