using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<MemberController> _logger;

        public MemberController(IMemberRepository memberRepository, ILogger<MemberController> logger)
        {
            _memberRepository = memberRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all members.
        /// </summary>
        [Authorize]
        [HttpGet("GetAllMembers")]
        public async Task<ActionResult> GetAllMembers()
        {
            _logger.LogDebug("Fetching all members");
            try
            {
                return Ok(await _memberRepository.GetAllMembers());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all members: {message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cancels the membership for a member.
        /// </summary>
        [Authorize]
        [HttpPut("CancelMembership/{userId}")]
        public async Task<ActionResult> CancelMembership(string userId)
        {
            _logger.LogDebug("Cancelling membership for user: {userId}", userId);
            try
            {
                return Ok(await _memberRepository.CancelMembershipForMember(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error cancelling membership for {userId}: {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates or updates a member.
        /// AO: AllowAnonymous is not ideal, but due to time its a necessary temp fix
        /// </summary>
        [AllowAnonymous]
        [HttpPost("UpsertMember")]
        public async Task<ActionResult> UpsertMember([FromBody] Member member)
        {
            _logger.LogDebug("Upserting member: {memberId}", member.Id);
            try
            {
                Member updatedMember = await _memberRepository.UpsertMember(member);
                _logger.LogInformation("Member upserted: {memberId}", member.Id);
                return Ok(updatedMember);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation while upserting member {MemberId}: {Message}", member.Id, ex.Message);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a member.
        /// </summary>
        [Authorize]
        [HttpDelete("DeleteMember/{userId}")]
        public async Task<ActionResult> DeleteMember(string userId)
        {
            _logger.LogDebug("Deleting member: {userId}", userId);
            try
            {
                return Ok(await _memberRepository.DeleteMember(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete member {UserId}: {Message}", userId, ex.Message);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Sets a member account as inactive.
        /// </summary>
        [Authorize]
        [HttpPut("SetAccountAsInactive/{userId}")]
        public async Task<ActionResult> SetMemberAccountAsInactive(string userId)
        {
            _logger.LogDebug("Setting account as inactive for user: {userId}", userId);
            try
            {
                return Ok(await _memberRepository.SetAccountAsInactive(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deactivating account for {userId}: {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a member by id.
        /// </summary>
        [Authorize]
        [HttpGet("GetMemberById/{userId}")]
        public async Task<ActionResult> GetMemberById(string userId)
        {
            _logger.LogInformation("Retrieving member with id {UserId}", userId);

            try
            {
                Member? member = await _memberRepository.GetMemberById(userId);

                if (member is null)
                {
                    _logger.LogWarning("Member with id {UserId} was not found", userId);

                    return NotFound($"Member with id '{userId}' was not found");
                }

                return Ok(member);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to retrieve member {UserId}: {Message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all members affiliated with a specific gym.
        /// </summary>
        [Authorize]
        [HttpGet("GetMemberByAffiliation/{affiliationId}")]
        public async Task<ActionResult> GetMemberByAffiliation(Guid affiliationId)
        {
            _logger.LogInformation("Retrieving members for affiliation {AffiliationId}", affiliationId);

            try
            {
                var members = await _memberRepository.GetMembersByAffiliation(affiliationId);

                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to retrieve members for affiliation {AffiliationId}: {Message}", affiliationId, ex.Message);

                return BadRequest(ex.Message);
            }
        }
    }
}