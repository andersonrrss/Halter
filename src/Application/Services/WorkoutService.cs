using GymApp.Application.DTOs;
using GymApp.Application.Interfaces;
using GymApp.Domain.Common;
using GymApp.Domain.Entities;

namespace GymApp.Application.Services;

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
                .NotFound("Treino não encontrado");

        var queryUserId = workout.Routine.UserId;
        if(queryUserId != requesterId)
            return Result<WorkoutWithExercisesResponse>
                .Forbidden("Você não pode acessar treinos de outros usuários");

        var exercises = await _workoutExerciseRepository.GetWorkoutExercisesAsync(workoutId);

        return Result<WorkoutWithExercisesResponse>
            .Success(WorkoutWithExercisesResponse.FromEntity(workout, exercises));
    }

    public async Task<Result<WorkoutResponse>> CreateWorkoutAsync(WorkoutRequest requestDTO, Guid requesterId)
    {
        var routine = await _routineRepository.GetRoutineByIdAsync(requestDTO.RoutineId);

        if(routine is null)
            return Result<WorkoutResponse>.NotFound("Ficha de treino inexistente");

        if(routine.UserId != requesterId)
            return Result<WorkoutResponse>.Forbidden("Você não pode editar esta ficha");

        var exists = await _workoutRepository.ExistsByNameAsync(requestDTO.RoutineId, requestDTO.Name);
        if(exists)
            return Result<WorkoutResponse>.BusinessFailure("Nome de treino já existente");
        
        var workout = new Workout(requestDTO.Name.Trim(), requestDTO.RoutineId);
        await _workoutRepository.AddAsync(workout);

        return Result<WorkoutResponse>
            .Success(WorkoutResponse.FromEntity(workout));
    }
}
