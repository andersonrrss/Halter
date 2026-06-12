using Halter.Domain.Entities;

namespace Halter.Application.Interfaces;

public interface IWorkoutSessionRepository
{
    Task<WorkoutSession?> GetLatestFromUserAsync(Guid userId);

    Task<WorkoutSession?> GetLatestFromWorkoutAsync(Guid workoutId, bool includeExercises = false);

    Task<IEnumerable<WorkoutSession>> GetAllByUserAsync(int page, int pageSize, Guid userId, Guid? workoutId);

    Task<WorkoutSession?> GetByIdAsync(Guid workoutSessionId);

    Task AddAsync(WorkoutSession workoutSession);

    Task UpdateAsync(WorkoutSession workoutSession);

    Task DeleteAsync(WorkoutSession workoutSession);
}
