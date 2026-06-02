using GymApp.Application.DTOs;
using GymApp.Application.Interfaces;
using GymApp.Domain.Common;
using GymApp.Domain.Entities;

namespace GymApp.Application.Services;

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
            return Result<WorkoutExerciseResponse>.NotFound("O Treino especificado não existe"); 
        if(workout.Routine.UserId != userId) 
            return Result<WorkoutExerciseResponse>.Forbidden(); 
        
        if(workout.Exercises.Count >= 50)
            return Result<WorkoutExerciseResponse>.BusinessFailure("O treino não pode conter mais que 50 exercícios");

        var isInWorkout = await _workoutExerciseRepository.IsInWorkout(workoutId, requestDTO.ExerciseId);
        if(isInWorkout)
            return Result<WorkoutExerciseResponse>.BusinessFailure("Exercício já está no treino");

        var exercise = await _exerciseRepository.GetByIdAsync(requestDTO.ExerciseId); 
        if(exercise is null) 
            return Result<WorkoutExerciseResponse>.NotFound("Exercício não existe");

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