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

    [HttpPost]
    public async Task<IActionResult> CreateClassAsync([FromBody] Class classModel)
    {
        _logger.LogDebug("Creating new class: {className}", classModel.Title);
        try
        {
            var created = await _repo.CreateClassAsync(classModel);
            _logger.LogInformation("Class created: {className}", classModel.Title);
            return Ok(created);
            return CreatedAtAction(nameof(GetClassByIdAsync), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating class: {className}", classModel.Title);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{classId}/members")]
    public async Task<IActionResult> RegisterMemberToClassAsync(Guid classId, [FromBody] Member member)
    {
        _logger.LogDebug("Registering member {memberId} to class {classId}", member.Id, classId);
        try
        {
            var updated = await _repo.RegisterMemberToClassAsync(classId, member);
            _logger.LogInformation("Member {memberId} registered to class {classId}", member.Id, classId);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Could not register member {memberId} to class {classId}: {message}", member.Id, classId, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{classId}/waitinglist")]
    public async Task<IActionResult> RegisterMemberToWaitingListAsync(Guid classId, [FromBody] Member member)
    {
        _logger.LogDebug("Registering member {memberId} to waiting list for class {classId}", member.Id, classId);
        try
        {
            var updated = await _repo.RegisterMemberToWaitingListAsync(classId, member);
            _logger.LogInformation("Member {memberId} registered to waiting list for class {classId}", member.Id, classId);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Could not register to waiting list for class {classId}: {message}", classId, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // GET

    [HttpGet]
    public async Task<IActionResult> GetAllClassesAsync()
    {
        _logger.LogDebug("Fetching all classes");
        try
        {
            return Ok(await _repo.GetAllClassesAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all classes");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("gym/{exerciseGymId}")]
    public async Task<IActionResult> GetAllClassesByExerciseGymAsync(Guid exerciseGymId)
    {
        _logger.LogDebug("Fetching classes for gym {exerciseGymId}", exerciseGymId);
        try
        {
            return Ok(await _repo.GetAllClassesByExerciseGymAsync(exerciseGymId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching classes for gym {exerciseGymId}", exerciseGymId);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassByIdAsync(Guid id)
    {
        _logger.LogDebug("Fetching class with id: {id}", id);
        try
        {
            var fitnessClass = await _repo.GetClassByIdAsync(id);
            if (fitnessClass is null)
            {
                _logger.LogWarning("Class with id {id} was not found", id);
                return NotFound($"Class with id '{id}' was not found");
            }
            return Ok(fitnessClass);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching class with id: {id}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}/waitinglist")]
    public async Task<IActionResult> GetWaitingListByClassAsync(Guid id)
    {
        _logger.LogDebug("Fetching waiting list for class {id}", id);
        try
        {
            return Ok(await _repo.GetWaitingListByClassAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Waiting list not found for class {id}", id);
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/members")]
    public async Task<IActionResult> GetRegisteredByClassAsync(Guid id)
    {
        _logger.LogDebug("Fetching registered members for class {id}", id);
        try
        {
            return Ok(await _repo.GetRegisteredByClassAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("No registered members found for class {id}", id);
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/attendees-count")]
    public async Task<IActionResult> GetNumberOfAttendeesByClassAsync(Guid id)
    {
        _logger.LogDebug("Fetching attendee count for class {id}", id);
        try
        {
            return Ok(await _repo.GetNumberOfAttendeesByClassAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Could not fetch attendee count for class {id}", id);
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/absence")]
    public async Task<IActionResult> CalculateAbsenceByClassAsync(Guid id)
    {
        _logger.LogDebug("Calculating absence for class {id}", id);
        try
        {
            return Ok(await _repo.CalculateAbsenceByClassAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Could not calculate absence for class {id}", id);
            return NotFound(ex.Message);
        }
    }

    // PUT

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelClassByIdAsync(Guid id)
    {
        _logger.LogDebug("Cancelling class with id: {id}", id);
        try
        {
            return Ok(await _repo.CancelClassByIdAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Class with id {id} could not be cancelled", id);
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{classId}/members/{memberId}")]
    public async Task<IActionResult> UnRegisterMemberFromClassAsync(Guid classId, Guid memberId)
    {
        _logger.LogDebug("Unregistering member {memberId} from class {classId}", memberId, classId);
        try
        {
            return Ok(await _repo.UnRegisterMemberFromClassAsync(classId, memberId));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Could not unregister member {memberId} from class {classId}", memberId, classId);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{classId}/waitinglist/{memberId}")]
    public async Task<IActionResult> UnRegisterMemberFromWaitingListAsync(Guid classId, Guid memberId)
    {
        _logger.LogDebug("Unregistering member {memberId} from waiting list for class {classId}", memberId, classId);
        try
        {
            return Ok(await _repo.UnRegisterMemberFromWaitingListAsync(classId, memberId));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Could not unregister member {memberId} from waiting list for class {classId}", memberId, classId);
            return BadRequest(ex.Message);
        }
    }

    // DELETE

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClassByIdAsync(Guid id)
    {
        _logger.LogDebug("Deleting class with id: {id}", id);
        try
        {
            return Ok(await _repo.DeleteClassByIdAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Class with id {id} was not found for deletion", id);
            return NotFound(ex.Message);
        }
    }
}