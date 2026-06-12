using System.ComponentModel.DataAnnotations;
using Halter.Domain.Common;
using Halter.Application.DTOs;

namespace Halter.Application.Interfaces;

public interface IRoutineService
{
    Task<Result<IEnumerable<RoutineResponse>>> GetUserRoutinesAsync(Guid userId);

    Task<Result<RoutineResponse>> GetRoutineAsync(Guid routineId, Guid requesterId);

    Task<Result<RoutineResponse>> CreateRoutineAsync(RoutineRequest routineDTO, Guid userId);
}
