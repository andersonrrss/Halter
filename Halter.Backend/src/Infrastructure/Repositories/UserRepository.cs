using GymApp.Application.Interfaces;
using GymApp.Domain.Entities;
using GymApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetUserByIdAsync(Guid id) => 
        await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetUserByEmailAsync(string email) => 
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
