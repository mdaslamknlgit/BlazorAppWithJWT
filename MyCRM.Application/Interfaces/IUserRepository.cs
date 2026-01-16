namespace MyCRM.Application.Interfaces;

public interface IUserRepository
{
    int? ValidateCredentials(string username, string password);
}
