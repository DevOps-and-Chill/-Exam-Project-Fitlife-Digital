using PTServiceAPI.Models.Enums;

namespace PTServiceAPI.Models
{
	public class Session
	{
		public Session(Guid memberId, Guid personalTrainerId, Guid centerId, TrainingGoal goal, DateTime startDate, DateTime endDate)
		{
			MemberId = memberId;
			PersonalTrainerId = personalTrainerId;
			CenterId = centerId;
			Goal = goal;
			StartDate = startDate;
			EndDate = endDate;
		}

		public Guid Id { get; init; } = Guid.NewGuid();
		public Guid MemberId { get; private set; }
		public Guid PersonalTrainerId { get; private set; }
		public Guid CenterId { get; private set; }
		public TrainingGoal Goal { get; private set;}
		public SessionStatus Status { get; private set; }
		public DateTime StartDate { get; private set; }
		public DateTime EndDate { get; private set; }
		public DateTime CreatedDate { get; init; } = DateTime.UtcNow;

		//Bruges af CosmosDB til at gruppere og fordele data i containeren
		public string PartitionKey { get; set; } = "sessions";
		
		public void CompleteProgram() => Status = SessionStatus.Completed;
		public void CancelProgram() => Status = SessionStatus.Cancelled;
		public void SetStartDate(DateTime date) => StartDate = date;
		public void SetEndDate(DateTime date) => EndDate = date;
	}
}