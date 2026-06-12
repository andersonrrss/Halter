using Halter.Application.DTOs;
using Halter.Application.Interfaces;
using Halter.Domain.Common;
using Halter.Domain.Entities;

namespace Halter.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IHashPasswordService _hashPasswordService;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IHashPasswordService hashPasswordService, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _hashPasswordService = hashPasswordService;
        _jwtService = jwtService;
    }

    public async Task<Result<string>> TryLoginUserAsync(LoginRequest loginDTO)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginDTO.Email.Trim());

        if(user is null)
            return Result<string>.Invalid("Email");

        if(!await _hashPasswordService.VerifyAsync(loginDTO.Password.Trim(), user.PasswordHash))
            return Result<string>.Invalid("Password");

        var token = _jwtService.Generate(user);

        return Result<string>.Success(token);
    }

    public async Task<Result<string>> TryRegisterUserAsync(RegisterRequest registerDTO)
    {
        var name = registerDTO.Name.Trim();
        var email = registerDTO.Email.Trim();
        var password = registerDTO.Password.Trim();
        
        if(await _userRepository.GetUserByEmailAsync(email) is not null)
            return Result<string>.AlreadyExists("Email");
        
        var hashedPassword = await _hashPasswordService.HashAsync(password);

        var user = new User(name, email, hashedPassword);

        await _userRepository.AddAsync(user);

        var token = _jwtService.Generate(user);

        return Result<string>.Success(token);
    }
}
