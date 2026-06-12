using Halter.Application.DTOs;
using Halter.Application.Interfaces;
using Halter.Domain.Common;
using Halter.Domain.Entities;

namespace Halter.Application.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IRoutineRepository _routineRepository;
    private readonly IWorkoutExerciseRepository _workoutExerciseRepository;

    public WorkoutService(IWorkoutRepository workoutRepository, IRoutineRepository routineRepository, IWorkoutExerciseRepository workoutExerciseRepository)
    {
        _workoutRepository = workoutRepository;
        _routineRepository = routineRepository;
        _workoutExerciseRepository = workoutExerciseRepository;
    }

    public async Task<Result<WorkoutWithExercisesResponse>> GetWorkoutAsync(Guid workoutId, Guid requesterId)
    {
        var workout = await _workoutRepository.GetByIdAsync(workoutId);

        if(workout is null)
            return Result<WorkoutWithExercisesResponse>
                .NotFound();

        var queryUserId = workout.Routine.UserId;
        if(queryUserId != requesterId)
            return Result<WorkoutWithExercisesResponse>
                .Forbidden();

        var exercises = await _workoutExerciseRepository.GetWorkoutExercisesAsync(workoutId);

        return Result<WorkoutWithExercisesResponse>
            .Success(WorkoutWithExercisesResponse.FromEntity(workout, exercises));
    }

    public async Task<Result<WorkoutResponse>> CreateWorkoutAsync(WorkoutRequest requestDTO, Guid requesterId)
    {
        var routine = await _routineRepository.GetRoutineByIdAsync(requestDTO.RoutineId);

        if(routine is null)
            return Result<WorkoutResponse>.NotFound();

        if(routine.UserId != requesterId)
            return Result<WorkoutResponse>.Forbidden();

        var exists = await _workoutRepository.ExistsByNameAsync(requestDTO.RoutineId, requestDTO.Name);
        if(exists)
            return Result<WorkoutResponse>.AlreadyExists();
        
        var workout = new Workout(requestDTO.Name.Trim(), requestDTO.RoutineId);
        await _workoutRepository.AddAsync(workout);

        return Result<WorkoutResponse>
            .Success(WorkoutResponse.FromEntity(workout));
    }
}
