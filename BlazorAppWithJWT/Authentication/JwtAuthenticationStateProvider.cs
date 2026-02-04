using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlazorAppWithJWT.Authentication
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null || httpContext.User == null)
            {
                return Task.FromResult(
                    new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))
                );
            }

            // IMPORTANT:
            // Use the user created by Cookie Authentication
            return Task.FromResult(
                new AuthenticationState(httpContext.User)
            );
        }

        // OPTIONAL: keep only if you explicitly need to refresh UI
        public void NotifyUserChanged()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var user = httpContext?.User
                       ?? new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user))
            );
        }
    }
}
