using Microsoft.AspNetCore.Mvc;
using ClassServiceAPI.Models;
using ClassServiceAPI.Repositories.Interfaces;

namespace ClassServiceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClassController : ControllerBase
{
    private readonly IClassRepository _repo;
    private readonly ILogger<ClassController> _logger;

    public ClassController(IClassRepository repo, ILogger<ClassController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    // POST

    [HttpPost]
    public async Task<IActionResult> CreateClassAsync([FromBody] Class classModel)
    {
        try
        {
            var created = await _repo.CreateClassAsync(classModel);
            _logger.LogInformation("Klasse oprettet: {className}", classModel.Title);
            return Ok(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved oprettelse af klasse: {className}", classModel.Title);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{classId}/members")]
    public async Task<IActionResult> RegisterMemberToClassAsync(Guid classId, [FromBody] Member member)
    {
        try
        {
            var updated = await _repo.RegisterMemberToClassAsync(classId, member);
            _logger.LogInformation("Medlem {memberId} tilmeldt klasse {classId}", member.Id, classId);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Kunne ikke tilmelde medlem {memberId} til klasse {classId}: {message}", member.Id, classId, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{classId}/waitinglist")]
    public async Task<IActionResult> RegisterMemberToWaitingListAsync(Guid classId, [FromBody] Member member)
    {
        try
        {
            var updated = await _repo.RegisterMemberToWaitingListAsync(classId, member);
            _logger.LogInformation("Medlem {memberId} tilmeldt venteliste for klasse {classId}", member.Id, classId);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Kunne ikke tilmelde til venteliste for klasse {classId}: {message}", classId, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // GET

    [HttpGet]
    public async Task<IActionResult> GetAllClassesAsync()
    {
        try
        {
            return Ok(await _repo.GetAllClassesAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved hentning af alle klasser");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("gym/{exerciseGymId}")]
    public async Task<IActionResult> GetAllClassesByExerciseGymAsync(Guid exerciseGymId)
    {
        try
        {
            return Ok(await _repo.GetAllClassesByExerciseGymAsync(exerciseGymId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved hentning af klasser for gym {exerciseGymId}", exerciseGymId);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassByIdAsync(Guid id)
    {
        try
        {
            var fitnessClass = await _repo.GetClassByIdAsync(id);
            if (fitnessClass is null)
            {
                _logger.LogWarning("Klasse med id {id} blev ikke fundet", id);
                return NotFound($"Klassen med id '{id}' blev ikke fundet");
            }
            return Ok(fitnessClass);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved hentning af klasse med id: {id}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}/waitinglist")]
    public async Task<IActionResult> GetWaitingListByClassAsync(Guid id)
    {
        try
        {
            return Ok(await _repo.GetWaitingListByClassAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Venteliste ikke fundet for klasse {id}", id);
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/members")]
    public async Task<IActionResult> GetRegisteredByClassAsync(Guid id)
    {
        try
        {
            return Ok(await _repo.GetRegisteredByClassAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Ingen tilmeldte fundet for klasse {id}", id);
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/attendees-count")]
    public async Task<IActionResult> GetNumberOfAttendeesByClassAsync(Guid id)
    {
        try
        {
            return Ok(await _repo.GetNumberOfAttendeesByClassAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Kunne ikke hente antal deltagere for klasse {id}", id);
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/absence")]
    public async Task<IActionResult> CalculateAbsenceByClassAsync(Guid id)
    {
        try
        {
            return Ok(await _repo.CalculateAbsenceByClassAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Kunne ikke beregne fravær for klasse {id}", id);
            return NotFound(ex.Message);
        }
    }

    // PUT

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelClassByIdAsync(Guid id)
    {
        try
        {
            var cancelled = await _repo.CancelClassByIdAsync(id);
            _logger.LogInformation("Klasse {id} aflyst", id);
            return Ok(cancelled);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Klasse med id {id} kunne ikke aflyses", id);
            return NotFound(ex.Message);
        }
    }

    // DELETE

    [HttpDelete("{classId}/members/{memberId}")]
    public async Task<IActionResult> UnRegisterMemberFromClassAsync(Guid classId, Guid memberId)
    {
        try
        {
            var updated = await _repo.UnRegisterMemberFromClassAsync(classId, memberId);
            _logger.LogInformation("Medlem {memberId} afmeldt klasse {classId}", memberId, classId);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Kunne ikke afmelde medlem {memberId} fra klasse {classId}", memberId, classId);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{classId}/waitinglist/{memberId}")]
    public async Task<IActionResult> UnRegisterMemberFromWaitingListAsync(Guid classId, Guid memberId)
    {
        try
        {
            var updated = await _repo.UnRegisterMemberFromWaitingListAsync(classId, memberId);
            _logger.LogInformation("Medlem {memberId} afmeldt venteliste for klasse {classId}", memberId, classId);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Kunne ikke afmelde medlem {memberId} fra venteliste for klasse {classId}", memberId, classId);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClassByIdAsync(Guid id)
    {
        try
        {
            var deleted = await _repo.DeleteClassByIdAsync(id);
            _logger.LogInformation("Klasse {id} slettet", id);
            return Ok(deleted);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Klasse med id {id} blev ikke fundet ved sletning", id);
            return NotFound(ex.Message);
        }
    }
}