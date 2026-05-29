using PTServiceAPI.Models;

namespace PTServiceAPI.Repositories
{
    public interface ISessionRepository
    {
        Task<Session?> GetByIdAsync(Guid id);
        Task<IEnumerable<Session>> GetAllAsync();
        Task AddAsync(Session session);
        Task UpdateAsync(Session session);
        Task DeleteAsync(Guid id);
    }
}