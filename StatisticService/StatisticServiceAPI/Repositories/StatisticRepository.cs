using StatisticServiceAPI.Models;

namespace StatisticServiceAPI.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        //JBS: En in-memory liste der fungerer som midlertidig database
        //JBS: Kan erstattes når vi får en database op og køre
        private readonly List<Statistik> _statistikker = new();

        //JBS: Finder og returnerer en statistik baseret på et id
        //JBS: Returner null hvis id'et ikke bliver fundet
        public Task<Statistik?> GetByIdAsync(Guid id)
        {
            var statistik = _statistikker.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(statistik);
        }

        //JBS: Returnerer alle statistikker i listen her
        public Task<IEnumerable<Statistik>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Statistik>>(_statistikker);
        }

        //JBS: Vi tilføjer en ny statistik til listen
        public Task AddAsync(Statistik statistik)
        {
            _statistikker.Add(statistik);
            return Task.CompletedTask;
        }

        //JBS: En eksisterende statistik bliver opdateret
        public Task UpdateAsync(Statistik statistik)
        {
            var existing = _statistikker.FirstOrDefault(s => s.Id == statistik.Id);
            if (existing != null)
            {
                _statistikker.Remove(existing);
                _statistikker.Add(statistik);
            }
            return Task.CompletedTask;
        }

        //JBS: Der slettes en statistik fra listen baseret på et id
        public Task DeleteAsync(Guid id)
        {
            var statistik = _statistikker.FirstOrDefault(s => s.Id == id);
            if (statistik != null)
                _statistikker.Remove(statistik);
            return Task.CompletedTask;
        }
    }
}