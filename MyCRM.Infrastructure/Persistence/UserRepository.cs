using Microsoft.EntityFrameworkCore;
using MyCRM.Application.DTOs;
using MyCRM.Application.Interfaces;

namespace MyCRM.Infrastructure.Persistence;

public sealed class UserRepository : IUserRepository
{
    private readonly MyCrmDbContext _dbContext;

    public UserRepository(MyCrmDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public AuthenticatedUserDto? ValidateCredentials(string username, string password)
    {
        var user = _dbContext.Users.SingleOrDefault(u => u.UserName == username && u.PasswordHash == password);

        if (user == null)
            return null;

        return new AuthenticatedUserDto
        {
            UserId = user.UserId,
            UserName = user.UserName,
            RoleId = user.RoleId
        };
    }


}
