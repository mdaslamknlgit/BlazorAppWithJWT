using MyCRM.Application.Interfaces;

namespace MyCRM.Infrastructure.Persistence;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    public void Add(string tokenHash, int userId, DateTime expiresAt)
    {
        // DB write will be added later
    }

    public bool IsValid(string tokenHash)
    {
        return true;
    }

    public void Revoke(string tokenHash)
    {
        // DB update will be added later
    }
}
