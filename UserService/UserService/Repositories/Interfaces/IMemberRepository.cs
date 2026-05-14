using UserServiceAPI.Models;

namespace UserServiceAPI.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        public Task<List<User>> GetAllMembers();
        public Task<bool> CancelMembershipForMember(Guid userId);

    }
}
