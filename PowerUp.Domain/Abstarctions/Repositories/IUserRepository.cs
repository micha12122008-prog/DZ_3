using PowerUp.Domain.Models.Users;

namespace PowerUp.Domain.Abstarctions.Repositories;

public interface IUserRepository
{
    ValueTask<User?> GetById(int id, CancellationToken cancellationToken);
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
    Task<bool> IsEmailExist(string email, CancellationToken cancellationToken);

    Task<List<User>> Search(
        string? email,
        string? nickName,
        CancellationToken cancellationToken);

    void Add(User user);
    void Update(User user);
    void Delete(User user);
}