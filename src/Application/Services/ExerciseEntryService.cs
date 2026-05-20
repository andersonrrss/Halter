using GymApp.Application.DTOs;
using GymApp.Application.Interfaces;
using GymApp.Domain.Common;
using GymApp.Domain.Entities;
using GymApp.Domain.Enums;

namespace GymApp.Application.Services;

public class ExerciseEntryService : IExerciseEntryService
{
    private readonly IExerciseEntryRepository _exerciseEntryRepository;
    private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
    private readonly IWorkoutSessionRepository _workoutSessionRepository;

    public ExerciseEntryService(
        IExerciseEntryRepository exerciseEntryRepository,
        IWorkoutExerciseRepository workoutExerciseRepository,
        IWorkoutSessionRepository workoutSessionRepository
    )
    {
        _exerciseEntryRepository = exerciseEntryRepository;
        _workoutExerciseRepository = workoutExerciseRepository;
        _workoutSessionRepository = workoutSessionRepository;
    }

    public async Task<Result<ExerciseEntryResponse>> AddAsync(
        ExerciseEntryRequest request, 
        Guid userId, 
        Guid workoutId)
    {
        var workoutSession = await _workoutSessionRepository.GetLatestFromWorkoutAsync(workoutId);

        if(
            workoutSession is null || 
            workoutSession.WorkoutId != workoutId
        )
            return Result<ExerciseEntryResponse>.NotFound("Sessão de treino não existe");

        if(workoutSession.UserId != userId)
            return Result<ExerciseEntryResponse>.Forbidden("Você não pode acessar isso");
        
        if(workoutSession.PausedAt is not null)
            return Result<ExerciseEntryResponse>.BusinessFailure("Sessão de treino pausada");
        if(workoutSession.FinishedAt is not null)
            return Result<ExerciseEntryResponse>.BusinessFailure("Sessão de treino já finalizada");

        var workoutExercise = await _workoutExerciseRepository.GetByIdAsync(request.WorkoutExerciseId);

        if(workoutExercise is null)
            return Result<ExerciseEntryResponse>.NotFound("Exercício inexistente");
        
        if(workoutSession.ExerciseEntries.Any(ee => ee.ExerciseId == workoutExercise.ExerciseId))
            return Result<ExerciseEntryResponse>.BusinessFailure("Exercício já registrado na sessão de treino");
                    
        if(request.SetsCompleted <= 0)
            return Result<ExerciseEntryResponse>.BusinessFailure("É necessário pelo menos uma série completa");

        var (minValue, errorMessage) = workoutExercise.ExerciseType == ExerciseType.Time 
            ? (10, "É necessário pelo menos 10 segundos de exercício")
            : (0, "É preciso pelo menos uma repetição");

        if(request.ValueAchieved <= minValue)
            return Result<ExerciseEntryResponse>.BusinessFailure(errorMessage);
        
        if(request.MaxWeight < 0)
            return Result<ExerciseEntryResponse>.BusinessFailure("Um exercício não pode ter peso negativo");

        var exerciseEntry = new ExerciseEntry(
            workoutExercise.ExerciseId,
            workoutSession.Id,
            request.ValueAchieved,
            request.SetsCompleted,
            workoutExercise.ExerciseType,
            request.MaxWeight
        );
        
        await _exerciseEntryRepository.AddAsync(exerciseEntry);

        return Result<ExerciseEntryResponse>.Success(
            ExerciseEntryResponse.FromEntity(exerciseEntry, workoutExercise.Exercise)
        );
    }

    public async Task<Result<IEnumerable<ExerciseSimpleResponse>>> GetTrainedExercisesAsync(
        Guid userId, 
        int page,
        int pageSize,
        int? muscleGroupId,
        string? search
    )
    {
        var exercises = await _exerciseEntryRepository.GetTrainedExercisesAsync(userId, page, pageSize, muscleGroupId, search);
        return Result<IEnumerable<ExerciseSimpleResponse>>
            .Success(exercises.Select(ExerciseSimpleResponse.FromEntity));
    }

    public async Task<Result<ExerciseProgressResponse>> GetExerciseProgress(
        int exerciseId, 
        Guid userId, 
        DateTime? from, 
        DateTime? to
    )
    {
        if(from is not null)
        {
            to ??= DateTime.UtcNow;

            if((to - from).Value.TotalDays is > 366 or < 0)
                return Result<ExerciseProgressResponse>
                    .BusinessFailure("O intevalo máximo permitido é de 1 ano");
        }
    
        if(to is not null && from is null)
            return Result<ExerciseProgressResponse>
                .BusinessFailure("É necessário informar a data de início do intervalo");

        var history = await _exerciseEntryRepository.GetExerciseHistory(userId, exerciseId, from, to);

        if(!history.Any())
            return Result<ExerciseProgressResponse>.NotFound("Nenhum histórico encontrado para esse exercício");

        var personalRecord = await _exerciseEntryRepository.GetExercisePersonalRecordAsync(exerciseId, userId);

        return Result<ExerciseProgressResponse>.Success(
            ExerciseProgressResponse.FromEntity(personalRecord!, history)
        );
    }
}
