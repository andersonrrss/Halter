using System.Security.Cryptography.X509Certificates;
using Halter.Application.Interfaces;
using Halter.Domain.Entities;
using Halter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Halter.Infrastructure.Repositories;

public class ExerciseEntryRepository : IExerciseEntryRepository
{
    private readonly AppDbContext _context;

    public ExerciseEntryRepository(AppDbContext context) =>
        _context = context;

    public async Task AddAsync(ExerciseEntry exerciseEntry)
    {
        await _context.ExerciseEntries.AddAsync(exerciseEntry);
        await _context.SaveChangesAsync();
    }

    public async Task<ExerciseEntry?> GetAsync(Guid entryId) =>
        await _context.ExerciseEntries.FirstOrDefaultAsync(ee => ee.Id == entryId);

    public async Task<IEnumerable<Exercise>> GetTrainedExercisesAsync(Guid userId, int page, int pageSize, int? muscleGroupId, string? search){
        var query = _context.ExerciseEntries
            .Where(ee => ee.WorkoutSession.UserId == userId)
            .Select(ee => ee.Exercise)
            .Distinct();
        
        if(!string.IsNullOrEmpty(search))
            query = query.Where(e => EF.Functions.ILike(e.Name, $"%{search}%"));

        if(muscleGroupId is not null)
            query = query.Where(e => e.MuscleGroupId == muscleGroupId);

        return await query
            .Include(e => e.MuscleGroup)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<ExerciseEntry>> GetExerciseHistory(Guid userId, int exerciseId, DateTime? from, DateTime? to)
    {
        var query = _context.ExerciseEntries
            .Where(ee => 
                ee.ExerciseId == exerciseId && ee.WorkoutSession.UserId == userId)
            .OrderByDescending(ee => ee.CompletedAt)
            .AsQueryable();

        int limit = 200;

        if(from is not null && to is not null)
        {
            query = query.Where(ee => 
                ee.CompletedAt >= from && 
                ee.CompletedAt <= to);
            limit = 500;
        }

        return await query.Take(limit).ToListAsync();
    }
    
    public async Task<IEnumerable<ExerciseEntry>> GetAllByWorkouSessionAsync(Guid workoutSessionId) =>
        await _context.ExerciseEntries
            .Where(ee => ee.WorkoutSessionId == workoutSessionId)
            .ToListAsync();

    public async Task<ExerciseEntry?> GetExercisePersonalRecordAsync(int exerciseId, Guid userId) =>
        await _context.ExerciseEntries
            .Where(ee => 
                ee.ExerciseId == exerciseId && ee.WorkoutSession.UserId == userId)
            .OrderByDescending(ee => ee.MaxWeight)
            .ThenByDescending(ee => ee.ValueAchieved)
            .ThenByDescending(ee => ee.SetsCompleted)
            .FirstOrDefaultAsync();
}
