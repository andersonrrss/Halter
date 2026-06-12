using Halter.Domain.Entities;

namespace Halter.Application.Interfaces;

public interface IRoutineRepository
{
    Task AddAsync(Routine routine);

    Task<IEnumerable<Routine>> GetUserRoutinesAsync(Guid userId);

    Task<Routine?> GetRoutineByIdAsync(Guid routineId);
}
