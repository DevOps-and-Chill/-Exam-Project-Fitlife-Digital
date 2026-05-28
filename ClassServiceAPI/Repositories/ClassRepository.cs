using ClassServiceAPI.Data;
using ClassServiceAPI.Messaging;
using ClassServiceAPI.Models;
using ClassServiceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClassServiceAPI.Repositories;

public class ClassRepository : IClassRepository
{
    private readonly ClassDbContext _context;
    private readonly ILogger<ClassRepository> _logger;
    private readonly IMessagePublisher _publisher;

    public ClassRepository(ClassDbContext context, ILogger<ClassRepository> logger, IMessagePublisher publisher)
    {
        _context = context;
        _logger = logger;
        _publisher = publisher;
    }

    // POST

    public async Task<Class> CreateClassAsync(Class classModel)
    {
        _context.Classes.Add(classModel);
        await _context.SaveChangesAsync();
        return classModel;
    }

    public async Task<Class> RegisterMemberToClassAsync(string classId, Member member)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        if (!fitnessClass.ActiveClass)
            throw new InvalidOperationException("Cannot register to an inactive class");

        if (fitnessClass.Members.Any(m => m.Id == member.Id))
            throw new InvalidOperationException("Member is already Members");

        if (fitnessClass.Members.Count >= fitnessClass.MemberLimit)
        {
            return await RegisterMemberToWaitingListAsync(classId, member);
        }

        fitnessClass.Members.Add(member);
        await _context.SaveChangesAsync();
        return fitnessClass;
    }

    public async Task<Class> RegisterMemberToWaitingListAsync(string classId, Member member)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        if (fitnessClass.WaitingList.Any(m => m.Id == member.Id))
            throw new InvalidOperationException("Member is already on the waiting list");

        fitnessClass.WaitingList.Add(member);
        await _context.SaveChangesAsync();
        return fitnessClass;
    }

    // GET

    public async Task<List<Class>> GetAllClassesAsync()
    {
        return await _context.Classes
            .Where(c => c.ActiveClass)
            .ToListAsync();
    }

    public async Task<List<Class>> GetClassesByExerciseGymAsync(string exerciseGymId)
    {
        return await _context.Classes
            .Where(c => c.ExerciseGymId == exerciseGymId)
            .ToListAsync();
    }

    public async Task<Class?> GetClassByIdAsync(string id)
    {
        return await _context.Classes
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<List<Class?>> GetClassesByMemberAsync(string id)
    {
        return await _context.Classes
            .Where(c => c.Members.Any(m => m.Id == id))
            .ToListAsync();
    }
    
    public async Task<List<Class?>> GetClassesByEmployeeAsync(string id)
    {
        return await _context.Classes
            .Where(c => c.CoachId == id)
            .ToListAsync();
    }

    

    public async Task<List<Member>> GetWaitingListByClassAsync(string classId)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        return fitnessClass.WaitingList;
    }

    public async Task<List<Member>> GetMembersByClassAsync(string classId)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        return fitnessClass.Members;
    }

    public async Task<int> GetNumberOfAttendeesByClassAsync(string classId)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        return fitnessClass.AttendedMembers.Count;
    }
    
    public async Task<int> CalculateAbsenceByClassAsync(string classId)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        if (fitnessClass.Members.Count == 0) return 0;

        var AttendedMembers = fitnessClass.AttendedMembers.Select(m => m.Id).ToHashSet();
        int absent = fitnessClass.Members.Count - AttendedMembers.Count;

        return absent;
    }

    // PUT

    public async Task<Class> CancelClassAsync(string id)
    {
        var fitnessClass = await GetClassByIdAsync(id)
                           ?? throw new InvalidOperationException($"Class '{id}' not found");

        fitnessClass.ActiveClass = false;
        await _context.SaveChangesAsync();

        // Publicére beskeden til rabbit
        var message = new ClassCancelledMessage
        {
            ClassId   = fitnessClass.Id,
            Title     = fitnessClass.Title,
            TimeStart = fitnessClass.TimeStart,
            TimeEnd   = fitnessClass.TimeEnd,
            MemberIds = fitnessClass.Members
                .Select(m => m.Id)
                .ToList()
        };

        await _publisher.PublishAsync(message, "class.cancelled");
        _logger.LogInformation("Published cancellation for class {ClassId} to RabbitMQ", id);

        return fitnessClass;
    }
    
    public async Task<Class> UpdateClassAsync(string id, Class updatedClass)
    {
        var fitnessClass = await GetClassByIdAsync(id)
                           ?? throw new InvalidOperationException($"Class '{id}' not found");

        fitnessClass.Title = updatedClass.Title;
        fitnessClass.CoachId = updatedClass.CoachId;
        fitnessClass.TimeStart = updatedClass.TimeStart;
        fitnessClass.TimeEnd = updatedClass.TimeEnd;
        fitnessClass.MemberLimit = updatedClass.MemberLimit;
        fitnessClass.RoomId = updatedClass.RoomId;
        fitnessClass.ActiveClass = updatedClass.ActiveClass;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated class {ClassId}", id);

        return fitnessClass;
    }

    public async Task<Class> UnRegisterMemberFromClassAsync(string classId, string memberId)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        var member = fitnessClass.Members.FirstOrDefault(m => m.Id == memberId)
            ?? throw new InvalidOperationException("Member is not Members in this class");

        fitnessClass.Members.Remove(member);
        
        // Så waiting list bliver opdateret
        var next = fitnessClass.WaitingList.FirstOrDefault();
        if (next is not null)
        {
            fitnessClass.WaitingList.Remove(next);
            fitnessClass.Members.Add(next);
            _logger.LogInformation("Member {MemberId} moved from waiting list to Members", next.Id);
        }

        await _context.SaveChangesAsync();
        return fitnessClass;
    }

    public async Task<Class> UnRegisterMemberFromWaitingListAsync(string classId, string memberId)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        var member = fitnessClass.WaitingList.FirstOrDefault(m => m.Id == memberId)
            ?? throw new InvalidOperationException("Member is not on the waiting list");

        fitnessClass.WaitingList.Remove(member);
        await _context.SaveChangesAsync();
        return fitnessClass;
    }

    // DELETE

    public async Task<Class> DeleteClassByIdAsync(string id)
    {
        var fitnessClass = await GetClassByIdAsync(id)
            ?? throw new InvalidOperationException($"Class '{id}' not found");

        _context.Classes.Remove(fitnessClass);
        await _context.SaveChangesAsync();
        return fitnessClass;
    }
}