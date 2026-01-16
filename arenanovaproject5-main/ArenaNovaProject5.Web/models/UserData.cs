namespace ArenaNovaProject5.Web.models;

public class UserData
{
    public string uid { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string fullName { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
    public DateTime createdAt { get; set; }
    
    // Computed properties for UI compatibility
    public string Uid => uid;
    public string Email => email;
    public string FullName => fullName;
    public string Role => role;
    public DateTime CreatedAt => createdAt;
}
