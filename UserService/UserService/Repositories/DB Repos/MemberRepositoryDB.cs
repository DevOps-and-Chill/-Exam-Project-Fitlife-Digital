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
            var member = await _context.Users
                .OfType<Member>()
                .FirstOrDefaultAsync(m => m.Id == userId);

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
            var member = await _context.Users
            .OfType<Member>()
            .FirstOrDefaultAsync(m => m.Id == userId);

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
            var member = await _context.Users
                .OfType<Member>()
                .FirstOrDefaultAsync(m => m.Id == userId);

            if (member == null)
            {
                return null;
            }

            member.SetUserAsInactive();

            await _context.SaveChangesAsync();

            return member;
        }

        public async Task<Member> UpsertMember(Member member)
        {
            _context.Users.Add(member);

            await _context.SaveChangesAsync();

            return member;
        }
    
    }
}
