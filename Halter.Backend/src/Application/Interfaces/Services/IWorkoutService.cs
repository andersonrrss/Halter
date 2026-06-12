using Halter.Application.DTOs;
using Halter.Domain.Common;

namespace Halter.Application.Interfaces;

public interface IWorkoutService
{
    Task<Result<WorkoutWithExercisesResponse>> GetWorkoutAsync(Guid workoutId, Guid requesterId);

    Task<Result<WorkoutResponse>> CreateWorkoutAsync(WorkoutRequest workoutDto, Guid requesterId);
}
