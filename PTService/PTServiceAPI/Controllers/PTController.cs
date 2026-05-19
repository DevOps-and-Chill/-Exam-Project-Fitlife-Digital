using Microsoft.AspNetCore.Mvc;
using PTServiceAPI.Models; 
using PTServiceAPI.Repositories;

namespace PTServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PTController : ControllerBase
    {
        //JBS: Der bliver logget til fejlhåndtering og repository til databehandling her. 
        private readonly ILogger<PTController> _logger;
        private readonly ISessionRepository _sessionRepository;

        //JBS: Dependency injection af logger og repository vha. konstruktøren
        public PTController(ILogger<PTController> logger, ISessionRepository sessionRepository)
        {
            _logger = logger;
            _sessionRepository = sessionRepository;

            //Vi logger hvilke server og IP der svarer
            var hostName = System.Net.Dns.GetHostName();
            var ips = System.Net.Dns.GetHostAddresses(hostName);
            var ipaddr = ips.First().MapToIPv4().ToString();
            _logger.LogInformation(1, $"PTService responding from {ipaddr}");
        }

        //JBS: Her bliver alle sessioner fra repository hentet
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Henter alle sessioner");
            var sessions = await _sessionRepository.GetAllAsync();
            return Ok(sessions);
        }

        //JBS: Henter en enkelt session baseret på id - returnerer 404 hvis den ikke bliver fundet
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Henter session med id: {id}", id);
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null)
            {
                _logger.LogWarning("Session med id: {id} blev ikke fundet!", id);
                return NotFound();
            }
            return Ok(session);
        }

        //JBS: Opretter en ny booking af personlig træning
        //JBS: Returnerer en 201 Created med link til den nye ressource
        [HttpPost]
        public async Task<IActionResult> Book(Session session)
        {
            _logger.LogInformation("Booker ny session for medlem: {memberId}", session.MemberId);
            await _sessionRepository.AddAsync(session);
            return CreatedAtAction(nameof(GetById), new { id = session.Id }, session);
        }

        //JBS: Accepterer en personlig træningsession og markerer den som gennemført
        [HttpPut("{id}/accept")]
        public async Task<IActionResult> Accept(Guid id)
        {
            _logger.LogInformation("Accepterer session med id: {id}", id);
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null)
            {
                _logger.LogWarning("Session med id: {id} blev ikke fundet", id);
                return NotFound();
            }
            session.CompleteProgram();
            await _sessionRepository.UpdateAsync(session);
            return Ok(session);
        }

        //JBS: Afmelder en personlig træningsession og sletter den fra repository
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            _logger.LogInformation("Afmelder session med id: {id}", id);
            await _sessionRepository.DeleteAsync(id);
            return NoContent();
        }
        
        //JBS: Afviser en personlig træningsession og markerer den som annulleret
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id)
        {
            _logger.LogInformation("Afviser session med id: {id}", id);
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null)
            {
                _logger.LogWarning("Session med id: {id} blev ikke fundet", id);
                return NotFound();
            }
            session.CancelProgram();
            await _sessionRepository.UpdateAsync(session);
            return Ok(session);
        }

    }
}
