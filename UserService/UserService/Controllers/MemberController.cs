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

        [HttpPut("CancelMembership")]
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
                return Ok(await _memberRepository.UpsertMember(member));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteMember")]
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

        [HttpPut("SetAccountAsInactive")]
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



    }
}
