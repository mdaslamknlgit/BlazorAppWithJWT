using MyCRM.Domain.Contacts;

namespace MyCRM.Application.Interfaces;

public interface IContactRepository
{
    Task AddAsync(Contact contact);

    Task UpdateAsync(Contact contact);
    Task<Contact?> GetByIdAsync(int contactId);
    Task<IReadOnlyList<Contact>> GetAllAsync();
}
