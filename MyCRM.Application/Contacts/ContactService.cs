using MyCRM.Application.Interfaces;
using MyCRM.Domain.Contacts;

namespace MyCRM.Application.Contacts;

public sealed class ContactService
{
    private readonly IUnitOfWork _unitOfWork;

    public ContactService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateAsync(CreateContactRequest request)
    {
        ContactRules.Validate(request.FirstName, request.LastName);

        var contact = new Contact(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone);

        await _unitOfWork.Contacts.AddAsync(contact);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateAsync(UpdateContactRequest request)
    {
        ContactRules.Validate(request.FirstName, request.LastName);

        var contact = await _unitOfWork.Contacts.GetByIdAsync(request.ContactId);

        if (contact == null)
            throw new Exception("Contact not found");

        contact.Update(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone);

        await _unitOfWork.CommitAsync();
    }


    public async Task<IReadOnlyList<ContactDto>> GetAllAsync()
    {
        var contacts = await _unitOfWork.Contacts.GetAllAsync();

        return contacts
            .Select(c => new ContactDto
            {
                ContactId = c.ContactId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone
            })
            .ToList();
    }


    public async Task<ContactDto?> GetByIdAsync(int contactId)
    {
        var contact = await _unitOfWork.Contacts.GetByIdAsync(contactId);

        if (contact == null)
            return null;

        return new ContactDto
        {
            ContactId = contact.ContactId,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            Phone = contact.Phone
        };
    }





}
