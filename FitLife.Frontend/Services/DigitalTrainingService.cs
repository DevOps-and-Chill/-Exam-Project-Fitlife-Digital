using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class DigitalTrainingService
{
    private readonly List<TrainingVideo> _videos = new()
    {
        new TrainingVideo
        {
            Title = "Squat teknik",
            Description = "Kort video om korrekt squat-teknik og typiske fejl.",
            Category = "Styrke"
        },
        new TrainingVideo
        {
            Title = "Core træning",
            Description = "Øvelser til mave, ryg og stabilitet.",
            Category = "Core"
        },
        new TrainingVideo
        {
            Title = "Opvarmning",
            Description = "En simpel opvarmningsrutine før træning.",
            Category = "Opvarmning"
        }
    };

    private readonly List<TrainingProgram> _programs = new()
    {
        new TrainingProgram
        {
            Title = "Begynderprogram",
            Description = "Et simpelt program til nye medlemmer.",
            Level = "Begynder"
        },
        new TrainingProgram
        {
            Title = "Styrketræning 3 dage",
            Description = "Program med fokus på styrke og progression.",
            Level = "Øvet"
        },
        new TrainingProgram
        {
            Title = "Vægttab og kondition",
            Description = "Program med fokus på cardio og basis styrke.",
            Level = "Begynder"
        }
    };

    public List<TrainingVideo> GetVideos()
    {
        return _videos;
    }

    public List<TrainingProgram> GetPrograms()
    {
        return _programs;
    }
}