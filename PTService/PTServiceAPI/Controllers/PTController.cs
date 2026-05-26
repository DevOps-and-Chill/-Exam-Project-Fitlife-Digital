using Microsoft.AspNetCore.Mvc;
using PTServiceAPI.Models; 
using PTServiceAPI.Repositories;

namespace PTServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PTController : ControllerBase
    {
        //JBS: Logger is injected for error handling, repository for data processing
        private readonly ILogger<PTController> _logger;
        private readonly ISessionRepository _sessionRepository;

        //JBS: Dependency injection of logger and repository via constructor
        public PTController(ILogger<PTController> logger, ISessionRepository sessionRepository)
        {
            _logger = logger;
            _sessionRepository = sessionRepository;

            //JBS: Log which server and IP is responding
            var hostName = System.Net.Dns.GetHostName();
            var ips = System.Net.Dns.GetHostAddresses(hostName);
            var ipaddr = ips.First().MapToIPv4().ToString();
            _logger.LogInformation("PTService responding from {IpAddress}", ipaddr);
        }

        //JBS: Fetches all sessions from the repository
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogDebug("Fetching all sessions");
            try
            {
                var sessions = await _sessionRepository.GetAllAsync();
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all sessions: {message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Fetches a single session by id - returns 404 if not found
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogDebug("Fetching session with id: {id}", id);
            try
            {
                var session = await _sessionRepository.GetByIdAsync(id);
                if (session == null)
                {
                    _logger.LogWarning("Session with id: {id} was not found", id);
                    return NotFound();
                }
                return Ok(session);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching session with id {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Creates a new personal training booking
        //JBS: Returns 201 Created with a link to the new resource
        [HttpPost]
        public async Task<IActionResult> Book(Session session)
        {
            _logger.LogDebug("Booking new session for member: {memberId}", session.MemberId);
            try
            {
                await _sessionRepository.AddAsync(session);
                _logger.LogInformation("Session booked for member: {memberId}", session.MemberId);
                return CreatedAtAction(nameof(GetById), new { id = session.Id }, session);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error booking session for member {memberId}: {message}", session.MemberId, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Accepts a personal training session and marks it as completed
        [HttpPut("{id}/accept")]
        public async Task<IActionResult> Accept(Guid id)
        {
            _logger.LogDebug("Accepting session with id: {id}", id);
            try
            {
                var session = await _sessionRepository.GetByIdAsync(id);
                if (session == null)
                {
                    _logger.LogWarning("Session with id: {id} was not found", id);
                    return NotFound();
                }
                session.CompleteProgram();
                await _sessionRepository.UpdateAsync(session);
                _logger.LogInformation("Session with id: {id} accepted", id);
                return Ok(session);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error accepting session with id {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Cancels a personal training session and removes it from the repository
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            _logger.LogDebug("Cancelling session with id: {id}", id);
            try
            {
                await _sessionRepository.DeleteAsync(id);
                _logger.LogInformation("Session with id: {id} cancelled", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error cancelling session with id {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //JBS: Rejects a personal training session and marks it as cancelled
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id)
        {
            _logger.LogDebug("Rejecting session with id: {id}", id);
            try
            {
                var session = await _sessionRepository.GetByIdAsync(id);
                if (session == null)
                {
                    _logger.LogWarning("Session with id: {id} was not found", id);
                    return NotFound();
                }
                session.CancelProgram();
                await _sessionRepository.UpdateAsync(session);
                _logger.LogInformation("Session with id: {id} rejected", id);
                return Ok(session);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error rejecting session with id {id}: {message}", id, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}