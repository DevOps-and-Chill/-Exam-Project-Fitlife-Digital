using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class SessionService
{
    // Denne service håndterer holdtræning/sessioner i frontend-prototypen.
    // Service-laget bruges som bindeled mellem UI og data.
    // TODO:
    // Senere skal denne service kommunikere med backend/API/database.

    // Midlertidig mock data til hold
    // TODO:
    // Skal senere hentes fra backend/database.
    private readonly List<TrainingSession> _sessions = new()
    {
        new TrainingSession
        {
            Title = "Yoga",

            // Instruktøren på holdet
            InstructorName = "Frederikke",

            // Tidspunkt for holdstart
            StartTime = DateTime.Today.AddHours(17),

            // Maks antal deltagere
            Capacity = 12,

            // Antal nuværende tilmeldte
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

    // Returnerer alle hold/sessioner
    // Bruges af frontend til at vise holdoversigt.
    // TODO:
    // Senere skal data hentes fra backend/API.
    public List<TrainingSession> GetSessions()
    {
        return _sessions;
    }

    // Tilmelder bruger til et hold
    // TODO:
    // Senere skal tilmelding gemmes i backend/database.
    public string Register(TrainingSession session)
    {
        // Marker at brugeren er tilmeldt
        session.IsUserSignedUp = true;

        // Øger antal tilmeldte deltagere
        session.RegisteredCount++;

        return $"Du er nu tilmeldt {session.Title}.";
    }

    // Afmelder bruger fra et hold
    // TODO:
    // Senere skal afmelding gemmes i backend/database.
    public string Unregister(TrainingSession session)
    {
        // Marker at brugeren ikke længere er tilmeldt
        session.IsUserSignedUp = false;

        // Reducerer antal tilmeldte deltagere
        session.RegisteredCount--;

        return $"Du er nu afmeldt {session.Title}.";
    }

    // Midlertidig venteliste-funktion
    // TODO:
    // Skal senere håndteres via backend/API med rigtig venteliste-logik.
    public string JoinWaitlist(TrainingSession session)
    {
        return $"Du er nu tilmeldt ventelisten til {session.Title}.";
    }
}