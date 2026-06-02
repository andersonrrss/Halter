using GymApp.Application.Interfaces;
using GymApp.Domain.Entities;
using GymApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Infrastructure.Repositories;

public class WorkoutExerciseRepository : IWorkoutExerciseRepository
{
    private readonly AppDbContext _context;

    public WorkoutExerciseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WorkoutExercise?> GetByIdAsync(Guid workoutExerciseId) =>
        await _context.WorkoutExercises
            .Include(we => we.Exercise)
            .FirstOrDefaultAsync(we => we.Id == workoutExerciseId);
    
    public async Task<IEnumerable<WorkoutExercise>> GetWorkoutExercisesAsync(Guid workoutId) =>
        await _context.WorkoutExercises
            .Include(we => we.Exercise)
            .Where(we => we.WorkoutId == workoutId)
            .ToListAsync();

    public async Task<bool> IsInWorkout(Guid workoutId, int exerciseId) =>
        await _context.WorkoutExercises.FirstOrDefaultAsync(
            we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId) is not null;

    public async Task AddAsync(WorkoutExercise workoutExercise)
    {
        await _context.WorkoutExercises.AddAsync(workoutExercise);
        await _context.SaveChangesAsync();
    }
}
