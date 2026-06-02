using GymApp.Application.Interfaces;
using GymApp.Domain.Entities;
using GymApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Infrastructure.Repositories;

public class RoutineRepository : IRoutineRepository
{
    private readonly AppDbContext _context;

    public RoutineRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Routine routine)
    {
        await _context.Routines.AddAsync(routine);
        await _context.SaveChangesAsync();
    }

    public async Task<Routine?> GetRoutineByIdAsync(Guid routineId) =>
        await _context.Routines
            .Include(r => r.Workouts)
            .FirstOrDefaultAsync(r => r.Id == routineId);

    public async Task<IEnumerable<Routine>> GetUserRoutinesAsync(Guid userId) =>
        await _context.Routines
            .Where(r => r.UserId == userId)
            .ToListAsync();
}
