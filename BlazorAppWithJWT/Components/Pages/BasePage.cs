using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

public abstract class BasePage : ComponentBase, IDisposable
{
    [Inject] protected PersistentComponentState ComponentState { get; set; } = default!;

    private PersistingComponentStateSubscription? _subscription;

    protected override void OnInitialized()
    {
        // Register persistence callback
        _subscription = ComponentState.RegisterOnPersisting(OnPersistingAsync);

        base.OnInitialized();
    }

    // This is the ONLY place PersistAsJson is allowed
    protected abstract Task OnPersistingAsync();

    public void Dispose()
    {
        _subscription?.Dispose();
    }
}
