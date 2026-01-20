using System.Text;
using System.Text.Json;

namespace ArenaNovaProject5.Web.Services
{
    public class UserCreationService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public UserCreationService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<bool> CreateUserDocumentAsync(string uid, string email, string fullName, string idToken)
        {
            try
            {
                Console.WriteLine($"[UserCreationService] Starting CreateUserDocumentAsync for UID: {uid}");
                
                var projectId = _configuration["Firebase:ProjectId"] ?? "project5-arenanova";
                var url = $"https://firestore.googleapis.com/v1/projects/{projectId}/databases/(default)/documents/users/{uid}";

                var document = new
                {
                    fields = new
                    {
                        uid = new { stringValue = uid },
                        email = new { stringValue = email },
                        fullName = new { stringValue = fullName },
                        role = new { stringValue = "parent" },
                        createdAt = new { timestampValue = DateTime.UtcNow.ToString("o") }
                    }
                };

                var json = JsonSerializer.Serialize(document);
                Console.WriteLine($"[UserCreationService] Request URL: {url}");
                Console.WriteLine($"[UserCreationService] Request Body: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var httpRequest = new HttpRequestMessage(HttpMethod.Patch, url)
                {
                    Content = content
                };

                // Add authorization header
                httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);
                Console.WriteLine($"[UserCreationService] Authorization header added");

                Console.WriteLine($"[UserCreationService] Sending request...");
                var response = await _httpClient.SendAsync(httpRequest);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[UserCreationService] Response Status: {response.StatusCode}");
                Console.WriteLine($"[UserCreationService] Response Body: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[UserCreationService] SUCCESS! User document created");
                    return true;
                }
                else
                {
                    Console.WriteLine($"[UserCreationService] FAILED! Status code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserCreationService] EXCEPTION: {ex.Message}");
                Console.WriteLine($"[UserCreationService] StackTrace: {ex.StackTrace}");
                return false;
            }
        }
    }
}
