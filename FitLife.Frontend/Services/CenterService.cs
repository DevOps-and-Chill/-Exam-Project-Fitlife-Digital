using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class CenterService
{
    // Denne service håndterer centre i frontend-prototypen.
    // TODO:
    // Senere skal data hentes fra backend/API/database.

    // Midlertidig mock data til centre
    private readonly List<Center> _centers = new()
    {
        new Center
        {
            Name = "Aarhus C",
            City = "Aarhus",
            Address = "Banegårdsgade 12"
        },

        new Center
        {
            Name = "Randers",
            City = "Randers",
            Address = "Vestergade 24"
        },

        new Center
        {
            Name = "Silkeborg",
            City = "Silkeborg",
            Address = "Torvet 8"
        },

        new Center
        {
            Name = "Horsens",
            City = "Horsens",
            Address = "Søndergade 17"
        }
    };

    // Returnerer alle centre
    // Bruges af frontend til centeroversigter.
    public List<Center> GetCenters()
    {
        return _centers;
    }
}