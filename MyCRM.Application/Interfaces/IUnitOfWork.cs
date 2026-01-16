namespace MyCRM.Application.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IContactRepository Contacts { get; }
    Task CommitAsync();
}
