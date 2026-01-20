using PowerUp.Domain.Models.Users;

namespace PowerUp.Domain.Responses.Auth;

public class AuthResponse
{
    public required string Token { get; set; }
    public required User User { get; set; }
}