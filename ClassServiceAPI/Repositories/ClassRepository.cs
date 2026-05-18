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
        _context   = context;
        _logger    = logger;
    }

    // POST

    public async Task<Class> CreateClassAsync(Class classModel)
    {
        _context.Classes.Add(classModel);
        await _context.SaveChangesAsync();
        return classModel;
    }

    public async Task<Class> RegisterMemberToClassAsync(Guid classId, Member member)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        if (!fitnessClass.ActiveClass)
            throw new InvalidOperationException("Cannot register to an inactive class");

        if (fitnessClass.Registered.Any(m => m.Id == member.Id))
            throw new InvalidOperationException("Member is already registered");

        if (fitnessClass.Registered.Count >= fitnessClass.MemberLimit)
        {
            // Fuld — flyt til venteliste i stedet
            return await RegisterMemberToWaitingListAsync(classId, member);
        }

        fitnessClass.Registered.Add(member);
        await _context.SaveChangesAsync();
        return fitnessClass;
    }

    public async Task<Class> RegisterMemberToWaitingListAsync(Guid classId, Member member)
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
        return await _context.Classes.ToListAsync();
    }

    public async Task<List<Class>> GetAllClassesByExerciseGymAsync(Guid exerciseGymId)
    {
        return await _context.Classes
            .Where(c => c.ExerciseGymId == exerciseGymId)
            .ToListAsync();
    }

    public async Task<Class?> GetClassByIdAsync(Guid id)
    {
        return await _context.Classes
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Member>> GetWaitingListByClassAsync(Guid classId)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        return fitnessClass.WaitingList;
    }

    public async Task<List<Member>> GetRegisteredByClassAsync(Guid classId)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        return fitnessClass.Registered;
    }

    public async Task<int> GetNumberOfAttendeesByClassAsync(Guid classId)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        return fitnessClass.Attended.Count;
    }

    // Fraværsprocent: tilmeldte der IKKE mødte op
    public async Task<double> CalculateAbsenceByClassAsync(Guid classId)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        if (fitnessClass.Registered.Count == 0) return 0;

        var attendedIds = fitnessClass.Attended.Select(m => m.Id).ToHashSet();
        int absent = fitnessClass.Registered.Count(m => !attendedIds.Contains(m.Id));

        return (double)absent / fitnessClass.Registered.Count * 100;
    }

    // PUT

    public async Task<Class> CancelClassByIdAsync(Guid id)
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
            MemberIds = fitnessClass.Registered
                .Select(m => m.Id)
                .ToList()
        };

        await _publisher.PublishAsync(message, "class.cancelled");
        _logger.LogInformation("Published cancellation for class {ClassId} to RabbitMQ", id);

        return fitnessClass;
    }

    public async Task<Class> UnRegisterMemberFromClassAsync(Guid classId, Guid memberId)
    {
        var fitnessClass = await GetClassByIdAsync(classId)
            ?? throw new InvalidOperationException($"Class '{classId}' not found");

        var member = fitnessClass.Registered.FirstOrDefault(m => m.Id == memberId)
            ?? throw new InvalidOperationException("Member is not registered in this class");

        fitnessClass.Registered.Remove(member);

        // Ryk første person på ventelisten op automatisk
        var next = fitnessClass.WaitingList.FirstOrDefault();
        if (next is not null)
        {
            fitnessClass.WaitingList.Remove(next);
            fitnessClass.Registered.Add(next);
            _logger.LogInformation("Member {MemberId} moved from waiting list to registered", next.Id);
        }

        await _context.SaveChangesAsync();
        return fitnessClass;
    }

    public async Task<Class> UnRegisterMemberFromWaitingListAsync(Guid classId, Guid memberId)
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

    public async Task<Class> DeleteClassByIdAsync(Guid id)
    {
        var fitnessClass = await GetClassByIdAsync(id)
            ?? throw new InvalidOperationException($"Class '{id}' not found");

        _context.Classes.Remove(fitnessClass);
        await _context.SaveChangesAsync();
        return fitnessClass;
    }
}