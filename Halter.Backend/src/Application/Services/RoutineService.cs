using Halter.Application.Interfaces;
using Halter.Domain.Common;
using Halter.Domain.Entities;
using Halter.Application.DTOs;

namespace Halter.Application.Services;

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
            return Result<RoutineResponse>.NotFound();

        if(routine.UserId != requesterId)
            return Result<RoutineResponse>.Forbidden();

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
