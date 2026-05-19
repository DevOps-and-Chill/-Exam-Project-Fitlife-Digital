using Microsoft.EntityFrameworkCore;
using PTServiceAPI.Data;
using PTServiceAPI.Models;

namespace PTServiceAPI.Repositories
{
    public class SessionRepositoryDB : ISessionRepository
    {
        private readonly PTDbContext _context;

        // Modtager PTDbContext via dependency injection
        public SessionRepositoryDB(PTDbContext context)
        {
            _context = context;
        }

        // Henter en enkelt session fra CosmosDB baseret på id
        public async Task<Session?> GetByIdAsync(Guid id)
        {
            return await _context.Sessions.FindAsync(id);
        }

        // Henter alle sessioner fra CosmosDB
        public async Task<IEnumerable<Session>> GetAllAsync()
        {
            return await _context.Sessions.ToListAsync();
        }

        // Gemmer en ny session i CosmosDB
        public async Task AddAsync(Session session)
        {
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
        }

        // Opdaterer en eksisterende session i CosmosDB
        public async Task UpdateAsync(Session session)
        {
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
        }

        // Sletter en session fra CosmosDB baseret på id
        public async Task DeleteAsync(Guid id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }
    }
}