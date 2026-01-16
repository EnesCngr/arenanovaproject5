using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ArenaNovaProject5.Web.Services
{
    public class FirebaseAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public FirebaseAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = "AIzaSyDX_ypE-DlpBlpn-iSWOqre8Yn2v22_56Q";
        }

        public async Task<FirebaseAuthResponse?> SignInAsync(string email, string password)
        {
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_apiKey}";
            
            var payload = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<FirebaseAuthResponse>(responseContent);
            }

            return null;
        }

        public async Task<FirebaseAuthResponse?> SignUpAsync(string email, string password)
        {
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={_apiKey}";
            
            var payload = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<FirebaseAuthResponse>(responseContent);
            }

            return null;
        }
    }
}
