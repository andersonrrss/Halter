using GymApp.Application.DTOs;
using GymApp.Domain.Common;

namespace GymApp.Application.Interfaces;

public interface IWorkoutService
{
    Task<Result<WorkoutWithExercisesResponse>> GetWorkoutAsync(Guid workoutId, Guid requesterId);

    Task<Result<WorkoutResponse>> CreateWorkoutAsync(WorkoutRequest workoutDto, Guid requesterId);
}
