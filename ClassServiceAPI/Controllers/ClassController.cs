using ClassServiceAPI.Models;
using ClassServiceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassServiceAPI.Controllers;

[Route("")]
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
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateClassAsync([FromBody] Class classModel)
    {
        _logger.LogDebug("Creating new class: {className}", classModel.Title);
        try
        {
            var created = await _repo.CreateClassAsync(classModel);
            _logger.LogInformation("Class created: {className}", classModel.Title);
            return Ok(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating class: {className}", classModel.Title);
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost("{classId}/members")]
    public async Task<IActionResult> RegisterMemberToClassAsync(string classId, [FromBody] Member member)
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

    [Authorize]
    [HttpPost("{classId}/waitinglist")]
    public async Task<IActionResult> RegisterMemberToWaitingListAsync(string classId, [FromBody] Member member)
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
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllClassesAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Authenticated user {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
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
    
    [Authorize]
    [HttpGet("exercisegyms/{exerciseGymId}")]
    public async Task<IActionResult> GetClassesByExerciseGymAsync(string exerciseGymId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Authenticated user {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        _logger.LogDebug("Fetching classes for gym {exerciseGymId}", exerciseGymId);
        try
        {
            return Ok(await _repo.GetClassesByExerciseGymAsync(exerciseGymId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching classes for gym {exerciseGymId}", exerciseGymId);
            return BadRequest(ex.Message);
        }
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassByIdAsync(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Authenticated user {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
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
    
    [HttpGet("members/{id}")]
    public async Task<IActionResult> GetClassesByMemberAsync(string id)
    {
        _logger.LogDebug("Fetching member with id: {id}'s classes", id);
        try
        {
            var fitnessClass = await _repo.GetClassesByMemberAsync(id);
            if (fitnessClass is null)
            {
                _logger.LogWarning("Classes not found. GetClassesByMemberAsync is null");
                return NotFound();
            }
            return Ok(fitnessClass);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching class with id: {id}", id);
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("employees/{id}")]
    public async Task<IActionResult> GetClassesByEmployeeAsync(string id)
    {
        _logger.LogDebug("Fetching member with id: {id}'s classes", id);
        try
        {
            var fitnessClass = await _repo.GetClassesByEmployeeAsync(id);
            if (fitnessClass == null)
            {
                _logger.LogWarning("Classes not found. GetGlaccesByEmployeeAsync is null", id);
                return NotFound();
            }
            return Ok(fitnessClass);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Classes not found");
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}/waitinglist")]
    public async Task<IActionResult> GetWaitingListByClassAsync(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Authenticated user {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
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

    [Authorize]
    [HttpGet("{id}/members")]
    public async Task<IActionResult> GetMembersByClassAsync(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Authenticated user {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        _logger.LogDebug("Fetching registered members for class {id}", id);
        try
        {
            return Ok(await _repo.GetMembersByClassAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("No registered members found for class {id}", id);
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}/attendees-count")]
    public async Task<IActionResult> GetNumberOfAttendeesByClassAsync(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Authenticated user {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
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

    [Authorize]
    [HttpGet("{id}/absence")]
    public async Task<IActionResult> CalculateAbsenceByClassAsync(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Authenticated user {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
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
    
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClassAsync(string id, [FromBody] Class classModel)
    {
        try
        {
            var updated = await _repo.UpdateClassAsync(id, classModel);
            if (updated is null)
                return NotFound($"Class with id '{id}' was not found");
            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating class: {id}", id);
            return BadRequest(ex.Message);
        }
    }
    
    
    // PATCH
    [Authorize]
    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> CancelClassByIdAsync(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Authenticated user {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        _logger.LogDebug("Cancelling class with id: {id}", id);
        try
        {
            var cancelled = await _repo.CancelClassAsync(id);
            _logger.LogInformation("Klasse {id} aflyst", id);
            return Ok(cancelled);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Class with id {id} could not be cancelled", id);
            return NotFound(ex.Message);
        }
    }

    // DELETE
    [Authorize]
    [HttpDelete("{classId}/members/{memberId}")]
    public async Task<IActionResult> UnRegisterMemberFromClassAsync(string classId, string memberId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Authenticated user {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        _logger.LogDebug("Unregistering member {memberId} from class {classId}", memberId, classId);
        try
        {
            var updated = await _repo.UnRegisterMemberFromClassAsync(classId, memberId);
            _logger.LogInformation("Medlem {memberId} afmeldt klasse {classId}", memberId, classId);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Could not unregister member {memberId} from class {classId}", memberId, classId);
            return BadRequest(ex.Message);
        }
    }
    [Authorize]
    [HttpDelete("{classId}/waitinglist/{memberId}")]
    public async Task<IActionResult> UnRegisterMemberFromWaitingListAsync(string classId, string memberId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Authenticated user {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        _logger.LogDebug("Unregistering member {memberId} from waiting list for class {classId}", memberId, classId);
        try
        {
            var updated = await _repo.UnRegisterMemberFromWaitingListAsync(classId, memberId);
            _logger.LogInformation("Medlem {memberId} afmeldt venteliste for klasse {classId}", memberId, classId);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Could not unregister member {memberId} from waiting list for class {classId}", memberId, classId);
            return BadRequest(ex.Message);
        }
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClassAsync(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Authenticated user {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        _logger.LogDebug("Deleting class with id: {id}", id);
        try
        {
            var deleted = await _repo.DeleteClassByIdAsync(id);
            _logger.LogInformation("Class {id} deleted", id);
            return Ok(deleted);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Class with id {id} was not found for deletion", id);
            return NotFound(ex.Message);
        }
    }
}