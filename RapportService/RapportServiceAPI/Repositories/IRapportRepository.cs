using RapportServiceAPI.Models;

namespace RapportServiceAPI.Repositories
{
    public interface IRapportRepository
    {
        Task<Statistik?> GetByIdAsync(Guid id);
        Task<IEnumerable<Statistik>> GetAllAsync();
        Task AddAsync(Statistik statistik);
        Task UpdateAsync(Statistik statistik);
        Task DeleteAsync(Guid id);
    }
}