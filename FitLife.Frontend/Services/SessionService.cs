using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class SessionService
{
    private readonly List<TrainingSession> _sessions = new()
    {
        new TrainingSession
        {
            Title = "Yoga",
            InstructorName = "Frederikke",
            StartTime = DateTime.Today.AddHours(17),
            Capacity = 12,
            RegisteredCount = 4
        },
        new TrainingSession
        {
            Title = "Spinning",
            InstructorName = "Mikkel",
            StartTime = DateTime.Today.AddHours(18),
            Capacity = 10,
            RegisteredCount = 10
        },
        new TrainingSession
        {
            Title = "Crossfit",
            InstructorName = "Jonas",
            StartTime = DateTime.Today.AddHours(19),
            Capacity = 15,
            RegisteredCount = 8
        }
    };

    public List<TrainingSession> GetSessions()
    {
        return _sessions;
    }

    public string Register(TrainingSession session)
    {
        session.IsUserSignedUp = true;
        session.RegisteredCount++;

        return $"Du er nu tilmeldt {session.Title}.";
    }

    public string Unregister(TrainingSession session)
    {
        session.IsUserSignedUp = false;
        session.RegisteredCount--;

        return $"Du er nu afmeldt {session.Title}.";
    }

    public string JoinWaitlist(TrainingSession session)
    {
        return $"Du er nu tilmeldt ventelisten til {session.Title}.";
    }
}