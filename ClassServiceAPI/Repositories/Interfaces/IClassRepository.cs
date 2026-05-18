using ClassServiceAPI.Models;

namespace ClassServiceAPI.Repositories.Interfaces;

public interface IClassRepository
{
    // POST
    
    Task<Class> CreateClassAsync(Class classModel);
    Task<Class> RegisterMemberToClassAsync(Guid classId, Member member);
    Task<Class> RegisterMemberToWaitingListAsync(Guid classId, Member member);

    // GET
    
    Task<List<Class>> GetAllClassesAsync();
    Task<List<Class>> GetAllClassesByExerciseGymAsync(Guid exerciseGymId);
    Task<Class?> GetClassByIdAsync(Guid id);
    Task<List<Member>> GetWaitingListByClassAsync(Guid classId);
    Task<List<Member>> GetRegisteredByClassAsync(Guid classId);
    Task<int> GetNumberOfAttendeesByClassAsync(Guid classId);
    Task<double> CalculateAbsenceByClassAsync(Guid classId);

    // PUT
    
    Task<Class> CancelClassByIdAsync(Guid id);
    Task<Class> UnRegisterMemberFromClassAsync(Guid classId, Guid memberId);
    Task<Class> UnRegisterMemberFromWaitingListAsync(Guid classId, Guid memberId);

    // DELETE
    
    Task<Class> DeleteClassByIdAsync(Guid id);
}