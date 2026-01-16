using System.Net.Http.Json;
using BlazorAppWithJWT.Models;

namespace BlazorAppWithJWT.Services
{
    public sealed class ContactService
    {
        private readonly HttpClient _http;

        public ContactService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<IReadOnlyList<ContactDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<IReadOnlyList<ContactDto>>(
                "api/contacts");
        }

        public async Task CreateAsync(ContactFormModel request)
        {
            await _http.PostAsJsonAsync("api/contacts", request);
        }

        public async Task UpdateAsync(int id, ContactFormModel request)
        {
            try
            {
                await _http.PutAsJsonAsync($"api/contacts", request);
            }
            catch (Exception ex)
            {
                var ErrorMsg = ex.ToString();
            }
        }

        public async Task<ContactDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ContactDto>($"api/contacts/{id}");
        }


    }
}
