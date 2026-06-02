using GymApp.Domain.Entities;

namespace GymApp.Application.Interfaces;

public interface IWorkoutRepository
{
    Task AddAsync(Workout workout);

    Task<Workout?> GetByIdAsync(Guid workoutId, bool includeRoutine = true);

    Task<IEnumerable<Workout>> GetRoutineWorkouts(Guid routineId);

    Task<bool> ExistsByNameAsync(Guid routineId, string workoutName);
}
