using GymApp.Application.DTOs;
using GymApp.Domain.Common;

namespace GymApp.Application.Interfaces;

public interface IWorkoutSessionService
{
    Task<Result<FinishedWorkoutSessionResponse>> GetAsync(Guid workoutSessionId, Guid userId);
    Task<Result<IEnumerable<FinishedWorkoutSessionResponse>>> GetAllByUserAsync(Guid userId, int page, int pageSize, Guid? workoutId);
    Task<Result<UnfinishedWorkoutSessionResponse>> StartSessionAsync(Guid workoutId, Guid userId);
    Task<Result<FinishedWorkoutSessionResponse>> FinishSessionAsync(Guid workoutId, Guid userId);
    Task<Result<UnfinishedWorkoutSessionResponse>> PauseSessionAsync(Guid workoutId, Guid userId);
}
