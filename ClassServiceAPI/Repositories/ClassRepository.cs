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
    
    // Post
    
    public Task CreateClassAsync(Class classModel)
    {
       /*
        await _collection.InsertOneAsync(classModel);
        */
       
        throw new NotImplementedException();
    }
        
    public Task RegisterMemberToClassByMemberIdAsync(int memberId) { 
        throw new NotImplementedException();
    }
    public Task RegisterMemberToWaitingListByMemberIdAsync(int memberId) {
        throw new NotImplementedException();
    }
    
    // Get
    
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
        
    // Put
    
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
    
    // Delete
        
    public async Task DeleteClassByIdAsync(int id) {
        throw new NotImplementedException();
    }
}