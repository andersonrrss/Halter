using GymApp.Application.DTOs;
using GymApp.Application.Interfaces;
using GymApp.Domain.Common;
using GymApp.Domain.Entities;

namespace GymApp.Application.Services;

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
                .NotFound("Treino não encontrado");
        
        if(workoutSession.UserId != userId)
            return Result<FinishedWorkoutSessionResponse>
                .Forbidden("Você não pode acessar esse treino");

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
                .Failure(result.Error!, result.ErrorType);

        var exercises = await _workoutExerciseRepository.GetWorkoutExercisesAsync(workoutId);
        if(!exercises.Any())
            return Result<UnfinishedWorkoutSessionResponse>
                .BusinessFailure("Esse treino não contém nenhum exercício... Sessão não iniciada");

        var lastestSession = await _workoutSessionRepository.GetLatestFromUserAsync(userId);
        if(lastestSession is null )
            return Result<UnfinishedWorkoutSessionResponse>.NotFound("Esse treino não tem sessões");

        if(lastestSession.FinishedAt is null)
            return Result<UnfinishedWorkoutSessionResponse>.BusinessFailure("Você ainda tem um treino não finalizado");

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
                .Failure(result.Error!, result.ErrorType);

        var lastestSession = await _workoutSessionRepository.GetLatestFromWorkoutAsync(workoutId, true);
        if(lastestSession is null || lastestSession.FinishedAt is not null)
            return Result<UnfinishedWorkoutSessionResponse>.NotFound("Esse treino não tem sessões em andamento");

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
                .Failure(result.Error!, result.ErrorType);
        
        var latestSession = await _workoutSessionRepository.GetLatestFromWorkoutAsync(workoutId, true);

        if(latestSession is null)
            return Result<FinishedWorkoutSessionResponse>.NotFound("Esse treino não tem nenhuma sessão iniciada");

        if(!latestSession.ExerciseEntries.Any())
        {
            await _workoutSessionRepository.DeleteAsync(latestSession);
            return Result<FinishedWorkoutSessionResponse>
                .BusinessFailure("Dados não salvos, nenhum exercício feito durante o treino");
        }

        if(latestSession.FinishedAt is not null)
            return Result<FinishedWorkoutSessionResponse>
                .BusinessFailure("A ultima sessão desse treino já foi finalizada");

        latestSession.FinishSession();
        await _workoutSessionRepository.UpdateAsync(latestSession);

        return Result<FinishedWorkoutSessionResponse>
            .Success(FinishedWorkoutSessionResponse.FromEntity(latestSession));
    }

    private async Task<Result> AuthenticateWorkoutDataAsync(Guid workoutId, Guid userId)
    {
        var workout = await _workoutRepository.GetByIdAsync(workoutId);

        if(workout is null)
            return Result.NotFound("Treino não encontrado");

        if(workout.Routine.UserId != userId)
            return Result.Forbidden();

        return Result.Success();
    }
}
