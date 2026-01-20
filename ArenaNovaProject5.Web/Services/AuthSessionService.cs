using System.Security.Claims;

namespace ArenaNovaProject5.Web.Services;

public class AuthSessionService
{
    public string? IdToken { get; private set; }
    public string? RefreshToken { get; private set; }
    public string? Email { get; private set; }
    public string? Uid { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(IdToken);

    public void SetSession(string idToken, string? refreshToken, string? email, string? uid = null)
    {
        IdToken = idToken;
        RefreshToken = refreshToken;
        Email = email;
        Uid = uid;
    }

    public void Clear()
    {
        
        IdToken = null;
        RefreshToken = null;
        Email = null;
        Uid = null;
        
    }

    public void ClearSession() => Clear();


    public ClaimsPrincipal ToPrincipal()
    {
        if (!IsAuthenticated)
            return new ClaimsPrincipal(new ClaimsIdentity());

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, Email ?? "parent"),
            new(ClaimTypes.Email, Email ?? ""),
            new("uid", Uid ?? ""),
            new("sub", Uid ?? "")
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Firebase"));
    }
}
