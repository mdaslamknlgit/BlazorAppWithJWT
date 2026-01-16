namespace MyCRM.Application.Interfaces;

public interface IRefreshTokenRepository
{
    void Add(string tokenHash, int userId, DateTime expiresAt);
    bool IsValid(string tokenHash);
    void Revoke(string tokenHash);
}
