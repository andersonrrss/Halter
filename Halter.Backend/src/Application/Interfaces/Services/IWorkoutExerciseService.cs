using GymApp.Domain.Common;
using GymApp.Application.DTOs;

namespace GymApp.Application.Interfaces;

public interface IWorkoutExerciseService
{
    Task<Result<WorkoutExerciseResponse>> CreateWorkoutExerciseAsync(Guid workoutId, WorkoutExerciseRequest requestDTO, Guid userId);
}
