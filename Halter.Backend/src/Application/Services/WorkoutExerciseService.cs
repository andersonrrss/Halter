using Halter.Application.DTOs;
using Halter.Application.Interfaces;
using Halter.Domain.Common;
using Halter.Domain.Entities;

namespace Halter.Application.Services;

public class WorkoutExerciseService : IWorkoutExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
    private readonly IWorkoutRepository _workoutRepository;

    public WorkoutExerciseService(
        IExerciseRepository exerciseRepository, 
        IWorkoutExerciseRepository workoutExerciseRepository,
        IWorkoutRepository workoutRepository
        )
    {
        _exerciseRepository = exerciseRepository;
        _workoutExerciseRepository = workoutExerciseRepository;
        _workoutRepository = workoutRepository;
    }

    public async Task<Result<WorkoutExerciseResponse>> CreateWorkoutExerciseAsync(Guid workoutId, WorkoutExerciseRequest requestDTO, Guid userId)
    {
        var workout = await _workoutRepository.GetByIdAsync(workoutId, true);
        if(workout is null) 
            return Result<WorkoutExerciseResponse>.NotFound(); 
        if(workout.Routine.UserId != userId) 
            return Result<WorkoutExerciseResponse>.Forbidden(); 
        
        if(workout.Exercises.Count >= 50)
            return Result<WorkoutExerciseResponse>.OutOfRange("Exercises");


        var exercise = await _exerciseRepository.GetByIdAsync(requestDTO.ExerciseId); 
        if(exercise is null) 
            return Result<WorkoutExerciseResponse>.NotFound();
            
        var isInWorkout = await _workoutExerciseRepository.IsInWorkout(workoutId, requestDTO.ExerciseId);
        if(isInWorkout)
            return Result<WorkoutExerciseResponse>.AlreadyExists();

        var result = WorkoutExercise.Create(
            requestDTO.ExerciseId, workoutId, 
            requestDTO.Sets, requestDTO.Values, 
            requestDTO.ExerciseType, exercise.TimeConstraint, 
            requestDTO.IsometricHoldSeconds
        );

        if(!result.IsSuccess)
            return Result<WorkoutExerciseResponse>.FieldFailure(result.FieldErrors!);

        await _workoutExerciseRepository.AddAsync(result.Value!);
        return Result<WorkoutExerciseResponse>.Success(
            WorkoutExerciseResponse.FromEntity(result.Value!, exercise.Name
        ));
    }
}