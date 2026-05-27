using ClassServiceAPI.Models;

namespace ClassServiceAPI.Repositories.Interfaces;

public interface IClassRepository
{
    // POST
    
    Task<Class> CreateClassAsync(Class classModel);
    Task<Class> RegisterMemberToClassAsync(string classId, Member member);
    Task<Class> RegisterMemberToWaitingListAsync(string classId, Member member);

    // GET
    
    Task<List<Class>> GetAllClassesAsync();
    Task<List<Class>> GetClassesByExerciseGymAsync(string exerciseGymId);
    Task<Class?> GetClassByIdAsync(string id);
    Task<List<Class?>> GetClassesByMemberAsync(string id);
    Task<List<Class?>> GetClassesByEmployeeAsync(string id);
    
    Task<List<Member>> GetWaitingListByClassAsync(string classId);
    Task<List<Member>> GetMembersByClassAsync(string classId);
    Task<int> GetNumberOfAttendeesByClassAsync(string classId);

    // PUT
    
    Task<Class> CancelClassByIdAsync(string id);
    Task<Class> UnRegisterMemberFromClassAsync(string classId, string memberId);
    Task<Class> UnRegisterMemberFromWaitingListAsync(string classId, string memberId);

    // DELETE
    
    Task<Class> DeleteClassByIdAsync(string id);
    
    // EXTRA
    
    Task<int> CalculateAbsenceByClassAsync(string classId);
}