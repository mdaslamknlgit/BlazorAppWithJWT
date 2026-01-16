using BlazorAppWithJWT.Models;
using BlazorAppWithJWT.Services;

public sealed class ContactsState
{
    private IReadOnlyList<ContactDto>? _contacts;
    private bool _loading;

    public IReadOnlyList<ContactDto>? Contacts => _contacts;

    public async Task LoadAsync(ContactService service)
    {
        if (_contacts != null || _loading)
            return;

        _loading = true;

        _contacts = await service.GetAllAsync();

        _loading = false;
    }
}
