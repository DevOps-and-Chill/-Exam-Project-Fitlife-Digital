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
        //JBS: Logger is injected for error handling, repository for data processing
        private readonly ILogger<RapportController> _logger;
        private readonly IRapportRepository _rapportRepository;

        //JBS: Dependency injection of logger and repository via constructor
        public RapportController(ILogger<RapportController> logger, IRapportRepository rapportRepository)
        {
            _logger = logger;
            _rapportRepository = rapportRepository;
        }

        //JBS: Fetches all statistics from the repository
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogDebug("Fetching all statistics");
            try
            {
                var statistikker = await _rapportRepository.GetAllAsync();
                return Ok(statistikker);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all statistics: {message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Fetches a single statistic by id - returns 404 if not found
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogDebug("Fetching statistic with id: {id}", id);
            try
            {
                var statistik = await _rapportRepository.GetByIdAsync(id);
                if (statistik == null)
                {
                    _logger.LogWarning("Statistic with id: {id} was not found", id);
                    return NotFound();
                }
                return Ok(statistik);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching statistic with id {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Creates a new statistic
        [HttpPost]
        public async Task<IActionResult> Create(Statistik statistik)
        {
            _logger.LogDebug("Creating new statistic");
            try
            {
                await _rapportRepository.AddAsync(statistik);
                _logger.LogInformation("Statistic created with id: {id}", statistik.Id);
                return CreatedAtAction(nameof(GetById), new { id = statistik.Id }, statistik);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating statistic: {message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Deletes a statistic by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogDebug("Deleting statistic with id: {id}", id);
            try
            {
                await _rapportRepository.DeleteAsync(id);
                _logger.LogInformation("Statistic with id: {id} deleted", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting statistic with id {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Stores a new data point in a statistic
        [HttpPost("{id}/storage")]
        public async Task<IActionResult> StoreLagring(Guid id, Lagring lagring)
        {
            _logger.LogDebug("Storing data point for statistic with id: {id}", id);
            try
            {
                var statistik = await _rapportRepository.GetByIdAsync(id);
                if (statistik == null)
                {
                    _logger.LogWarning("Statistic with id: {id} was not found", id);
                    return NotFound();
                }
                statistik.Lagrings.Add(lagring);
                await _rapportRepository.UpdateAsync(statistik);
                _logger.LogInformation("Data point stored for statistic with id: {id}", id);
                return Ok(statistik);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error storing data point for statistic {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Generates a share link for a statistic
        [HttpPost("{id}/share")]
        public async Task<IActionResult> CreateDeling(Guid id, Deling deling)
        {
            _logger.LogDebug("Creating share link for statistic with id: {id}", id);
            try
            {
                var statistik = await _rapportRepository.GetByIdAsync(id);
                if (statistik == null)
                {
                    _logger.LogWarning("Statistic with id: {id} was not found", id);
                    return NotFound();
                }
                deling.GenerateShareLink();
                statistik.Delings.Add(deling);
                await _rapportRepository.UpdateAsync(statistik);
                _logger.LogInformation("Share link created for statistic with id: {id}", id);
                return Ok(statistik);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating share link for statistic {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Revokes a shared statistic
        [HttpDelete("{id}/share/{userId}")]
        public async Task<IActionResult> RevokeDeling(Guid id, Guid userId)
        {
            _logger.LogDebug("Revoking share for statistic {id} and user {userId}", id, userId);
            try
            {
                var statistik = await _rapportRepository.GetByIdAsync(id);
                if (statistik == null)
                {
                    _logger.LogWarning("Statistic with id: {id} was not found", id);
                    return NotFound();
                }
                var deling = statistik.Delings.FirstOrDefault(d => d.SharedWithUserId == userId);
                if (deling == null)
                {
                    _logger.LogWarning("Share not found for statistic {id} and user {userId}", id, userId);
                    return NotFound();
                }
                await _rapportRepository.UpdateAsync(statistik);
                _logger.LogInformation("Share revoked for statistic {id} and user {userId}", id, userId);
                return Ok(statistik);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error revoking share for statistic {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Runs an analysis on a statistic
        [HttpPost("{id}/analysis")]
        public async Task<IActionResult> RunAnalyse(Guid id, Analyse analyse)
        {
            _logger.LogDebug("Running analysis for statistic with id: {id}", id);
            try
            {
                var statistik = await _rapportRepository.GetByIdAsync(id);
                if (statistik == null)
                {
                    _logger.LogWarning("Statistic with id: {id} was not found", id);
                    return NotFound();
                }
                statistik.Analyses.Add(analyse);
                await _rapportRepository.UpdateAsync(statistik);
                _logger.LogInformation("Analysis added for statistic with id: {id}", id);
                return Ok(statistik);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error running analysis for statistic {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Fetches a list of registered members for a session
        [HttpGet("{id}/registered")]
        public async Task<IActionResult> GetTilmeldte(Guid id)
        {
            _logger.LogDebug("Fetching registered members for statistic with id: {id}", id);
            try
            {
                var statistik = await _rapportRepository.GetByIdAsync(id);
                if (statistik == null)
                {
                    _logger.LogWarning("Statistic with id: {id} was not found", id);
                    return NotFound();
                }
                return Ok(statistik.Registrered);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching registered members for statistic {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Fetches a list of members who attended a session
        [HttpGet("{id}/attendance")]
        public async Task<IActionResult> GetFremmoede(Guid id)
        {
            _logger.LogDebug("Fetching attendance for statistic with id: {id}", id);
            try
            {
                var statistik = await _rapportRepository.GetByIdAsync(id);
                if (statistik == null)
                {
                    _logger.LogWarning("Statistic with id: {id} was not found", id);
                    return NotFound();
                }
                return Ok(statistik.Attended);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching attendance for statistic {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Fetches the waiting list for a session
        [HttpGet("{id}/waitinglist")]
        public async Task<IActionResult> GetVenteliste(Guid id)
        {
            _logger.LogDebug("Fetching waiting list for statistic with id: {id}", id);
            try
            {
                var statistik = await _rapportRepository.GetByIdAsync(id);
                if (statistik == null)
                {
                    _logger.LogWarning("Statistic with id: {id} was not found", id);
                    return NotFound();
                }
                return Ok(statistik.WaitingList);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching waiting list for statistic {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}