using GymApp.Domain.Entities;

namespace GymApp.Application.Interfaces;

public interface IJwtService
{
    string Generate(User user);
}
