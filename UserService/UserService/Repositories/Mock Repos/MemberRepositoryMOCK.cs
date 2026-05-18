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
        public Task<bool> CancelMembershipForMember(string userId)
        {
            var memberToCancel = members.SingleOrDefault(m => m.Id == userId);

            if (memberToCancel == null)
            {
                return Task.FromResult(false);
            }

            memberToCancel.CancelMembership();

            return Task.FromResult(true);
        }

        public Task<Member> DeleteMember(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllMembers()
        {
            List<User> result = members
                .Cast<User>()
                .ToList();

            return Task.FromResult(result);
        }

        public Task<Member?> GetMemberById(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Member>> GetMembersByAffiliation(Guid affiliationId)
        {
            throw new NotImplementedException();
        }

        public Task<Member> SetAccountAsInactive(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Member> UpsertMember(Member member)
        {
            throw new NotImplementedException();
        }

        Task<Member> IMemberRepository.CancelMembershipForMember(string userId)
        {
            throw new NotImplementedException();
        }

        Task<List<Member>> IMemberRepository.GetAllMembers()
        {
            throw new NotImplementedException();
        }
    }
}
