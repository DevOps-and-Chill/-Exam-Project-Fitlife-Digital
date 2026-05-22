using Microsoft.EntityFrameworkCore;
using RapportServiceAPI.Data;
using RapportServiceAPI.Models;

namespace RapportServiceAPI.Repositories
{
    public class StatistikRepositoryDB : IRapportRepository
    {
        private readonly RapportDbContext _context;

        // Modtager RapportDbContext via dependency injection
        public StatistikRepositoryDB(RapportDbContext context)
        {
            _context = context;
        }

        // Henter en enkelt statistik fra CosmosDB baseret på id
        public async Task<Statistik?> GetByIdAsync(Guid id)
        {
            return await _context.Statistics.FindAsync(id);
        }

        // Henter alle statistikker fra CosmosDB
        public async Task<IEnumerable<Statistik>> GetAllAsync()
        {
            return await _context.Statistics.ToListAsync();
        }

        // Gemmer en ny statistik i CosmosDB
        public async Task AddAsync(Statistik statistik)
        {
            _context.Statistics.Add(statistik);
            await _context.SaveChangesAsync();
        }

        // Opdaterer en eksisterende statistik i CosmosDB
        public async Task UpdateAsync(Statistik statistik)
        {
            _context.Statistics.Update(statistik);
            await _context.SaveChangesAsync();
        }

        // Sletter en statistik fra CosmosDB baseret på id
        public async Task DeleteAsync(Guid id)
        {
            var statistik = await _context.Statistics.FindAsync(id);
            if (statistik != null)
            {
                _context.Statistics.Remove(statistik);
                await _context.SaveChangesAsync();
            }
        }
    }
}