using Microsoft.EntityFrameworkCore;
using MyCRM.Application.Interfaces;
using MyCRM.Domain.Contacts;

namespace MyCRM.Infrastructure.Persistence;

public sealed class ContactRepository : IContactRepository
{
    private readonly MyCrmDbContext _dbContext;

    public ContactRepository(MyCrmDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Contact contact)
    {
        await _dbContext.Contacts.AddAsync(contact);
    }
    public async Task UpdateAsync(Contact contact)
    {
        await _dbContext.Contacts
            .Where(c => c.ContactId == contact.ContactId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(c => c.FirstName, contact.FirstName)
                .SetProperty(c => c.LastName, contact.LastName)
                .SetProperty(c => c.Email, contact.Email)
                .SetProperty(c => c.Phone, contact.Phone)
            );
    }


    public async Task<Contact?> GetByIdAsync(int contactId)
    {
        return await _dbContext.Contacts
            .FirstOrDefaultAsync(x => x.ContactId == contactId && !x.IsDeleted);
    }

    public async Task<IReadOnlyList<Contact>> GetAllAsync()
    {
        return await _dbContext.Contacts
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

   
}
