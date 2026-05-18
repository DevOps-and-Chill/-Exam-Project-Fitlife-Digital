using Microsoft.AspNetCore.Mvc;
using UserServiceAPI.Models;
using UserServiceAPI.Repositories;
using UserServiceAPI.Repositories.Interfaces;

namespace UserServiceAPI.Controllers
{
    [ApiController]
    [Route("member")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;

        public MemberController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [HttpGet("GetAllMembers")]
        public async Task<ActionResult> GetAllMembers()
        {
            try
            {
                return Ok(await _memberRepository.GetAllMembers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CancelMembership/{userId}")]
        public async Task<ActionResult> CancelMembership(string userId)
        {
            try
            {
                return Ok(await _memberRepository.CancelMembershipForMember(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpsertMember")]
        public async Task<ActionResult> UpsertMember([FromBody] Member member)
        {
            try
            {
                Member updatedMember = await _memberRepository.UpsertMember(member);

                return Ok(updatedMember);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteMember/{userId}")]
        public async Task<ActionResult> DeleteMember(string userId)
        {
            try
            {
                return Ok(await _memberRepository.DeleteMember(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("SetAccountAsInactive/{userId}")]
        public async Task<ActionResult> SetMemberAccountAsInactive(string userId)
        {
            try
            {
                return Ok(await _memberRepository.SetAccountAsInactive(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMemberById/{userId}")]
        public async Task<ActionResult> GetMemberById(string userId)
        {
            try
            {
                Member? member = await _memberRepository.GetMemberById(userId);

                if (member is null)
                {
                    return NotFound(
                        $"Member with id '{userId}' was not found");
                }

                return Ok(member);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMemberByAffiliation/{affiliationId}")]
        public async Task<ActionResult> GetMemberByAffiliation(Guid affiliationId)
        {
            try
            {
                var members = await _memberRepository.GetMembersByAffiliation(affiliationId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
