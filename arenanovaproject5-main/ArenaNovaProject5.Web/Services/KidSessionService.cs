namespace ArenaNovaProject5.Web.Services;

public class KidSessionService
{
    public bool IsKidAuthenticated { get; private set; }
    public int? ActiveChildId { get; private set; }

    public void LoginDemo(int childId)
    {
        IsKidAuthenticated = true;
        ActiveChildId = childId;
    }

    public void Logout()
    {
        IsKidAuthenticated = false;
        ActiveChildId = null;
    }
}
