namespace RapportServiceAPI.Models
{
    public class Lagring
    {
        public Lagring(string dataType, double value)
        {
            DataType = dataType;
            Value = value;
        }

        public Guid Id { get; init; } = Guid.NewGuid();
        public string DataType { get; private set; }
        public double Value { get; private set; }
        public DateTime RecordedAt { get; init; } = DateTime.UtcNow;

        //JBS: Gemmer et nyt datapunkt
        public void StoreDataPoint(double value, string type)
        {
            Value = value;
            DataType = type;
        }

        //JBS: Sletter datapunktet - det håndteres gennem repository
        public void DeleteDataPoint() {}
    }
}