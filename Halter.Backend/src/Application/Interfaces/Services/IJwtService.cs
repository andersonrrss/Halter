using Halter.Domain.Entities;

namespace Halter.Application.Interfaces;

public interface IJwtService
{
    string Generate(User user);
}
