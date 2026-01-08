using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using ArenaNovaProject5.Web.Services;

namespace ArenaNovaProject5.Web.Components.Pages
{
    public partial class Login
    {
        [Inject] private FirebaseAuthService FirebaseAuth { get; set; } = default!;
        [Inject] private AuthSessionService Session { get; set; } = default!;
        [Inject] private FirebaseAuthStateProvider AuthState { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;

        [SupplyParameterFromQuery]
        public string? ReturnUrl { get; set; }

        private LoginModel Model { get; set; } = new();
        private bool IsBusy { get; set; }
        private string? Error { get; set; }

        private async Task HandleLogin()
        {
            Error = null;
            IsBusy = true;

            try
            {
                var res = await FirebaseAuth.SignInAsync(Model.Email, Model.Password);

                if (res is null || string.IsNullOrWhiteSpace(res.IdToken))
                {
                    Error = "Login failed. Please check your credentials.";
                    return;
                }

                // Store auth session
                Session.SetSession(res.IdToken, res.RefreshToken, res.Email ?? Model.Email);

                AuthState.NotifyAuthChanged();

                var target = string.IsNullOrWhiteSpace(ReturnUrl) ? "/dashboard" : "/" + ReturnUrl.TrimStart('/');
                Nav.NavigateTo(target);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public class LoginModel
        {
            [Required, EmailAddress]
            public string Email { get; set; } = "";

            [Required, MinLength(6)]
            public string Password { get; set; } = "";
        }
    }
}
