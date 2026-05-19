namespace PTServiceAPI.Models
{
    public class PersonalTrainingProgram
    {
        public PersonalTrainingProgram(Guid memberId, Guid personalTrainerId)
        {
            MemberId = memberId;
            PersonalTrainerId = personalTrainerId;
        }

        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid MemberId { get; private set; }
        public Guid PersonalTrainerId { get; private set; }
        public List<Message> SentMessages { get; private set; } = new();
        public List<Message> ReceivedMessages { get; private set; } = new();
        public List<Session> ActiveSessions { get; private set; } = new();

        public void SendMessage(Message message) => SentMessages.Add(message);
        public void ReceiveMessage(Message message) => ReceivedMessages.Add(message);
        public void ViewPersonalTrainingProgram() { }
    }
}