using Halter.Domain.Common;
using Halter.Application.DTOs;

namespace Halter.Application.Interfaces;

public interface IWorkoutExerciseService
{
    Task<Result<WorkoutExerciseResponse>> CreateWorkoutExerciseAsync(Guid workoutId, WorkoutExerciseRequest requestDTO, Guid userId);
}
