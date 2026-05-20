using GymApp.Application.DTOs;
using GymApp.Application.Interfaces;
using GymApp.Domain.Common;
using GymApp.Domain.Entities;

namespace GymApp.Application.Services;

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
            return Result<string>.FieldFailure("Email", "Email não registrado");

        if(!await _hashPasswordService.VerifyAsync(loginDTO.Password.Trim(), user.PasswordHash))
            return Result<string>.FieldFailure("Password", "Senha incorreta");

        var token = _jwtService.Generate(user);

        return Result<string>.Success(token);
    }

    public async Task<Result<string>> TryRegisterUserAsync(RegisterRequest registerDTO)
    {
        var name = registerDTO.Name.Trim();
        var email = registerDTO.Email.Trim();
        var password = registerDTO.Password.Trim();
        
        if(await _userRepository.GetUserByEmailAsync(email) is not null)
            return Result<string>.FieldFailure("Email", "Email já registrado");
        
        var hashedPassword = await _hashPasswordService.HashAsync(password);

        var user = new User(name, email, hashedPassword);

        await _userRepository.AddAsync(user);

        var token = _jwtService.Generate(user);

        return Result<string>.Success(token);
    }
}
