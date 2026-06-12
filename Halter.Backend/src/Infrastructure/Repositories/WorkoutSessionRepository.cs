using Halter.Application.Interfaces;
using Halter.Domain.Entities;
using Halter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Halter.Infrastructure.Repositories;

public class WorkoutSessionRepository : IWorkoutSessionRepository
{
    private readonly AppDbContext _context;

    public WorkoutSessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WorkoutSession?> GetLatestFromUserAsync(Guid userId) =>
        await _context.WorkoutSessions
            .Where(ws => ws.UserId == userId)
            .OrderByDescending(ws => ws.StartedAt)
            .FirstOrDefaultAsync();

    public async Task<WorkoutSession?> GetLatestFromWorkoutAsync(Guid workoutId, bool includeExercises = false)
    {
        var query = _context.WorkoutSessions.AsQueryable();

        if(includeExercises)
            query = query
                .Include(ws => ws.ExercisesEntries)
                .ThenInclude(ee => ee.Exercise);

        return await query
            .Where(ws => ws.WorkoutId == workoutId)
            .OrderByDescending(ws => ws.StartedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<WorkoutSession>> GetAllByUserAsync(int page, int pageSize, Guid userId, Guid? workoutId)
    {
        
        var query = _context.WorkoutSessions
            .Skip(page * pageSize)
            .Take(pageSize)
            .Where(ws => ws.UserId == userId)
            .AsQueryable();

        if(workoutId is not null)
            query = query.Where(ws => ws.WorkoutId == workoutId);

        return await query
            .OrderByDescending(ws => ws.StartedAt)
            .ToListAsync();
    }
    

    public async Task<WorkoutSession?> GetByIdAsync(Guid workoutSessionId) => 
        await _context.WorkoutSessions
            .Include(ws => ws.ExercisesEntries)
            .ThenInclude(ee => ee.Exercise)
            .FirstOrDefaultAsync(ws => ws.Id == workoutSessionId);

    public async Task AddAsync(WorkoutSession workoutSession)
    {
        await _context.WorkoutSessions.AddAsync(workoutSession);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(WorkoutSession workoutSession)
    {
        _context.WorkoutSessions.Update(workoutSession);
        await _context.SaveChangesAsync();
    } 

    public async Task DeleteAsync(WorkoutSession workoutSession)
    {
        _context.WorkoutSessions.Remove(workoutSession);
        await _context.SaveChangesAsync();
    }
}
