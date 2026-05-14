using UserServiceAPI.Models;
using UserServiceAPI.Repositories.Interfaces;

namespace UserServiceAPI.Repositories
{
    public class MemberRepositoryDB : IMemberRepository
    {
        public Task<bool> CancelMembershipForMember(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllMembers()
        {
            throw new NotImplementedException();
        }
    }
}
