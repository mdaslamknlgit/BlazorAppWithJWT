namespace MyCRM.Domain.Contacts;

public sealed class Contact
{
    public int ContactId { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Contact() { }

    public Contact(
        string firstName,
        string lastName,
        string? email,
        string? phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void Update(
        string firstName,
        string lastName,
        string? email,
        string? phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
