using GymApp.Application.Interfaces;
using GymApp.Domain.Entities;
using GymApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Infrastructure.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly AppDbContext _context;

    public WorkoutRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Workout workout)
    {
        await _context.Workouts.AddAsync(workout);
        await _context.SaveChangesAsync();
    }

    public async Task<Workout?> GetByIdAsync(Guid workoutId, bool includeExercises = false)
    {
        var query = _context.Workouts.AsQueryable();

        if(includeExercises)
            query = query.Include(w => w.Exercises);
        query = query.Include(w => w.Routine);
        
        return await query.FirstOrDefaultAsync(w => w.Id == workoutId);
    }

    public async Task<IEnumerable<Workout>> GetRoutineWorkouts(Guid routineId) =>
        await _context.Workouts
            .Where(w => w.RoutineId == routineId)
            .ToListAsync();

    public async Task<bool> ExistsByNameAsync(Guid routineId, string workoutName) =>
        await _context.Workouts.AnyAsync(w => 
            w.RoutineId == routineId &&
            EF.Functions.ILike(w.Name, workoutName.Trim())
        );
}
