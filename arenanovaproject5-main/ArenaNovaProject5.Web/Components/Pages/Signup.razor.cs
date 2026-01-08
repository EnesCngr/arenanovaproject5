using Microsoft.AspNetCore.Components;
using ArenaNovaProject5.Web.Services;
using Microsoft.JSInterop;

namespace ArenaNovaProject5.Web.Components.Pages
{
    public partial class Signup
    {
        [Inject] private FirebaseAuthService AuthService { get; set; } = default!;
        [Inject] private FirebaseService FirebaseService { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private string fullName = string.Empty;
        private string role = string.Empty;
        private string email = string.Empty;
        private string password = string.Empty;
        private string message = string.Empty;
        private bool isSuccess = false;
        private bool isLoading = false;

        private async Task HandleSignUp()
        {
            message = string.Empty;
            isSuccess = false;
            isLoading = true;

            try
            {
                await JS.InvokeVoidAsync("console.log", "Starting registration...");

                if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || 
                    string.IsNullOrWhiteSpace(password))
                {
                    message = "Please fill in all fields.";
                    return;
                }

                if (password.Length < 6)
                {
                    message = "Password must be at least 6 characters.";
                    return;
                }

                await JS.InvokeVoidAsync("console.log", $"Creating user: {email}");

                // Create Firebase Auth account (C# HTTP call)
                var authResult = await AuthService.SignUpAsync(email, password);

                if (authResult == null || string.IsNullOrEmpty(authResult.LocalId))
                {
                    message = "Registration failed. Please try again.";
                    await JS.InvokeVoidAsync("console.error", "Auth failed");
                    return;
                }

                await JS.InvokeVoidAsync("console.log", $"Auth successful! UID: {authResult.LocalId}");

                // Save user data to Firestore (including fullName and role from form)
                var userData = new
                {
                    uid = authResult.LocalId,
                    email = authResult.Email,
                    fullName = fullName,
                    role = "parent",
                    createdAt = DateTime.UtcNow.ToString("o")
                };

                await JS.InvokeVoidAsync("console.log", "Saving to Firestore...");

                var firestoreResult = await FirebaseService.SetDocumentAsync("users", authResult.LocalId, userData);

                if (firestoreResult.Success)
                {
                    isSuccess = true;
                    message = $"Registration Successful! Welcome, {fullName}!";
                    await JS.InvokeVoidAsync("console.log", "User saved to Firestore!");
                    
                    // Clear form
                    fullName = string.Empty;
                    email = string.Empty;
                    password = string.Empty;
                }
                else
                {
                    message = $"Auth created but Firestore failed: {firestoreResult.Error}";
                    await JS.InvokeVoidAsync("console.error", $"Firestore error: {firestoreResult.Error}");
                }
            }
            catch (Exception ex)
            {
                message = $"Error: {ex.Message}";
                await JS.InvokeVoidAsync("console.error", $"Exception: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}