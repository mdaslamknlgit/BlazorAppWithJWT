using MyCRM.Application.Interfaces;

namespace MyCRM.Infrastructure.Persistence;

public sealed class UserRepository : IUserRepository
{
    public int? ValidateCredentials(string username, string password)
    {
        return 1;
    }
}
