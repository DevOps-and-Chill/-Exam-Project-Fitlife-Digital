public class OpeningHoursSpecification
{
    public string DayOfWeek { get; set; } = "";

    public TimeOnly Opens { get; set; }

    public TimeOnly Closes { get; set; }
}