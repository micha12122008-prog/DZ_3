namespace PowerUp.Domain.Requests.Auth;

public class RegisterUserRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string NickName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
}