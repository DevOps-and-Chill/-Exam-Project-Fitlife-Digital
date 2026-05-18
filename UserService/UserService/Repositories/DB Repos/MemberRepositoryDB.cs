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

        public async Task<List<Member>> GetAllMembers()
        {
            return await _context.Users.OfType<Member>().ToListAsync();
        }

        public async Task<Member?> SetAccountAsInactive(string userId)
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

        public async Task<Member?> GetMemberById(string userId)
        {
            var member = await _context.Users
                .OfType<Member>()
                .FirstOrDefaultAsync(m => m.Id == userId);
            return member;
        }

        public async Task<Member> UpsertMember(Member member)
        {
            bool emailExists = await _context.Users
                .AnyAsync(u =>
                    u.Email == member.Email &&
                    u.Id != member.Id);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    $"Email '{member.Email}' is already in use");
            }

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

        public async Task<List<Member>> GetMembersByAffiliation(Guid affiliationId)
        {
            return await _context.Users
                .OfType<Member>()
                .Where(e => e.Affiliation == affiliationId)
                .ToListAsync();
        }

    }
}
