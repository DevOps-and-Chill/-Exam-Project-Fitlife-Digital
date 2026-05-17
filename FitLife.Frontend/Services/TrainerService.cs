using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class TrainerService
{
    // Denne service håndterer personlige trænere og PT-funktionalitet.
    // Service-laget fungerer som bindeled mellem frontend og data.
    // TODO:
    // Senere skal denne service kommunikere med backend/API/database.

    // Midlertidig mock data til trænere
    // TODO:
    // Skal senere hentes fra backend/database.
    private readonly List<Trainer> _trainers = new()
    {
        new Trainer
        {
            Name = "Frederikke",

            // Kort beskrivelse som vises i UI
            Description = "Styrketræning, holdtræning og motivation.",

            // Trænerens specialeområde
            Specialty = "Styrketræning",

            // Bruges til bookinglogik i frontend
            IsAvailable = true
        },

        new Trainer
        {
            Name = "Jonas",
            Description = "Vægttab, teknik og personlige programmer.",
            Specialty = "Vægttab",
            IsAvailable = false
        },

        new Trainer
        {
            Name = "Mikkel",
            Description = "Kondition, spinning og funktionel træning.",
            Specialty = "Kondition",
            IsAvailable = true
        }
    };

    // Returnerer alle trænere
    // Bruges af frontend til at vise oversigt over PT-trænere.
    // TODO:
    // Senere skal data hentes via API/backend.
    public List<Trainer> GetTrainers()
    {
        return _trainers;
    }

    // Midlertidig ventelistefunktion
    // Bruges når en træner ikke er ledig.
    // TODO:
    // Skal senere gemmes i backend/database.
    public string JoinWaitlist(Trainer trainer)
    {
        return $"Du er nu tilmeldt ventelisten hos {trainer.Name}.";
    }

    // Midlertidig profilfunktion
    // TODO:
    // Senere kan denne metode hente detaljeret trænerprofil fra backend.
    public string ShowProfile(Trainer trainer)
    {
        return $"Profil for {trainer.Name} vises ikke som separat side endnu.";
    }

    // Opretter booking af personlig træning
    // TODO:
    // Senere skal booking gemmes via backend/API/database.
    public string BookSession(Trainer trainer, DateTime date, string time)
    {
        return $"Du har booket en session med {trainer.Name} den {date:dd/MM} kl. {time}.";
    }

    // Midlertidig beskedfunktion
    // TODO:
    // Senere skal denne kobles til MessageService/backend.
    public string SendMessage(Trainer trainer)
    {
        return $"Din besked til {trainer.Name} er sendt.";
    }
}