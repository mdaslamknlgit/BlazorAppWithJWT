using BlazorAppWithJWT.Authentication;
using BlazorAppWithJWT.Components;
using BlazorAppWithJWT.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MyBlazorApp.Http;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// =============================
// Services
// =============================

// Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpContext access
builder.Services.AddHttpContextAccessor();

// HttpClient + DelegatingHandler
builder.Services.AddTransient<ApiAuthorizationHandler>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7273/");
})
.AddHttpMessageHandler<ApiAuthorizationHandler>();

// Authentication & Authorization
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/logina";
        options.AccessDeniedPath = "/logina";
    });

builder.Services.AddAuthorization();
builder.Services.AddAuthorizationCore();

// Blazor auth state
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

// Controllers (AuthLoginController)
builder.Services.AddControllers();


builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<ContactsState>();


// =============================
// Build
// =============================
var app = builder.Build();


// =============================
// Middleware pipeline
// =============================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// 🔑 REQUIRED for cookie auth
app.UseAuthentication();
app.UseAuthorization();

// MVC controllers
app.MapControllers();

// Blazor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
