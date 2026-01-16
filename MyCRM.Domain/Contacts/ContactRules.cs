namespace MyCRM.Domain.Contacts;

public static class ContactRules
{
    public static void Validate(
        string firstName,
        string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required");
    }
}
