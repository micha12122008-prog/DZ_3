using PowerUp.Domain.Enums;
using PowerUp.Domain.Models.Base;

namespace PowerUp.Domain.Models.Users;

public class User : BaseEntity
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string NickName { get; set; } = null!;
    public required DateTime DateOfBirth { get; set; }

    public UserRole Role { get; set; }
}