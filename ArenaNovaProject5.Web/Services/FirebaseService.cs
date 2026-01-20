using Microsoft.JSInterop;
using System.Text.Json;

namespace ArenaNovaProject5.Web.Services
{
    public class FirebaseService
    {
        private readonly IJSRuntime _jsRuntime;

        public FirebaseService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // ============= Authentication Methods =============

        public async Task<FirebaseAuthResult> SignInAsync(string email, string password)
        {
            var result = await _jsRuntime.InvokeAsync<FirebaseAuthResult>("firebaseSignIn", email, password);
            return result;
        }

        public async Task<FirebaseAuthResult> SignUpAsync(string email, string password)
        {
            var result = await _jsRuntime.InvokeAsync<FirebaseAuthResult>("firebaseSignUp", email, password);
            return result;
        }

        public async Task<FirebaseResult> SignOutAsync()
        {
            var result = await _jsRuntime.InvokeAsync<FirebaseResult>("firebaseSignOut");
            return result;
        }

        public async Task<FirebaseUser?> GetCurrentUserAsync()
        {
            var result = await _jsRuntime.InvokeAsync<FirebaseUser?>("firebaseGetCurrentUser");
            return result;
        }

        // ============= Firestore Methods =============

        public async Task<T?> GetDocumentAsync<T>(string collectionName, string documentId)
        {
            var result = await _jsRuntime.InvokeAsync<FirebaseDataResult>("firestoreGetDocument", collectionName, documentId);
            
            if (result.Success && !string.IsNullOrEmpty(result.Data))
            {
                return JsonSerializer.Deserialize<T>(result.Data);
            }
            
            return default;
        }

        public async Task<List<T>> GetCollectionAsync<T>(string collectionName)
        {
            var result = await _jsRuntime.InvokeAsync<FirebaseDataResult>("firestoreGetCollection", collectionName);
            
            if (result.Success && !string.IsNullOrEmpty(result.Data))
            {
                return JsonSerializer.Deserialize<List<T>>(result.Data) ?? new List<T>();
            }
            
            return new List<T>();
        }

        public async Task<FirebaseResult> SetDocumentAsync<T>(string collectionName, string documentId, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var result = await _jsRuntime.InvokeAsync<FirebaseResult>("firestoreSetDocument", collectionName, documentId, json);
            return result;
        }

        public async Task<FirebaseAddResult> AddDocumentAsync<T>(string collectionName, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var result = await _jsRuntime.InvokeAsync<FirebaseAddResult>("firestoreAddDocument", collectionName, json);
            return result;
        }

        public async Task<FirebaseResult> UpdateDocumentAsync<T>(string collectionName, string documentId, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var result = await _jsRuntime.InvokeAsync<FirebaseResult>("firestoreUpdateDocument", collectionName, documentId, json);
            return result;
        }

        public async Task<FirebaseResult> DeleteDocumentAsync(string collectionName, string documentId)
        {
            var result = await _jsRuntime.InvokeAsync<FirebaseResult>("firestoreDeleteDocument", collectionName, documentId);
            return result;
        }

        public async Task<List<T>> QueryCollectionAsync<T>(string collectionName, string field, string operatorStr, object value)
        {
            var result = await _jsRuntime.InvokeAsync<FirebaseDataResult>("firestoreQueryCollection", collectionName, field, operatorStr, value);
            
            if (result.Success && !string.IsNullOrEmpty(result.Data))
            {
                return JsonSerializer.Deserialize<List<T>>(result.Data) ?? new List<T>();
            }
            
            return new List<T>();
        }
    }

    // ============= Result Classes =============

    public class FirebaseResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    public class FirebaseAuthResult : FirebaseResult
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
    }

    public class FirebaseDataResult : FirebaseResult
    {
        public string? Data { get; set; }
    }

    public class FirebaseAddResult : FirebaseResult
    {
        public string? DocumentId { get; set; }
    }

    public class FirebaseUser
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
    }
}
