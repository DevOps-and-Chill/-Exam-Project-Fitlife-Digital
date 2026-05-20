namespace FitLife.Frontend.Models;

public class PersonalTrainingSession
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public Guid PersonalTrainerId { get; set; }
    public Guid CenterId { get; set; }

    public int Goal { get; set; }
    public int Status { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedDate { get; set; }

    public string PartitionKey { get; set; } = "sessions";
}