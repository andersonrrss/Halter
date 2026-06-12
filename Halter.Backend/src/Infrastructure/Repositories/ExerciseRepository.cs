using Halter.Application.Interfaces;
using Halter.Domain.Entities;
using Halter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Halter.Infrastructure.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly AppDbContext _context;

    public ExerciseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Exercise?> GetByIdAsync(int exerciseId) => 
        await _context.Exercises.FirstOrDefaultAsync(e => e.Id == exerciseId);

    public async Task<IEnumerable<Exercise>> GetAllAsync(int page, int pageSize, string? search, int? muscleGroupId)
    {
        var query = _context.Exercises
            .Include(e => e.MuscleGroup)
            .AsQueryable();

        if(!string.IsNullOrEmpty(search))
            query = query.Where(e => EF.Functions.ILike(e.Name, $"%{search}%"));

        if(muscleGroupId is not null)
            query = query.Where(e => e.MuscleGroupId == muscleGroupId);

        return await query.Skip(page * pageSize).Take(pageSize).ToListAsync();
    }
}
