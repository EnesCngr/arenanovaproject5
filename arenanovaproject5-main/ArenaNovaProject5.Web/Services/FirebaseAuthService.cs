using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ArenaNovaProject5.Web.Services
{
    public class FirebaseAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FirebaseAuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string ApiKey => _configuration["Firebase:ApiKey"] ?? "YOUR_FIREBASE_API_KEY";

        public async Task<FirebaseAuthResponse?> SignUpAsync(string email, string password)
        {
            var request = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(
                    $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}",
                    content
                );

                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonSerializer.Deserialize<FirebaseAuthError>(responseJson);
                    throw new Exception(error?.Error?.Message ?? "Registration failed");
                }

                return JsonSerializer.Deserialize<FirebaseAuthResponse>(responseJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Firebase SignUp Error: {ex.Message}");
                return null;
            }
        }

        public async Task<FirebaseAuthResponse?> SignInAsync(string email, string password)
        {
            var request = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(
                    $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}",
                    content
                );

                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonSerializer.Deserialize<FirebaseAuthError>(responseJson);
                    throw new Exception(error?.Error?.Message ?? "Sign in failed");
                }

                return JsonSerializer.Deserialize<FirebaseAuthResponse>(responseJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Firebase SignIn Error: {ex.Message}");
                return null;
            }
        }

        public async Task<FirebaseAuthResponse?> RefreshTokenAsync(string refreshToken)
        {
            var request = new
            {
                grant_type = "refresh_token",
                refresh_token = refreshToken
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(
                    $"https://securetoken.googleapis.com/v1/token?key={ApiKey}",
                    content
                );

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<FirebaseAuthResponse>(responseJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Firebase RefreshToken Error: {ex.Message}");
                return null;
            }
        }
    }
}
