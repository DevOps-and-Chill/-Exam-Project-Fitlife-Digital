using ClassServiceAPI.Models;

namespace ClassServiceAPI.Repositories.Interfaces;

public interface IClassRepository
{
    // POST
    Task CreateClassAsync(Class classModel);
    Task RegisterMemberToClassByMemberIdAsync(int memberId);
    Task RegisterMemberToWaitingListByMemberIdAsync(int memberId);
    
    
    // GET
    Task GetAllClassesAsync();
    Task GetAllClassesByExerciseGymAsync(int exerciseGymId);
    Task GetClassByIdAsync(int id);
    Task GetClassRatingByIdAsync(int id);
    Task GetWaitingListByClassAsync(int id);
    Task GetRegisteredByClassAsync(int id);
    Task GetNumberOfAttendeesByClassAsync(int id);
    Task CalculateAbsenceByClassAsync(int id);
    
   
    // PUT
    Task CancelClassByIdAsync(int id);
    Task RateClassByIdAsync(int id, double rating);
    Task UnRegisterMemberFromClassByClassAndMemberAsync(int memberId, int classId);
    Task UnRegisterMemberFromWaitingListByClassAndMemberAsync(int memberId, int classId);
    
    // DELETE
    Task DeleteClassByIdAsync(int id);
}
