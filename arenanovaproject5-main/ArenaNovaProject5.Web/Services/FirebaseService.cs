using Microsoft.JSInterop;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace ArenaNovaProject5.Web.Services
{
    public class FirebaseService
    {
        private readonly IJSRuntime _js;
        private readonly IMemoryCache _cache;

        public FirebaseService(IJSRuntime js, IMemoryCache cache)
        {
            _js = js;
            _cache = cache;
        }

        // Login
        public async Task<object?> LoginAsync(string email, string password)
        {
            return await _js.InvokeAsync<object>("firebaseSignIn", "auth", email, password);
        }

        // Logout
        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("firebaseSignOut", "auth");
        }

        // Get user data with caching
        public async Task<object?> GetUserDataAsync(string uid)
        {
            if (_cache.TryGetValue($"user_{uid}", out var userData))
            {
                return userData;
            }

            var result = await _js.InvokeAsync<object>("getUserData", uid);
            if (result != null)
            {
                _cache.Set($"user_{uid}", result, TimeSpan.FromMinutes(30));
            }
            return result;
        }

        // Get user and child progress with caching
        public async Task<object?> GetUserAndChildProgressAsync(string userUid, string childUid)
        {
            string cacheKey = $"progress_{userUid}_{childUid}";
            if (_cache.TryGetValue(cacheKey, out var progressData))
            {
                return progressData;
            }

            var result = await _js.InvokeAsync<object>("getUserAndChildProgress", userUid, childUid);
            if (result != null)
            {
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));
            }
            return result;
        }

        // Get subcollection with caching
        public async Task<object?> GetSubcollectionAsync(string parentCollection, string docId, string subcollectionName)
        {
            string cacheKey = $"subcol_{parentCollection}_{docId}_{subcollectionName}";
            if (_cache.TryGetValue(cacheKey, out var subcolData))
            {
                return subcolData;
            }

            var result = await _js.InvokeAsync<object>("getSubcollection", parentCollection, docId, subcollectionName);
            if (result != null)
            {
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));
            }
            return result;
        }

          public async Task<object?> AddChildAccountAsync(object data)
        {
            return await _js.InvokeAsync<object>("firebasestoreAddChildAccount", data);
        }

        // Update user data
        public async Task UpdateUserDataAsync(string uid, object data)
        {
            await _js.InvokeVoidAsync("firebasestoreUpdateUserData", uid, data);
        }

        // Update child progress
        public async Task UpdateChildProgressAsync(string childUid, object data)
        {
            await _js.InvokeVoidAsync("firebasestoreUpdateChildProgress", childUid, data);
        }

        // Delete child account
        public async Task DeleteChildAccountAsync(string childUid)
        {
            await _js.InvokeVoidAsync("firebasestoreDeleteChildAccount", childUid);
        }

        // Listen for child accounts changes
        public async Task OnChildAccountsChangedAsync(string userUid, DotNetObjectReference<object> dotNetHelper)
        {
            await _js.InvokeVoidAsync("firebasestoreOnChildAccountsChanged", userUid, dotNetHelper);
        }

        // Listen for child progress changes
        public async Task OnChildProgressChangedAsync(string childUid, DotNetObjectReference<object> dotNetHelper)
        {
            await _js.InvokeVoidAsync("firebasestoreOnChildProgressChanged", childUid, dotNetHelper);
        }

        // Count subcollections
        public async Task<int> CountSubcollectionsAsync(string docPath)
        {
            return await _js.InvokeAsync<int>("countsubcollections", docPath);
        }

        // List subcollection names
        public async Task<string[]?> ListSubcollectionNamesAsync(string docPath)
        {
            return await _js.InvokeAsync<string[]>("listSubcollectionNames", docPath);
        }

        // Local storage helpers
        public async Task StoreUidInLocalStorageAsync(string uid)
        {
            await _js.InvokeVoidAsync("storeuidinlocalstorage", uid);
        }

        public async Task<string?> GetUidFromLocalStorageAsync()
        {
            return await _js.InvokeAsync<string>("getuidfromlocalstorage");
        }

        public async Task RemoveUidFromLocalStorageAsync()
        {
            await _js.InvokeVoidAsync("removeuidfromlocalstorage");
        }

        public async Task ClearLocalStorageAsync()
        {
            await _js.InvokeVoidAsync("clearlocalstorage");
        }

        public async Task StoreChildUidInLocalStorageAsync(string childUid)
        {
            await _js.InvokeVoidAsync("storechilduidinlocalstorage", childUid);
        }

        public async Task<string?> GetChildUidFromLocalStorageAsync()
        {
            return await _js.InvokeAsync<string>("getchilduidfromlocalstorage");
        }

        public async Task RemoveChildUidFromLocalStorageAsync()
        {
            await _js.InvokeVoidAsync("removechilduidfromlocalstorage");
        }

        // Query all collections snapshot
        public async Task<object?> QuerySnapshotAsync()
        {
            return await _js.InvokeAsync<object>("querySnapshot");
        }

        public async Task<string?> GetCurrentUidAsync()
        {
             return await _js.InvokeAsync<string>("getCurrentUid", "auth");
        }

        // Generic GetDocumentAsync method
        public async Task<T?> GetDocumentAsync<T>(string collection, string documentId) where T : class
        {
            try
            {
                var result = await _js.InvokeAsync<System.Text.Json.JsonElement>("firebaseGetDocument", collection, documentId);
                if (result.ValueKind == System.Text.Json.JsonValueKind.Null || result.ValueKind == System.Text.Json.JsonValueKind.Undefined)
                {
                    return null;
                }

                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                return System.Text.Json.JsonSerializer.Deserialize<T>(result.GetRawText(), options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting document {collection}/{documentId}: {ex.Message}");
                return null;
            }
        }

        // Generic GetSubcollectionAsync method
        public async Task<List<T>?> GetSubcollectionAsync<T>(string parentCollection, string docId, string subcollectionName) where T : class
        {
            try
            {
                var result = await _js.InvokeAsync<System.Text.Json.JsonElement>("firebaseGetSubcollection", parentCollection, docId, subcollectionName);
                if (result.ValueKind == System.Text.Json.JsonValueKind.Null || result.ValueKind == System.Text.Json.JsonValueKind.Undefined)
                {
                    return null;
                }

                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                return System.Text.Json.JsonSerializer.Deserialize<List<T>>(result.GetRawText(), options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting subcollection {parentCollection}/{docId}/{subcollectionName}: {ex.Message}");
                return null;
            }
        }

        // Set document
        public async Task<FirebaseResult> SetDocumentAsync(string collection, string documentId, object data)
        {
            try
            {
                await _js.InvokeVoidAsync("firebaseSetDocument", collection, documentId, data);
                return new FirebaseResult { Success = true };
            }
            catch (Exception ex)
            {
                return new FirebaseResult { Success = false, Error = ex.Message };
            }
        }

        // Get collection
        public async Task<List<T>?> GetCollectionAsync<T>(string collection) where T : class
        {
            try
            {
                var result = await _js.InvokeAsync<object>("firebaseGetCollection", collection);
                if (result == null) return null;

                var json = result.ToString();
                return json != null ? System.Text.Json.JsonSerializer.Deserialize<List<T>>(json) : null;
            }
            catch
            {
                return null;
            }
        }

        // Get current user
        public async Task<FirebaseUser?> GetCurrentUserAsync()
        {
            try
            {
                var uid = await GetCurrentUidAsync();
                if (string.IsNullOrEmpty(uid)) return null;

                return new FirebaseUser
                {
                    UserId = uid,
                    Email = string.Empty
                };
            }
            catch
            {
                return null;
            }
        }

        // Sign in
        public async Task<FirebaseResult> SignInAsync(string email, string password)
        {
            try
            {
                var result = await LoginAsync(email, password);
                if (result != null)
                {
                    return new FirebaseResult { Success = true, UserId = result.ToString() };
                }
                return new FirebaseResult { Success = false, Error = "Login failed" };
            }
            catch (Exception ex)
            {
                return new FirebaseResult { Success = false, Error = ex.Message };
            }
        }

        // Sign up - returns detailed auth result
        public async Task<FirebaseAuthResult?> SignUpAsync(string email, string password)
        {
            try
            {
                var result = await _js.InvokeAsync<FirebaseAuthResult>("firebaseSignUp", "auth", email, password);
                return result;
            }
            catch
            {
                return null;
            }
        }

        // Sign up with result wrapper (for compatibility)
        public async Task<FirebaseResult> SignUpWithResultAsync(string email, string password)
        {
            try
            {
                var authResult = await SignUpAsync(email, password);
                if (authResult != null && !string.IsNullOrEmpty(authResult.LocalId))
                {
                    return new FirebaseResult 
                    { 
                        Success = true, 
                        UserId = authResult.LocalId 
                    };
                }
                return new FirebaseResult { Success = false, Error = "Sign up failed" };
            }
            catch (Exception ex)
            {
                return new FirebaseResult { Success = false, Error = ex.Message };
            }
        }

        // Sign out
        public async Task<FirebaseResult> SignOutAsync()
        {
            try
            {
                await LogoutAsync();
                return new FirebaseResult { Success = true };
            }
            catch (Exception ex)
            {
                return new FirebaseResult { Success = false, Error = ex.Message };
            }
        }

    }

    public class FirebaseUser
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
    }

    public class FirebaseResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? UserId { get; set; }
    }

    public class FirebaseAuthResult
    {
        public string LocalId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string IdToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

}