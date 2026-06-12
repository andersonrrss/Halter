using Halter.Application.DTOs;
using Halter.Domain.Common;

namespace Halter.Application.Interfaces;

public interface IExerciseService
{
    Task<Result<IEnumerable<ExerciseResponse>>> GetExercisesAsync(int page, int pageSize, int? muscleGroupId, string? search);
}
