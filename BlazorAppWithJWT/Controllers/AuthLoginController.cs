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

        [HttpPost("login_old")]
        [IgnoreAntiforgeryToken]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Login_Old([FromForm] string username, [FromForm] string password, [FromForm] string? returnUrl)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsJsonAsync(
                "https://localhost:7273/api/auth/login",
                new { username, password });

            if (!response.IsSuccessStatusCode)
            {
                return LocalRedirect("/logina");
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            // 1. Create claims (minimum required)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

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

            //var returnUrl = Navigation.ToBaseRelativePath(Navigation.Uri);

            //Navigation.NavigateTo($"/auth/login?returnUrl=/{returnUrl}", true);

            // 4. Redirect to home
            returnUrl ??= "/";
            return LocalRedirect(returnUrl);
            //return LocalRedirect("/");
        }

        [HttpPost("login")]
        [IgnoreAntiforgeryToken]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Login([FromForm] string username,[FromForm] string password,[FromForm] string? returnUrl)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsJsonAsync(
                "https://localhost:7273/api/auth/login",
                new { username, password });

            if (!response.IsSuccessStatusCode)
            {
                return LocalRedirect("/logina");
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            // IMPORTANT: use username from API/token, not from UI
            var userNameFromApi = username;
            // if your LoginResponse has UserName, use:
            // var userNameFromApi = result.UserName;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userNameFromApi)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true
                   
                });

            // Store JWT only for API calls
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

            returnUrl ??= "/";
            return LocalRedirect(returnUrl);
        }


        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete("auth_token");

            return LocalRedirect("/logina");
        }

        private class LoginResponse
        {
            public string AccessToken { get; set; }
        }
    }
}
