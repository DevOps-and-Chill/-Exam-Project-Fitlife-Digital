using FitLife.Frontend.Models.DigitalContent.Enums;

namespace FitLife.Frontend.Models.DigitalContent
{
	public class WorkoutVideo : DigitalContent
	{
		public string ContentUrl { get; set; }
		public int Duration { get; set; }
		public DateTime UploadDate { get; set; }
		public string ThumbnailUrl { get; set; }
		public ExerciseEquipment ExerciseEquipment { get; set; } = ExerciseEquipment.None;
	}
}
