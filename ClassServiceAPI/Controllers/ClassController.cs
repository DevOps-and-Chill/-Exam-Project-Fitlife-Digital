using Microsoft.AspNetCore.Mvc;
using ClassServiceAPI.Models;
using ClassServiceAPI.Repositories.Interfaces;

namespace ClassServiceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClassController : ControllerBase
{
    private readonly IClassRepository _repo;

    public ClassController(IClassRepository repo)
    {
        _repo = repo;
    }

    // POST

    [HttpPost("create-class")]
    public async Task<IActionResult> CreateClassAsync([FromBody] Class classModel)
    {
        try
        {
            var created = await _repo.CreateClassAsync(classModel);
            return Ok(created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{classId}/register-member")]
    public async Task<IActionResult> RegisterMemberToClassAsync(Guid classId, [FromBody] Member member)
    {
        try
        {
            var updated = await _repo.RegisterMemberToClassAsync(classId, member);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{classId}/register-member-to-waitinglist")]
    public async Task<IActionResult> RegisterMemberToWaitingListAsync(Guid classId, [FromBody] Member member)
    {
        try
        {
            var updated = await _repo.RegisterMemberToWaitingListAsync(classId, member);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET

    [HttpGet("get-all-classes")]
    public async Task<IActionResult> GetAllClassesAsync()
    {
        try
        {
            return Ok(await _repo.GetAllClassesAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-classes-by-gym/{exerciseGymId}")]
    public async Task<IActionResult> GetAllClassesByExerciseGymAsync(Guid exerciseGymId)
    {
        try
        {
            return Ok(await _repo.GetAllClassesByExerciseGymAsync(exerciseGymId));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-class-by-id/{id}")]
    public async Task<IActionResult> GetClassByIdAsync(Guid id)
    {
        try
        {
            var fitnessClass = await _repo.GetClassByIdAsync(id);

            if (fitnessClass is null)
                return NotFound($"Class with id '{id}' was not found");

            return Ok(fitnessClass);
        }
        catch (Exception ex)
        {
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
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/registered-members")]
    public async Task<IActionResult> GetRegisteredByClassAsync(Guid id)
    {
        try
        {
            return Ok(await _repo.GetRegisteredByClassAsync(id));
        }
        catch (InvalidOperationException ex)
        {
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
            return NotFound(ex.Message);
        }
    }

    // PUT

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelClassByIdAsync(Guid id)
    {
        try
        {
            return Ok(await _repo.CancelClassByIdAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{classId}/unregister-member/{memberId}")]
    public async Task<IActionResult> UnRegisterMemberFromClassAsync(Guid classId, Guid memberId)
    {
        try
        {
            return Ok(await _repo.UnRegisterMemberFromClassAsync(classId, memberId));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{classId}/unregister-member-from-waitinglist/{memberId}")]
    public async Task<IActionResult> UnRegisterMemberFromWaitingListAsync(Guid classId, Guid memberId)
    {
        try
        {
            return Ok(await _repo.UnRegisterMemberFromWaitingListAsync(classId, memberId));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> DeleteClassByIdAsync(Guid id)
    {
        try
        {
            return Ok(await _repo.DeleteClassByIdAsync(id));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}