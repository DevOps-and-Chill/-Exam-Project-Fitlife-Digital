using Microsoft.EntityFrameworkCore;
using UserServiceAPI.Data;
using UserServiceAPI.Models;
using UserServiceAPI.Repositories.Interfaces;

namespace UserServiceAPI.Repositories
{
    public class MemberRepositoryDB : IMemberRepository
    {
        private readonly UserDbContext _context;

        public MemberRepositoryDB(UserDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Sets the property ActiveMembership to false and endDate to "now"
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns updated member-object</returns>
        public async Task<Member> CancelMembershipForMember(string userId)
        {
            var member = await GetMemberById(userId);

            if (member == null)
            {
                return null;
            }

            member.CancelMembership();

            await _context.SaveChangesAsync();

            return member;
        }

        /// <summary>
        /// Removes the member from the db
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns the member-object that has been removed</returns>
        public async Task<Member> DeleteMember(string userId)
        {
            var member = await GetMemberById(userId);

            if (member == null)
            {
                return null;
            }

            _context.Users.Remove(member);

            await _context.SaveChangesAsync();

            return member;
        }

        /// <summary>
        /// Returns all object of type Member from the db
        /// </summary>
        /// <returns>Returns a list of members</returns>
        public async Task<List<Member>> GetAllMembers()
        {
            return await _context.Set<Member>().ToListAsync();
        }

        /// <summary>
        /// Sets the property "ActiveUser" to false for the Member
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns the updated member-object</returns>
        public async Task<Member> SetAccountAsInactive(string userId)
        {
            var member = await GetMemberById(userId);

            if (member == null)
            {
                return null;
            }

            member.SetUserAsInactive();

            await _context.SaveChangesAsync();

            return member;
        }

        /// <summary>
        /// Finds the member based on the userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns member-object, returns null if not found</returns>
        public async Task<Member?> GetMemberById(string userId)
        {
            var member = await _context.Set<Member>()
                .FirstOrDefaultAsync(m => m.Id == userId);

            return member;
        }

        /// <summary>
        /// Adds a new Member or updates an existing Member and persists the change to the data store.
        /// </summary>
        /// <remarks>Performs an email uniqueness check, updates profile and membership fields on the
        /// existing entity when present, and calls SaveChangesAsync to persist changes.</remarks>
        /// <param name="member">Member to add or update; if Id matches an existing user, that user's profile and membership are updated,
        /// otherwise a new user is created.</param>
        /// <returns>The persisted Member entity — either the newly added instance or the updated existing instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when another user already exists with the same Email.</exception>
        public async Task<Member> UpsertMember(Member member)
        {
            //CS: Midlertidigt slået fra pga. Cosmos query-fejl ved AnyAsync på inheritance hierarchy
            // bool emailExists = await _context.Set<Member>()
            //     .AnyAsync(u =>
            //         u.Email == member.Email &&
            //         u.Id != member.Id);

            // if (emailExists)
            // {
            //     throw new InvalidOperationException(
            //         $"Email '{member.Email}' is already in use");
            // }

            Member? existingMember = await GetMemberById(member.Id);

            if (existingMember is null)
            {
                _context.Users.Add(member);
            }
            else
            {
                existingMember.UpdateUserInformation(
                    member.RoleName,
                    member.GivenName,
                    member.FamilyName,
                    member.BirthDate,
                    member.Address,
                    member.Telephone,
                    member.Email,
                    member.Affiliation,
                    member.ActiveUser);

                existingMember.UpdateMembership(
                    member.MembershipType,
                    member.MembershipOptional,
                    member.StartDate,
                    member.EndDate);
            }

            await _context.SaveChangesAsync();

            return existingMember ?? member;
        }

        /// <summary>
        /// Finds the members based on their affiliation (property on base class) 
        /// </summary>
        /// <param name="affiliationId">Guid representing the id for the facility</param>
        /// <returns>Return list of members</returns>
        public async Task<List<Member>> GetMembersByAffiliation(Guid affiliationId)
        {
            return await _context.Set<Member>()
                .Where(e => e.Affiliation == affiliationId)
                .ToListAsync();
        }
    }
}