namespace FitLife.Frontend.Models.DigitalContent
{
	public abstract class DigitalContent
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string PartitionKey { get; set; } = "DigitalContent";
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		public bool ActiveContent { get; set; } = true;
		public Guid CreatedBy { get; set; }

		public void ChangeArchiveStateAsync(bool activeContent)
		{
			if (activeContent)
				activeContent = false;
			else
				activeContent = true;
		}
	}
}
