using Microsoft.AspNetCore.Mvc;
using PTServiceAPI.Models; 
using PTServiceAPI.Repositories;

namespace PTServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PTController : ControllerBase
    {
        //Der bliver logget til fejlhåndtering og repository til databehandling her. 
        private readonly ILogger<PTController> _logger;
        private readonly ISessionRepository _sessionRepository;

        //Dependency injection af logger og repository vha. konstruktøren
        public PTController(ILogger<PTController> logger, ISessionRepository sessionRepository)
        {
            _logger = logger;
            _sessionRepository = sessionRepository;
        }

        //Her bliver alle sessioner fra repository hentet
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sessions = await _sessionRepository.GetAllAsync();
            return Ok(sessions);
        }

        //Henter en enkelt session baseret på id - returnerer 404 hvis den ikke bliver fundet
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null)
                return NotFound();
            return Ok(session);
        }

        //Opretter en ny booking af personlig træning
        //Returnerer en 201 Created med link til den nye ressource
        [HttpPost]
        public async Task<IActionResult> Book(Session session)
        {
            await _sessionRepository.AddAsync(session);
            return CreatedAtAction(nameof(GetById), new { id = session.Id }, session);
        }

        //Accepterer en personlig træningsession og markerer den som gennemført
        [HttpPut("{id}/accept")]
        public async Task<IActionResult> Accept(Guid id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null)
                return NotFound();
            session.CompleteProgram();
            await _sessionRepository.UpdateAsync(session);
            return Ok(session);
        }

        //Afmelder en personlig træningsession og sletter den fra repository
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _sessionRepository.DeleteAsync(id);
            return NoContent();
        }
        
        //Afviser en personlig træningsession og markerer den som annulleret
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null)
                return NotFound();
            session.CancelProgram();
            await _sessionRepository.UpdateAsync(session);
            return Ok(session);
        }

    }
}
