using Halter.Domain.Entities;

namespace Halter.Application.Interfaces;

public interface IExerciseEntryRepository
{
    Task AddAsync(ExerciseEntry exerciseEntry);
    Task<ExerciseEntry?> GetAsync(Guid entryId);
    Task<IEnumerable<Exercise>> GetTrainedExercisesAsync(Guid userId, int page, int pageSize, int? muscleGroupId, string? search);
    Task<IEnumerable<ExerciseEntry>> GetExerciseHistory(Guid userId, int exerciseId, DateTime? from, DateTime? to);
    Task<IEnumerable<ExerciseEntry>> GetAllByWorkouSessionAsync(Guid workoutSessionId);
    Task<ExerciseEntry?> GetExercisePersonalRecordAsync(int exerciseId, Guid userId);
}
