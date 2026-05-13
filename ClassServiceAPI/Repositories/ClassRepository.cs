using ClassServiceAPI.Models;
using ClassServiceAPI.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;

namespace ClassServiceAPI.Repositories;

public class ClassRepository : IClassRepository
{
    private readonly Container _container;
    private readonly ILogger<ClassRepository> _logger;
    public ClassRepository(CosmosClient cosmosClient, string databaseName, string containerName, ILogger<ClassRepository> logger) {
        
        _container = cosmosClient.GetContainer(databaseName, containerName);
        _logger = logger;
    }
    
    // POST
    
    public Task CreateClassAsync(Class classModel)
    {
        throw new NotImplementedException();
    }
        
    public Task RegisterMemberToClassByMemberIdAsync(int memberId) { 
        throw new NotImplementedException();
    }
    public Task RegisterMemberToWaitingListByMemberIdAsync(int memberId) {
        throw new NotImplementedException();
    }
    
    // GET
    
    public async Task GetAllClassesAsync() {
        throw new NotImplementedException();
    }
        
    public async Task GetAllClassesByExerciseGymAsync(int exerciseGymId) {
        throw new NotImplementedException();
    }
        
    public async Task GetClassByIdAsync(int id) {
        throw new NotImplementedException();
    }
        
    public async Task GetClassRatingByIdAsync(int id) { 
        throw new NotImplementedException();
    }
        
    public async Task GetWaitingListByClassAsync(int id) { 
        throw new NotImplementedException();
    }
        
    public async Task GetRegisteredByClassAsync(int id) {
        throw new NotImplementedException();
    }
        
    public async Task GetNumberOfAttendeesByClassAsync(int id) {
        throw new NotImplementedException();
            }
        
    public async Task CalculateAbsenceByClassAsync(int id) {
        throw new NotImplementedException();
    }
        
    // PUT
    
    public async Task CancelClassByIdAsync(int id) {
        throw new NotImplementedException();
    }
        
    public async Task RateClassByIdAsync(int id, double rating) {
                throw new NotImplementedException();
    }
         
    public async Task UnRegisterMemberFromClassByClassAndMemberAsync(int memberId, int classId) { 
        throw new NotImplementedException();
    }
        
    public async Task UnRegisterMemberFromWaitingListByClassAndMemberAsync(int memberId, int classId) {
        throw new NotImplementedException();
    }
    
    // DELETE
        
    public async Task DeleteClassByIdAsync(int id) {
        throw new NotImplementedException();
    }
}