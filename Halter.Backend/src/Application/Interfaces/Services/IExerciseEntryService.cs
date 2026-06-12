using Halter.Application.DTOs;
using Halter.Domain.Common;

namespace Halter.Application.Interfaces;

public interface IExerciseEntryService
{
    Task<Result<ExerciseEntryResponse>> AddAsync(ExerciseEntryRequest request, Guid userId, Guid workoutId);
    Task<Result<IEnumerable<ExerciseSimpleResponse>>> GetTrainedExercisesAsync(
        Guid userId, 
        int page,
        int pageSize,
        int? muscleGroupId,
        string? search
    );

    Task<Result<ExerciseProgressResponse>> GetExerciseProgress(
        int exerciseId, 
        Guid userId, 
        DateTime? from, 
        DateTime? to
    );
}
