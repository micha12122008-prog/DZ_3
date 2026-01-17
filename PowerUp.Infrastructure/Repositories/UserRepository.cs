using Microsoft.EntityFrameworkCore;
using PowerUp.Domain.Abstarctions.Repositories;
using PowerUp.Domain.Models.Users;
using PowerUp.Infrastructure.Repositories.Base;

namespace PowerUp.Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(PowerUpContext context) : base(context)
    {
    }

    public Task<User?> GetByEmail(string email, CancellationToken ct)
    {
        return Set<User>()
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public Task<bool> IsEmailExist(string email, CancellationToken ct)
    {
        return Set<User>()
            .AnyAsync(u => u.Email == email.Trim(), ct);
    }

    public Task<List<User>> Search(
        string? email,
        string? nickName,
        CancellationToken ct)
    {
        return Set<User>()
            .Where(u =>
                (email == null || u.Email.Contains(email)) &&
                (nickName == null || u.NickName.Contains(nickName)))
            .ToListAsync(ct);
    }
}