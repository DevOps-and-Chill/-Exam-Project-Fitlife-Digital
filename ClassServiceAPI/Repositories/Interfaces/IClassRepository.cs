using ClassServiceAPI.Models;

namespace ClassServiceAPI.Repositories.Interfaces;

public interface IClassRepository
{
    // Post
    Task CreateClassAsync(Class classModel);
    Task RegisterMemberToClassByMemberIdAsync(int memberId);
    Task RegisterMemberToWaitingListByMemberIdAsync(int memberId);
    
    
    // Get
    Task GetAllClassesAsync();
    Task GetAllClassesByExerciseGymAsync(int exerciseGymId);
    Task GetClassByIdAsync(int id);
    Task GetClassRatingByIdAsync(int id);
    Task GetWaitingListByClassAsync(int id);
    Task GetRegisteredByClassAsync(int id);
    Task GetNumberOfAttendeesByClassAsync(int id);
    Task CalculateAbsenceByClassAsync(int id);
    
   
    // Put
    Task CancelClassByIdAsync(int id);
    Task RateClassByIdAsync(int id, double rating);
    Task UnRegisterMemberFromClassByClassAndMemberAsync(int memberId, int classId);
    Task UnRegisterMemberFromWaitingListByClassAndMemberAsync(int memberId, int classId);
    
    // Delete
    Task DeleteClassByIdAsync(int id);
}
