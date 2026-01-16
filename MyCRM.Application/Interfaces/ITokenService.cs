using MyCRM.Infrastructure.Auth;

namespace MyCRM.Application.Interfaces;

public interface ITokenService
{
    TokenResult CreateTokens(int userId, string username);
}
