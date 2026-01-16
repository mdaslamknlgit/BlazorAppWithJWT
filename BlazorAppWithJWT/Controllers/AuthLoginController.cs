using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Security.Claims;

namespace BlazorAppWithJWT.Controllers
{
    [ApiController]
    [Route("auth")]
    [IgnoreAntiforgeryToken]
    public class AuthLoginController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthLoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromForm] string username,
            [FromForm] string password)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsJsonAsync(
                "https://localhost:7273/api/auth/login",
                new { username, password });

            if (!response.IsSuccessStatusCode)
            {
                return LocalRedirect("/login");
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            // 1. Create claims (minimum required)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            // 2. SIGN IN (this creates the auth cookie ASP.NET understands)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true
                });

            // 3. OPTIONAL: still store JWT for API calls
            Response.Cookies.Append(
                "auth_token",
                result.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/"
                });

            // 4. Redirect to home
            return LocalRedirect("/");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete("auth_token");

            return LocalRedirect("/login");
        }

        private class LoginResponse
        {
            public string AccessToken { get; set; }
        }
    }
}
