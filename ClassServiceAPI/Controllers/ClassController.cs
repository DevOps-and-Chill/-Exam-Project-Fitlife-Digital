using ClassServiceAPI.Messaging;
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

    [HttpPost("create-class")]
    public async Task<IActionResult> CreateClassAsync([FromBody] Class classModel)
    {
        _logger.LogInformation("Opretter ny klasse: {className}", classModel.Name);
        try
        {
            var created = await _repo.CreateClassAsync(classModel);
            _logger.LogInformation("Klasse oprettet: {className}", classModel.Name);
            return Ok(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved oprettelse af klasse: {className}", classModel.Name);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{classId}/register-member")]
    public async Task<IActionResult> RegisterMemberToClassAsync(Guid classId, [FromBody] Member member)
    {
        _logger.LogInformation("Tilmelder medlem {memberId} til klasse {classId}", member.Id, classId);
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

    [HttpPost("{classId}/register-member-to-waitinglist")]
    public async Task<IActionResult> RegisterMemberToWaitingListAsync(Guid classId, [FromBody] Member member)
    {
        _logger.LogInformation("Tilmelder medlem {memberId} til venteliste for klasse {classId}", member.Id, classId);
        try
        {
            var updated = await _repo.RegisterMemberToWaitingListAsync(classId, member);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Kunne ikke tilmelde til venteliste for klasse {classId}: {message}", classId, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // GET

    [HttpGet("get-all-classes")]
    public async Task<IActionResult> GetAllClassesAsync()
    {
        _logger.LogInformation("Henter alle klasser");
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

    [HttpGet("get-classes-by-gym/{exerciseGymId}")]
    public async Task<IActionResult> GetAllClassesByExerciseGymAsync(Guid exerciseGymId)
    {
        _logger.LogInformation("Henter klasser for gym {exerciseGymId}", exerciseGymId);
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

    [HttpGet("get-class-by-id/{id}")]
    public async Task<IActionResult> GetClassByIdAsync(Guid id)
    {
        _logger.LogInformation("Henter klasse med id: {id}", id);
        try
        {
            var fitnessClass = await _repo.GetClassByIdAsync(id);
            if (fitnessClass is null)
            {
                _logger.LogWarning("Klasse med id {id} blev ikke fundet", id);
                return NotFound($"Class with id '{id}' was not found");
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
        _logger.LogInformation("Henter venteliste for klasse {id}", id);
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

    [HttpGet("{id}/registered-members")]
    public async Task<IActionResult> GetRegisteredByClassAsync(Guid id)
    {
        _logger.LogInformation("Henter tilmeldte medlemmer for klasse {id}", id);
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
        _logger.LogInformation("Henter antal deltagere for klasse {id}", id);
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
        _logger.LogInformation("Beregner fravær for klasse {id}", id);
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
        _logger.LogInformation("Aflysser klasse med id: {id}", id);
        try
        {
            return Ok(await _repo.CancelClassByIdAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Klasse med id {id} kunne ikke aflyses", id);
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{classId}/unregister-member/{memberId}")]
    public async Task<IActionResult> UnRegisterMemberFromClassAsync(Guid classId, Guid memberId)
    {
        _logger.LogInformation("Afmelder medlem {memberId} fra klasse {classId}", memberId, classId);
        try
        {
            return Ok(await _repo.UnRegisterMemberFromClassAsync(classId, memberId));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Kunne ikke afmelde medlem {memberId} fra klasse {classId}", memberId, classId);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{classId}/unregister-member-from-waitinglist/{memberId}")]
    public async Task<IActionResult> UnRegisterMemberFromWaitingListAsync(Guid classId, Guid memberId)
    {
        _logger.LogInformation("Afmelder medlem {memberId} fra venteliste for klasse {classId}", memberId, classId);
        try
        {
            return Ok(await _repo.UnRegisterMemberFromWaitingListAsync(classId, memberId));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Kunne ikke afmelde medlem {memberId} fra venteliste for klasse {classId}", memberId, classId);
            return BadRequest(ex.Message);
        }
    }

    // DELETE

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> DeleteClassByIdAsync(Guid id)
    {
        _logger.LogInformation("Sletter klasse med id: {id}", id);
        try
        {
            return Ok(await _repo.DeleteClassByIdAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Klasse med id {id} blev ikke fundet ved sletning", id);
            return NotFound(ex.Message);
        }
    }
}