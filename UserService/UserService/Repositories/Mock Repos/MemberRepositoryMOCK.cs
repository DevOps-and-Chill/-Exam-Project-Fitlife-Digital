using UserServiceAPI.Models;
using UserServiceAPI.Models.Enums;
using UserServiceAPI.Repositories.Interfaces;
using UserServiceAPI.TestData;

namespace UserServiceAPI.Repositories
{
    public class MemberRepositoryMOCK : IMemberRepository
    {
        List<Member> members = MemberTestData.members;

        /// <summary>
        /// Used to cancel membership for member 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True if member is found and cancelled. False if member is null</returns>
        public Task<bool> CancelMembershipForMember(Guid userId)
        {
            var memberToCancel = members.SingleOrDefault(m => m.Id == userId);

            if (memberToCancel == null)
            {
                return Task.FromResult(false);
            }

            memberToCancel.CancelMembership();

            return Task.FromResult(true);
        }

        public Task<List<User>> GetAllMembers()
        {
            List<User> result = members
                .Cast<User>()
                .ToList();

            return Task.FromResult(result);
        }
    }
}
