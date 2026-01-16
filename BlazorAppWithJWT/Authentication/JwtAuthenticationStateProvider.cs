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

            if (httpContext == null)
            {
                return Task.FromResult(
                    new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))
                );
            }

            var token = httpContext.Request.Cookies["auth_token"];

            if (string.IsNullOrWhiteSpace(token))
            {
                return Task.FromResult(
                    new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))
                );
            }

            // Minimal identity (token validation is API’s job)
            var identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Name, "AuthenticatedUser")
                },
                authenticationType: "jwt"
            );

            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }

        // Called after successful login
        public void MarkUserAsAuthenticated()
        {
            var identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Name, "AuthenticatedUser")
                },
                authenticationType: "jwt"
            );

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user))
            );
        }

        // Called on logout
        public void MarkUserAsLoggedOut()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(anonymous))
            );
        }
    }
}
