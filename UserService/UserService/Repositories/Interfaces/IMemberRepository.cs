using UserServiceAPI.Models;

namespace UserServiceAPI.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        public Task<List<Member>> GetAllMembers();
        public Task<Member> CancelMembershipForMember(string userId);
        public Task<Member> UpsertMember(Member member);
        public Task<Member> DeleteMember(string userId);
        public Task<Member> SetAccountAsInactive(string userId);
        public Task<Member?> GetMemberById(string userId);
        public Task<List<Member>> GetMembersByAffiliation(Guid affiliationId);
    }
}
