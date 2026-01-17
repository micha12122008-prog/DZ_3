using Microsoft.AspNetCore.Identity;
using PowerUp.Domain.Abstarctions;
using PowerUp.Domain.Abstarctions.Repositories;
using PowerUp.Domain.Enums;
using PowerUp.Domain.Models.Users;

namespace PowerUp.Application.Services.Users;

public class UsersService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UsersService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<int> Register(
        string email,
        string password,
        string nickName,
        DateTime dateOfBirth,
        CancellationToken ct)
    {
        if (await _userRepository.IsEmailExist(email, ct))
            throw new Exception("Email already exists");

        var user = new User
        {
            Email = email,
            NickName = nickName,
            DateOfBirth = dateOfBirth,
            Role = UserRole.User
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(ct);

        return user.Id;
    }
    
    public async Task<User> Login(
        string email,
        string password,
        CancellationToken ct)
    {
        var user = await _userRepository.GetByEmail(email, ct)
            ?? throw new Exception("User not found");

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            password);

        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Invalid password");

        return user;
    }
    
    public async Task<User?> GetById(int id, CancellationToken ct)
    {
        return await _userRepository.GetById(id, ct);
    }
    
    public Task<List<User>> Search(
        string? email,
        string? nickName,
        CancellationToken ct)
    {
        return _userRepository.Search(email, nickName, ct);
    }
    
    public async Task Update(
        int id,
        string nickName,
        DateTime dateOfBirth,
        UserRole role,
        CancellationToken ct)
    {
        var user = await _userRepository.GetById(id, ct)
            ?? throw new Exception("User not found");

        user.NickName = nickName;
        user.DateOfBirth = dateOfBirth;
        user.Role = role;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(ct);
    }
    
    public async Task Delete(int id, CancellationToken ct)
    {
        var user = await _userRepository.GetById(id, ct)
            ?? throw new Exception("User not found");

        _userRepository.Delete(user);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
