using System.Runtime.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatisticServiceAPI.Models;
using StatisticServiceAPI.Repositories;

namespace StatisticServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticController : ControllerBase
    {
        //JBS: Logger is injected for error handling, repository for data processing
        private readonly ILogger<StatisticController> _logger;
        private readonly IStatisticRepository _StatisticRepository;

        //JBS: Dependency injection of logger and repository via constructor
        public StatisticController(ILogger<StatisticController> logger, IStatisticRepository StatisticRepository)
        {
            _logger = logger;
            _StatisticRepository = StatisticRepository;
        }

        //JBS: Fetches all statistics from the repository
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogDebug("Fetching all statistics");
            try
            {
                var statistikker = await _StatisticRepository.GetAllAsync();
                return Ok(statistikker);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all statistics: {message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Fetches a single statistic by id - returns 404 if not found
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogDebug("Fetching statistic with id: {id}", id);
            try
            {
                var statistik = await _StatisticRepository.GetByIdAsync(id);
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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Statistik statistik)
        {
            _logger.LogDebug("Creating new statistic");
            try
            {
                await _StatisticRepository.AddAsync(statistik);
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
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogDebug("Deleting statistic with id: {id}", id);
            try
            {
                await _StatisticRepository.DeleteAsync(id);
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
        [Authorize]
        [HttpPost("{id}/storage")]
        public async Task<IActionResult> StoreLagring(Guid id, Lagring lagring)
        {
            _logger.LogDebug("Storing data point for statistic with id: {id}", id);
            try
            {
                var statistik = await _StatisticRepository.GetByIdAsync(id);
                if (statistik == null)
                {
                    _logger.LogWarning("Statistic with id: {id} was not found", id);
                    return NotFound();
                }
                statistik.Lagrings.Add(lagring);
                await _StatisticRepository.UpdateAsync(statistik);
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
        [Authorize]
        [HttpPost("{id}/share")]
        public async Task<IActionResult> CreateDeling(Guid id, Deling deling)
        {
            _logger.LogDebug("Creating share link for statistic with id: {id}", id);
            try
            {
                var statistik = await _StatisticRepository.GetByIdAsync(id);
                if (statistik == null)
                {
                    _logger.LogWarning("Statistic with id: {id} was not found", id);
                    return NotFound();
                }
                deling.GenerateShareLink();
                statistik.Delings.Add(deling);
                await _StatisticRepository.UpdateAsync(statistik);
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
        [Authorize]
        [HttpDelete("{id}/share/{userId}")]
        public async Task<IActionResult> RevokeDeling(Guid id, Guid userId)
        {
            _logger.LogDebug("Revoking share for statistic {id} and user {userId}", id, userId);
            try
            {
                var statistik = await _StatisticRepository.GetByIdAsync(id);
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
                statistik.Delings.Remove(deling);
                await _StatisticRepository.UpdateAsync(statistik);
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
        [Authorize]
        [HttpPost("{id}/analysis")]
        public async Task<IActionResult> RunAnalyse(Guid id, Analyse analyse)
        {
            _logger.LogDebug("Running analysis for statistic with id: {id}", id);
            try
            {
                var statistik = await _StatisticRepository.GetByIdAsync(id);
                if (statistik == null)
                {
                    _logger.LogWarning("Statistic with id: {id} was not found", id);
                    return NotFound();
                }
                statistik.Analyses.Add(analyse);
                await _StatisticRepository.UpdateAsync(statistik);
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
        [Authorize]
        [HttpGet("{id}/registered")]
        public async Task<IActionResult> GetTilmeldte(Guid id)
        {
            _logger.LogDebug("Fetching registered members for statistic with id: {id}", id);
            try
            {
                var statistik = await _StatisticRepository.GetByIdAsync(id);
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
        [Authorize]
        [HttpGet("{id}/attendance")]
        public async Task<IActionResult> GetFremmoede(Guid id)
        {
            _logger.LogDebug("Fetching attendance for statistic with id: {id}", id);
            try
            {
                var statistik = await _StatisticRepository.GetByIdAsync(id);
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
        [Authorize]
        [HttpGet("{id}/waitinglist")]
        public async Task<IActionResult> GetVenteliste(Guid id)
        {
            _logger.LogDebug("Fetching waiting list for statistic with id: {id}", id);
            try
            {
                var statistik = await _StatisticRepository.GetByIdAsync(id);
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