using Microsoft.AspNetCore.Mvc;
using ClassServiceAPI.Repositories;
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
        await _repo.CreateClassAsync(classModel);
        return Ok();
    }

    [HttpPost("register-member-to-class")]
    public async Task<IActionResult> RegisterMemberToClassByMemberIdAsync(int memberId)
    {
        await _repo.RegisterMemberToClassByMemberIdAsync(memberId);
        return Ok();
    }

    [HttpPost("register-member-to-waitinglist")]
    public async Task<IActionResult> RegisterMemberToWaitingListByMemberIdAsync(int memberId)
    {
        await _repo.RegisterMemberToWaitingListByMemberIdAsync(memberId);
        return Ok();
    }

    // GET

    [HttpGet("get-all-classes")]
    public async Task<IActionResult> GetAllClassesAsync()
    {
        await _repo.GetAllClassesAsync();
        return Ok();
    }

    [HttpGet("get-classes-by-gym/{exerciseGymId}")]
    public async Task<IActionResult> GetAllClassesByExerciseGymAsync(int exerciseGymId)
    {
        await _repo.GetAllClassesByExerciseGymAsync(exerciseGymId);
        return Ok();
    }

    [HttpGet("get-class-by-id/{id}")]
    public async Task<IActionResult> GetClassByIdAsync(int id)
    {
        await _repo.GetClassByIdAsync(id);
        return Ok();
    }

    [HttpGet("get-class-rating/{id}")]
    public async Task<IActionResult> GetClassRatingByIdAsync(int id)
    {
        await _repo.GetClassRatingByIdAsync(id);
        return Ok();
    }

    [HttpGet("get-waitinglist/{id}")]
    public async Task<IActionResult> GetWaitingListByClassAsync(int id)
    {
        await _repo.GetWaitingListByClassAsync(id);
        return Ok();
    }

    [HttpGet("get-registered-members/{id}")]
    public async Task<IActionResult> GetRegisteredByClassAsync(int id)
    {
        await _repo.GetRegisteredByClassAsync(id);
        return Ok();
    }

    [HttpGet("get-attendees-count/{id}")]
    public async Task<IActionResult> GetNumberOfAttendeesByClassAsync(int id)
    {
        await _repo.GetNumberOfAttendeesByClassAsync(id);
        return Ok();
    }

    [HttpGet("calculate-absence/{id}")]
    public async Task<IActionResult> CalculateAbsenceByClassAsync(int id)
    {
        await _repo.CalculateAbsenceByClassAsync(id);
        return Ok();
    }

    // PUT

    [HttpPut("cancel-class/{id}")]
    public async Task<IActionResult> CancelClassByIdAsync(int id)
    {
        await _repo.CancelClassByIdAsync(id);
        return Ok();
    }

    [HttpPut("rate-class/{id}")]
    public async Task<IActionResult> RateClassByIdAsync(int id, double rating)
    {
        await _repo.RateClassByIdAsync(id, rating);
        return Ok();
    }

    [HttpPut("unregister-member-from-class")]
    public async Task<IActionResult> UnRegisterMemberFromClassByClassAndMemberAsync(int memberId, int classId)
    {
        await _repo.UnRegisterMemberFromClassByClassAndMemberAsync(memberId, classId);
        return Ok();
    }

    [HttpPut("unregister-member-from-waitinglist")]
    public async Task<IActionResult> UnRegisterMemberFromWaitingListByClassAndMemberAsync(int memberId, int classId)
    {
        await _repo.UnRegisterMemberFromWaitingListByClassAndMemberAsync(memberId, classId);
        return Ok();
    }

    // DELETE

    [HttpDelete("delete-class/{id}")]
    public async Task<IActionResult> DeleteClassByIdAsync(int id)
    {
        await _repo.DeleteClassByIdAsync(id);
        return Ok();
    }
}