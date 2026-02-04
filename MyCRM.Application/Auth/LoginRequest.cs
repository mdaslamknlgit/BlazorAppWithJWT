namespace MyCRM.Application.Auth;

public sealed class LoginRequest
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;

    public int RoleId { get; set; } = 0;
}
