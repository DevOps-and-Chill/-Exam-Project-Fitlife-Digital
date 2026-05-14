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
                return BadRequest(ex);
            }
        }

        [HttpPut("CancelMembership")]
        public async Task<ActionResult> CancelMembership(Guid userId)
        {
            try
            {
                return Ok(await _memberRepository.CancelMembershipForMember(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
