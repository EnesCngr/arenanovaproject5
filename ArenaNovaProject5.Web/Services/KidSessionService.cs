namespace ArenaNovaProject5.Web.Services;

public class KidSessionService
{
    public bool IsKidAuthenticated { get; private set; }
    public int? ActiveChildId { get; private set; }
    public string? ActiveChildIdString { get; private set; }

    public void LoginDemo(int childId)
    {
        IsKidAuthenticated = true;
        ActiveChildId = childId;
        ActiveChildIdString = null;
    }

    public void LoginWithId(string childId)
    {
        IsKidAuthenticated = true;
        ActiveChildIdString = childId;
        ActiveChildId = null;
    }

    public void Logout()
    {
        IsKidAuthenticated = false;
        ActiveChildId = null;
        ActiveChildIdString = null;
    }
}
