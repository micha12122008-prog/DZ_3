using PowerUp.Application.Services.Auth;
using PowerUp.Domain.Abstarctions.Repositories;
using PowerUp.Domain.Enums;
using PowerUp.Domain.Models.Users;
using PowerUp.Domain.Requests.Auth;
using PowerUp.Domain.Responses.Auth;
using PowerUp.Shared.Hash;
using AuthResponse = PowerUp.Domain.Responses.Auth.AuthResponse;
using RegisterUserRequest = PowerUp.Domain.Requests.Auth.RegisterUserRequest;

namespace PowerUp.Api.Services.Auth;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenService _jwtTokenService;

    public AuthService(
        IUserRepository userRepository,
        JwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> Login(
        LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmail(
            request.Email,
            cancellationToken);

        if (user == null)
            throw new Exception("Invalid credentials");

        if (!HasherUtil.VerifyPassword(request.Password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        var token = _jwtTokenService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            User = user
        };
    }

    public async Task<AuthResponse> Register(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var exists = await _userRepository.IsEmailExist(
            request.Email,
            cancellationToken);

        if (exists)
            throw new Exception("Email already exists");

        var user = new User
        {
            Email = request.Email,
            NickName = request.NickName,
            DateOfBirth = request.DateOfBirth,
            Role = UserRole.User,
            PasswordHash = HasherUtil.HashPassword(request.Password)
        };

        _userRepository.Add(user);

        var token = _jwtTokenService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            User = user
        };
    }
}
