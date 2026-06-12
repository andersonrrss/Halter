using Halter.Domain.Entities;

namespace Halter.Application.Interfaces;

public interface IExerciseRepository
{
    Task<Exercise?> GetByIdAsync(int exerciseId);

    Task<IEnumerable<Exercise>> GetAllAsync(int page, int pageSize, string? search, int? muscleGroupId);
}
