using System.ComponentModel.DataAnnotations;
using GymApp.Domain.Common;
using GymApp.Application.DTOs;

namespace GymApp.Application.Interfaces;

public interface IRoutineService
{
    Task<Result<IEnumerable<RoutineResponse>>> GetUserRoutinesAsync(Guid userId);

    Task<Result<RoutineResponse>> GetRoutineAsync(Guid routineId, Guid requesterId);

    Task<Result<RoutineResponse>> CreateRoutineAsync(RoutineRequest routineDTO, Guid userId);
}
