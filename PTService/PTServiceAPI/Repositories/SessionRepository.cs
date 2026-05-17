using PTServiceAPI.Models;

namespace PTServiceAPI.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly List<Session> _sessions = new();

        //Finder og returnerer en session baseret på id
        //Returnerer null hvis der ikke findes noget på id'et
        public Task<Session?> GetByIdAsync(Guid id)
        {
            var session = _sessions.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(session);
        }

        //Returnerer alle sessioner i listen
        public Task<IEnumerable<Session>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Session>>(_sessions);
        }

        //Tilføjer en ny session til listen
        public Task AddAsync(Session session)
        {
            _sessions.Add(session);
            return Task.CompletedTask;
        }

        //Opdaterer en eksisterende session ved at fjerne den gamle og tilføje den opdaterede
        public Task UpdateAsync(Session session)
        {
            var existing = _sessions.FirstOrDefault(s => s.Id == session.Id);
            if (existing != null)
            {
                _sessions.Remove(existing);
                _sessions.Add(session);
            }
            return Task.CompletedTask;
        }

        //Sletter en session fra listen på id
        public Task DeleteAsync(Guid id)
        {
            var session = _sessions.FirstOrDefault(s => s.Id == id);
            if (session != null)
                _sessions.Remove(session);
            return Task.CompletedTask;
        }
    }
}