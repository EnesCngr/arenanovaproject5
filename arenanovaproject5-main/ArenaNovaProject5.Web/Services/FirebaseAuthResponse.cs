using System.Text.Json.Serialization;

namespace ArenaNovaProject5.Web.Services
{
    public class FirebaseAuthResponse
    {
        [JsonPropertyName("idToken")]
        public string IdToken { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("expiresIn")]
        public string ExpiresIn { get; set; } = string.Empty;

        [JsonPropertyName("localId")]
        public string LocalId { get; set; } = string.Empty;

        [JsonPropertyName("registered")]
        public bool Registered { get; set; }
    }
}
