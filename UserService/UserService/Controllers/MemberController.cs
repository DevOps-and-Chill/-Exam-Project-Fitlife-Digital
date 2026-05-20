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
        /// <returns>
        /// Returns a list of all members.
        /// </returns>
        [HttpGet("GetAllMembers")]
        public async Task<ActionResult> GetAllMembers()
        {
            _logger.LogInformation("Henter alle medlemmer");
            try
            {
                return Ok(await _memberRepository.GetAllMembers());
            }
            catch (Exception ex)
            {
                _logger.LogError("Fejl ved hentning af alle medlemmer: {message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cancels the membership for a member.
        /// </summary>
        /// <param name="userId">
        /// The id of the member.
        /// </param>
        /// <returns>
        /// Returns the updated member object with the membership set as inactive.
        /// </returns>
        [HttpPut("CancelMembership/{userId}")]
        public async Task<ActionResult> CancelMembership(string userId)
        {
            _logger.LogInformation("Annullerer medlemskab for bruger: {userId}", userId);
            try
            {
                return Ok(await _memberRepository.CancelMembershipForMember(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Fejl ved annullering af medlemskab for {userId}: {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates or updates a member.
        /// </summary>
        /// <param name="member">
        /// The member object to create or update.
        /// </param>
        /// <returns>
        /// Returns the created or updated member object.
        /// </returns>
        [HttpPost("UpsertMember")]
        public async Task<ActionResult> UpsertMember([FromBody] Member member)
        {
            _logger.LogInformation("Opretter eller opdaterer medlem: {memberId}", member.Id);
            try
            {
                Member updatedMember = await _memberRepository.UpsertMember(member);
                _logger.LogInformation("Medlem oprettet/opdateret: {memberId}", member.Id);
                return Ok(updatedMember);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Ugyldig operation ved upsert af medlem {memberId}: {message}", member.Id, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a member.
        /// </summary>
        /// <param name="userId">
        /// The id of the member.
        /// </param>
        /// <returns>
        /// Returns the deleted member object.
        /// </returns>
        [HttpDelete("DeleteMember/{userId}")]
        public async Task<ActionResult> DeleteMember(string userId)
        {
            _logger.LogInformation("Sletter medlem: {userId}", userId);
            try
            {
                return Ok(await _memberRepository.DeleteMember(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Fejl ved sletning af medlem {userId}: {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Sets a member account as inactive.
        /// </summary>
        /// <param name="userId">
        /// The id of the member.
        /// </param>
        /// <returns>
        /// Returns the updated member object with ActiveUser set to false.
        /// </returns>
        [HttpPut("SetAccountAsInactive/{userId}")]
        public async Task<ActionResult> SetMemberAccountAsInactive(string userId)
        {
            _logger.LogInformation("Sætter konto som inaktiv for bruger: {userId}", userId);
            try
            {
                return Ok(await _memberRepository.SetAccountAsInactive(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Fejl ved deaktivering af konto for {userId}: {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a member by id.
        /// </summary>
        /// <param name="userId">
        /// The id of the member.
        /// </param>
        /// <returns>
        /// Returns the member object if found.
        /// Returns NotFound if no member exists with the provided id.
        /// </returns>
        [HttpGet("GetMemberById/{userId}")]
        public async Task<ActionResult> GetMemberById(string userId)
        {
            _logger.LogInformation("Henter medlem med id: {userId}", userId);
            try
            {
                Member? member = await _memberRepository.GetMemberById(userId);

                if (member is null)
                {
                    _logger.LogWarning("Medlem med id {userId} blev ikke fundet", userId);
                    return NotFound(
                        $"Member with id '{userId}' was not found");
                }

                return Ok(member);
            }
            catch (Exception ex)
            {
                _logger.LogError("Fejl ved hentning af medlem {userId}: {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all members affiliated with a specific gym.
        /// </summary>
        /// <param name="affiliationId">
        /// The affiliation id of the gym.
        /// </param>
        /// <returns>
        /// Returns a list of members associated with the provided affiliation id.
        /// </returns>
        [HttpGet("GetMemberByAffiliation/{affiliationId}")]
        public async Task<ActionResult> GetMemberByAffiliation(Guid affiliationId)
        {
            _logger.LogInformation("Henter medlemmer for tilknytning: {affiliationId}", affiliationId);
            try
            {
                var members = await _memberRepository.GetMembersByAffiliation(affiliationId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError("Fejl ved hentning af medlemmer for tilknytning {affiliationId}: {message}", affiliationId, ex.Message);
                return BadRequest(ex.Message);
            }
        }


    }
}
