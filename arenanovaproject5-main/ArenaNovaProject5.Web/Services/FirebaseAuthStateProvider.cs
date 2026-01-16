using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ArenaNovaProject5.Web.Services
{
    public class FirebaseAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthSessionService _session;

        public FirebaseAuthStateProvider(AuthSessionService session)
        {
            _session = session;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = _session.ToPrincipal();
            return Task.FromResult(new AuthenticationState(user));
        }

        public void NotifyAuthChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}