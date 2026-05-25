using StatisticServiceAPI.Models;

namespace StatisticServiceAPI.Repositories
{
    public interface IStatisticRepository
    {
        Task<Statistik?> GetByIdAsync(Guid id);
        Task<IEnumerable<Statistik>> GetAllAsync();
        Task AddAsync(Statistik statistik);
        Task UpdateAsync(Statistik statistik);
        Task DeleteAsync(Guid id);
    }
}