using Halter.Application.DTOs;
using Halter.Application.Interfaces;
using Halter.Domain.Common;
using Halter.Domain.Entities;

namespace Halter.Application.Services;

public class WorkoutSessionService : IWorkoutSessionService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
    private readonly IWorkoutSessionRepository _workoutSessionRepository;


    public WorkoutSessionService(
        IWorkoutRepository workoutRepository,
        IWorkoutExerciseRepository workoutExerciseRepository,
        IWorkoutSessionRepository workoutSessionRepository
    )
    {
        _workoutRepository = workoutRepository;
        _workoutExerciseRepository = workoutExerciseRepository;
        _workoutSessionRepository = workoutSessionRepository;
    }

    public async Task<Result<FinishedWorkoutSessionResponse>> GetAsync(Guid workoutSessionId, Guid userId)
    {
        var workoutSession = await _workoutSessionRepository.GetByIdAsync(workoutSessionId);

        if(workoutSession is null)
            return Result<FinishedWorkoutSessionResponse>
                .NotFound();
        
        if(workoutSession.UserId != userId)
            return Result<FinishedWorkoutSessionResponse>
                .Forbidden();

        return Result<FinishedWorkoutSessionResponse>
            .Success(FinishedWorkoutSessionResponse.FromEntity(workoutSession));
    }

    public async Task<Result<IEnumerable<FinishedWorkoutSessionResponse>>> GetAllByUserAsync(Guid userId,int page,int pageSize, Guid? workoutId)
    {
        var workoutSessionsList = await _workoutSessionRepository.GetAllByUserAsync(page, pageSize, userId, workoutId);

        return Result<IEnumerable<FinishedWorkoutSessionResponse>>.Success(
            workoutSessionsList.Select(FinishedWorkoutSessionResponse.FromEntity)
        );
    }


    public async Task<Result<UnfinishedWorkoutSessionResponse>> StartSessionAsync(Guid workoutId, Guid userId)
    {
        var result = await AuthenticateWorkoutDataAsync(workoutId, userId);
        if(!result.IsSuccess)
            return Result<UnfinishedWorkoutSessionResponse>
                .Failure(result.ErrorCode!);

        var exercises = await _workoutExerciseRepository.GetWorkoutExercisesAsync(workoutId);
        if(!exercises.Any())
            return Result<UnfinishedWorkoutSessionResponse>.Empty();

        var lastestSession = await _workoutSessionRepository.GetLatestFromUserAsync(userId);
        if(lastestSession is null )
            return Result<UnfinishedWorkoutSessionResponse>.Empty();

        if(lastestSession.FinishedAt is null)
            return Result<UnfinishedWorkoutSessionResponse>.Failure(ErrorCodes.SessionAlreadyActive);

        var workoutSession = new WorkoutSession(workoutId, userId);
        await _workoutSessionRepository.AddAsync(workoutSession);

        return Result<UnfinishedWorkoutSessionResponse>.Success(
            UnfinishedWorkoutSessionResponse.FromEntity(workoutSession)
        );
    }

    public async Task<Result<UnfinishedWorkoutSessionResponse>> PauseSessionAsync(Guid workoutId, Guid userId)
    {
        var result = await AuthenticateWorkoutDataAsync(workoutId, userId);
        if(!result.IsSuccess)
            return Result<UnfinishedWorkoutSessionResponse>
                .Failure(result.ErrorCode!);

        var lastestSession = await _workoutSessionRepository.GetLatestFromWorkoutAsync(workoutId, true);
        if(lastestSession is null || lastestSession.FinishedAt is not null)
            return Result<UnfinishedWorkoutSessionResponse>.Failure(ErrorCodes.NoActiveSession);

        lastestSession.TogglePause();
        await _workoutSessionRepository.UpdateAsync(lastestSession);
        return Result<UnfinishedWorkoutSessionResponse>
            .Success(UnfinishedWorkoutSessionResponse.FromEntity(lastestSession));
    }

    public async Task<Result<FinishedWorkoutSessionResponse>> FinishSessionAsync(Guid workoutId, Guid userId)
    {
        var result = await AuthenticateWorkoutDataAsync(workoutId, userId);

        if(!result.IsSuccess)
            return Result<FinishedWorkoutSessionResponse>
                .Failure(result.ErrorCode!);
        
        var latestSession = await _workoutSessionRepository.GetLatestFromWorkoutAsync(workoutId, true);

        if(latestSession is null || latestSession.FinishedAt is not null)
            return Result<FinishedWorkoutSessionResponse>.Failure(ErrorCodes.NoActiveSession);

        if(!latestSession.ExercisesEntries.Any())
        {
            await _workoutSessionRepository.DeleteAsync(latestSession);
            return Result<FinishedWorkoutSessionResponse>
                .Empty("ExercisesEntries");
        }

        latestSession.FinishSession();
        await _workoutSessionRepository.UpdateAsync(latestSession);

        return Result<FinishedWorkoutSessionResponse>
            .Success(FinishedWorkoutSessionResponse.FromEntity(latestSession));
    }

    private async Task<Result> AuthenticateWorkoutDataAsync(Guid workoutId, Guid userId)
    {
        var workout = await _workoutRepository.GetByIdAsync(workoutId);

        if(workout is null)
            return Result.NotFound();

        if(workout.Routine.UserId != userId)
            return Result.Forbidden();

        return Result.Success();
    }
}
