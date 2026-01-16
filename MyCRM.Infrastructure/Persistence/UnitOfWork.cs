using MyCRM.Application.Interfaces;

namespace MyCRM.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly MyCrmDbContext _dbContext;

    public IUserRepository Users { get; }
    public IRefreshTokenRepository RefreshTokens { get; }
    public IContactRepository Contacts { get; }

    public UnitOfWork(
        MyCrmDbContext dbContext,
        IUserRepository users,
        IRefreshTokenRepository refreshTokens,
        IContactRepository contacts)
    {
        _dbContext = dbContext;
        Users = users;
        RefreshTokens = refreshTokens;
        Contacts = contacts;
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
