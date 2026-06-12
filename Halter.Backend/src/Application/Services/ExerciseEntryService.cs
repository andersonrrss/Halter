using Halter.Application.DTOs;
using Halter.Application.Interfaces;
using Halter.Domain.Common;
using Halter.Domain.Entities;
using Halter.Domain.Enums;

namespace Halter.Application.Services;

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
            return Result<ExerciseEntryResponse>.NotFound();

        if(workoutSession.UserId != userId)
            return Result<ExerciseEntryResponse>.Forbidden();
        
        if(workoutSession.PausedAt is not null)
            return Result<ExerciseEntryResponse>.InvalidState();
        if(workoutSession.FinishedAt is not null)
            return Result<ExerciseEntryResponse>.InvalidState();

        var workoutExercise = await _workoutExerciseRepository.GetByIdAsync(request.WorkoutExerciseId);

        if(workoutExercise is null)
            return Result<ExerciseEntryResponse>.NotFound();
        
        if(workoutSession.ExercisesEntries.Any(ee => ee.ExerciseId == workoutExercise.ExerciseId))
            return Result<ExerciseEntryResponse>.AlreadyExists();
                    
        if(request.SetsCompleted <= 0)
            return Result<ExerciseEntryResponse>.OutOfRange("SetsCompleted");

        var minValue = workoutExercise.ExerciseType == ExerciseType.Time ? 10 : 0;

        if(request.ValueAchieved <= minValue)
            return Result<ExerciseEntryResponse>.OutOfRange("ValueAchieved");
        
        if(request.MaxWeight < 0)
            return Result<ExerciseEntryResponse>.OutOfRange("MaxWeight");

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
                    .OutOfRange();
        }
    
        if(to is not null && from is null)
            return Result<ExerciseProgressResponse>
                .Required("from");

        var history = await _exerciseEntryRepository.GetExerciseHistory(userId, exerciseId, from, to);

        if(!history.Any())
            return Result<ExerciseProgressResponse>.Empty();

        var personalRecord = await _exerciseEntryRepository.GetExercisePersonalRecordAsync(exerciseId, userId);

        return Result<ExerciseProgressResponse>.Success(
            ExerciseProgressResponse.FromEntity(personalRecord!, history)
        );
    }
}
