using GymApp.Application.Interfaces;
using GymApp.Domain.Common;
using GymApp.Domain.Entities;
using GymApp.Application.DTOs;

namespace GymApp.Application.Services;

public class RoutineService : IRoutineService
{
    private readonly IRoutineRepository _routineRepository;

    public RoutineService(IRoutineRepository routineRepository)
    {
        _routineRepository = routineRepository;
    }

    public async Task<Result<IEnumerable<RoutineResponse>>> GetUserRoutinesAsync(Guid userId)
    {
        var routines = await _routineRepository.GetUserRoutinesAsync(userId);
            
        return Result<IEnumerable<RoutineResponse>>
            .Success(routines.Select(RoutineResponse.FromEntity));
    }

    public async Task<Result<RoutineResponse>> GetRoutineAsync(Guid routineId, Guid requesterId)
    {
        var routine = await _routineRepository.GetRoutineByIdAsync(routineId);

        if(routine is null)
            return Result<RoutineResponse>.NotFound("Ficha de treino não encontrada");

        if(routine.UserId != requesterId)
            return Result<RoutineResponse>.Forbidden("Você não pode acessar essa ficha de treino");

        return Result<RoutineResponse>
            .Success(RoutineResponse.FromEntity(routine));
    }

    public async Task<Result<RoutineResponse>> CreateRoutineAsync(RoutineRequest routineDTO, Guid userId)
    {
        var routine = new Routine(routineDTO.Name, userId);

        await _routineRepository.AddAsync(routine);
        return Result<RoutineResponse>
            .Success(RoutineResponse.FromEntity(routine));
    }
}
