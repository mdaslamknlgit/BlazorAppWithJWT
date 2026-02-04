using MyCRM.Application.DTOs;

namespace MyCRM.Application.Interfaces;

public interface IUserRepository
{
    public AuthenticatedUserDto? ValidateCredentials(string username,string password);
}
