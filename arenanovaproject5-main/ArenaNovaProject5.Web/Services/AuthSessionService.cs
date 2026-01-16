using System;
using System.Threading.Tasks;

namespace ArenaNovaProject5.Web.Services
{
    public class AuthSessionService
    {
        public string? CurrentUid { get; private set; }
        public string? UserId => CurrentUid; // Alias for Dashboard compatibility
        public string? IdToken { get; private set; }
        public string? RefreshToken { get; private set; }
        public string? Email { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(CurrentUid);

        public event Action? OnChange;

        public void SetUid(string? uid)
        {
            CurrentUid = uid;
            NotifyStateChanged();
        }

        public void SetSession(string? idToken, string? refreshToken, string? email, string? uid)
        {
            IdToken = idToken;
            RefreshToken = refreshToken;
            Email = email;
            CurrentUid = uid;
            NotifyStateChanged();
        }

        public void Clear()
        {
            CurrentUid = null;
            IdToken = null;
            RefreshToken = null;
            Email = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public System.Security.Claims.ClaimsPrincipal ToPrincipal()
        {
            if (string.IsNullOrEmpty(CurrentUid))
            {
                return new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity());
            }

            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, CurrentUid)
            };

            if (!string.IsNullOrEmpty(Email))
            {
                claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, Email));
            }

            var identity = new System.Security.Claims.ClaimsIdentity(claims, "Firebase");
            return new System.Security.Claims.ClaimsPrincipal(identity);
        }
    }
}