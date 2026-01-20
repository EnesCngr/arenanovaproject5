using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace ArenaNovaProject5.Web.Services
{
    public class FirestoreRestService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FirestoreRestService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string ProjectId => _configuration["Firebase:ProjectId"] ?? "project5-arenanova";

        public async Task<bool> SetUserAsync(string uid, UserModel user, string idToken)
        {
            try
            {
                Console.WriteLine($"[FirestoreRestService] Starting SetUserAsync for UID: {uid}");
                
                // Firestore REST API endpoint
                var url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/users/{uid}";

                var document = new
                {
                    fields = new
                    {
                        uid = new { stringValue = user.Uid },
                        email = new { stringValue = user.Email },
                        fullName = new { stringValue = user.FullName },
                        role = new { stringValue = user.Role },
                        createdAt = new { timestampValue = user.CreatedAt.ToUniversalTime().ToString("o") }
                    }
                };

                var json = JsonSerializer.Serialize(document);
                Console.WriteLine($"[FirestoreRestService] URL: {url}");
                Console.WriteLine($"[FirestoreRestService] Payload: {json}");
                Console.WriteLine($"[FirestoreRestService] IdToken exists: {!string.IsNullOrEmpty(idToken)}");

                var request = new HttpRequestMessage(HttpMethod.Patch, url)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                // Add authorization header with Bearer token
                if (!string.IsNullOrEmpty(idToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", idToken);
                    Console.WriteLine($"[FirestoreRestService] Authorization header added");
                }
                else
                {
                    Console.WriteLine($"[FirestoreRestService] WARNING: No IdToken provided!");
                }

                Console.WriteLine($"[FirestoreRestService] Sending request...");
                var response = await _httpClient.SendAsync(request);
                
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[FirestoreRestService] Response Status: {response.StatusCode}");
                Console.WriteLine($"[FirestoreRestService] Response Body: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[FirestoreRestService] SUCCESS! User document created.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"[FirestoreRestService] FAILED! Status: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FirestoreRestService] EXCEPTION: {ex.Message}");
                Console.WriteLine($"[FirestoreRestService] StackTrace: {ex.StackTrace}");
                return false;
            }
        }
    }
}
