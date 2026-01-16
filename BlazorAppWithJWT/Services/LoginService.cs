using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace BlazorAppWithJWT.Services
{
    public class LoginService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var response = await client.PostAsJsonAsync(
                    "https://localhost:7273/api/auth/login",
                    new { username, password });

                if (!response.IsSuccessStatusCode)
                    return false;

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                _httpContextAccessor.HttpContext.Response.Cookies.Append(
                    "auth_token",
                    result.AccessToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Path = "/"
                    });

                return true;
            }
            catch (Exception ex)
            {
                var ErrorMsg=ex.ToString();
                return false;
            }
        }

        private class LoginResponse
        {
            public string AccessToken { get; set; }
        }
    }
}
