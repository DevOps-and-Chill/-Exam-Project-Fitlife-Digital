using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class TrainerService
{
    private readonly List<Trainer> _trainers = new()
    {
        new Trainer
        {
            Name = "Frederikke",
            Description = "Styrketræning, holdtræning og motivation.",
            Specialty = "Styrketræning",
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

    public List<Trainer> GetTrainers()
    {
        return _trainers;
    }

    public string JoinWaitlist(Trainer trainer)
    {
        return $"Du er nu tilmeldt ventelisten hos {trainer.Name}.";
    }

    public string ShowProfile(Trainer trainer)
    {
        return $"Profil for {trainer.Name} vises ikke som separat side endnu.";
    }

    public string BookSession(Trainer trainer, DateTime date, string time)
    {
        return $"Du har booket en session med {trainer.Name} den {date:dd/MM} kl. {time}.";
    }

    public string SendMessage(Trainer trainer)
    {
        return $"Din besked til {trainer.Name} er sendt.";
    }
}