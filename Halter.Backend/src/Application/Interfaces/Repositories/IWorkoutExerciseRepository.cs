using GymApp.Domain.Entities;

namespace GymApp.Application.Interfaces;

public interface IWorkoutExerciseRepository
{
    Task AddAsync(WorkoutExercise workoutExercise);
    Task<WorkoutExercise?> GetByIdAsync(Guid workoutExerciseId);
    Task<bool> IsInWorkout(Guid workoutId, int exerciseId);
    Task<IEnumerable<WorkoutExercise>> GetWorkoutExercisesAsync(Guid workoutId);
}
