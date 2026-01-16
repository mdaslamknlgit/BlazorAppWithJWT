using System.Net.Http.Json;

namespace BlazorAppWithJWT.Services
{
    public class SchemeService
    {
        private readonly HttpClient _http;

        public SchemeService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<List<string>> GetSchemesAsync()
        {
            return await _http.GetFromJsonAsync<List<string>>("api/schemes");
        }
    }
}
