using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;
using RapportServiceAPI.Models;
using RapportServiceAPI.Repositories;

namespace RapportServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RapportController : ControllerBase
    {
        //JBS: Der bliver logget til fejlhåndtering og repository til datahåndtering
        private readonly ILogger<RapportController> _logger;
        private readonly IRapportRepository _rapportRepository;

        //JBS: Dependency injection af logger og repository via konstruktøren.
        public RapportController(ILogger<RapportController> logger, IRapportRepository rapportRepository)
        {
            _logger = logger;
            _rapportRepository = rapportRepository;
        }

        //JBS: Her henter vi alle statistikker fra repository
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statistikker = await _rapportRepository.GetAllAsync();
            return Ok(statistikker);
        }

        //JBS: Vi henter en enkelt statistik baseret på id
        //JBS: Returner fejl hvis der ikke findes det givne id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var statistik = await _rapportRepository.GetByIdAsync(id);
            if (statistik == null)
                return NotFound();
            return Ok(statistik);
        }

        //JBS: Vi opretter en ny statistik 
        [HttpPost]
        public async Task<IActionResult> Create(Statistik statistik)
        {
            await _rapportRepository.AddAsync(statistik);
            return CreatedAtAction(nameof(GetById), new { id = statistik.Id }, statistik);
        }

        //JBS: Vi sletter en statistik baseret på baggrund af et id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _rapportRepository.DeleteAsync(id);
            return NoContent();
        }

        //JBS: Vi gemmer et nyt datapunkt i en statistik. 
        [HttpPost("{id}/lagring")]
        public async Task<IActionResult> StoreLagring(Guid id, Lagring lagring)
        {
            var statistik = await _rapportRepository.GetByIdAsync(id);
            if (statistik == null)
                return NotFound();
            statistik.Lagrings.Add(lagring);
            await _rapportRepository.UpdateAsync(statistik);
            return Ok(statistik);
        }

        //JBS: Vi genererer et share link til en statistik
        //JBS: Hvis det er vi gerne vil sende det rundt 
        [HttpPost("{id}/deling")]
        public async Task<IActionResult> CreateDeling(Guid id, Deling deling)
        {
            var statistik = await _rapportRepository.GetByIdAsync(id);
            if (statistik == null)
                return NotFound();
            deling.GenerateShareLink();
            statistik.Delings.Add(deling);
            await _rapportRepository.UpdateAsync(statistik);
            return Ok(statistik);
        }

        //JBS: Det her er til at tilbagekalde en delt statistik.
        [HttpDelete("{id}/deling/{userId}")]
        public async Task<IActionResult> RevokeDeling(Guid id, Guid userId)
        {
            var statistik = await _rapportRepository.GetByIdAsync(id);
            if (statistik == null)
                return NotFound();
            var deling = statistik.Delings.FirstOrDefault(d => d.SharedWithUserId == userId);
            if (deling == null)
                return NotFound();
            await _rapportRepository.UpdateAsync(statistik);
            return Ok(statistik);
        }

        //JBS: Hvis vi har lyst til at køre en analyse på en statistik.
        [HttpPost("{id}/analyse")]
        public async Task<IActionResult> RunAnalyse(Guid id, Analyse analyse)
        {
            var statistik = await _rapportRepository.GetByIdAsync(id);
            if (statistik == null)
                return NotFound();
            statistik.Analyses.Add(analyse);
            await _rapportRepository.UpdateAsync(statistik);
            return Ok(statistik);
        }

        //JBS: Her kan vi se en liste over tilmeldte medlemmer til en session
        [HttpGet("{id}/tilmeldte")]
        public async Task<IActionResult> GetTilmeldte(Guid id)
        {
            var statistik = await _rapportRepository.GetByIdAsync(id);
            if (statistik == null)
                return NotFound();
            return Ok(statistik.Registrered);
        }

        //JBS: Her henter vi en liste over medlemmer der mødte på til en session
        [HttpGet("{id}/fremmøde")]
        public async Task<IActionResult> GetFremmoede(Guid id)
        {
            var statistik = await _rapportRepository.GetByIdAsync(id);
            if (statistik == null)
                return NotFound();
            return Ok(statistik.Attended);
        }
        
        //JBS: Her kan vi hente en venteliste for en session
        [HttpGet("{id}/venteliste")]
        public async Task<IActionResult> GetVenteliste(Guid id)
        {
            var statistik = await _rapportRepository.GetByIdAsync(id);
            if (statistik == null)
                return NotFound();
            return Ok(statistik.WaitingList);
        }
    }
}