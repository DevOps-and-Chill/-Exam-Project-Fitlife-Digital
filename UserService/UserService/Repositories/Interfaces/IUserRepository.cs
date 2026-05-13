using UserServiceAPI.Models;

namespace UserServiceAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsers();
        public Task<List<User>> GetAllMembers();
        public Task<List<User>> GetAllEmployees();
        public Task<List<User>> GetUsersInExerciseGymByRole(Guid exerciseGymId, string role);
        public Task<User> UpsertUser();
        public Task<bool> DeleteUser(Guid userId);
        public Task<bool> SetUserAsInactive(Guid userId);
        public Task<bool> CancelMembershipForMember(Guid userId);
        public Task<bool> EndEmploymentForEmployee(Guid userId);
        public Task<bool> SetEmployeeAsManagerForExerciseGym(Guid exerciseGymId,Guid userId);

        
    }
}
