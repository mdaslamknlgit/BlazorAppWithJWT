namespace MyCRM.Application.Contacts;

public sealed class ContactDto
{
    public int ContactId { get; init; }
    public string? FirstName { get; init; } = string.Empty;
    public string? LastName { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Phone { get; init; }
}
