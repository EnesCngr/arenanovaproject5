namespace ArenaNovaProject5.Web.models;

public class ChildLink
{
    public string Id { get; set; } = string.Empty;
    public string? childUid { get; set; }
    public string? fullName { get; set; }
    public bool active { get; set; }
    public int age { get; set; }
    public DateTime createdAt { get; set; }

    // Computed properties for UI compatibility
    public string? ChildUid => childUid;
    public string? DisplayName => fullName;
    public string? FullName => fullName;
    public string? Nickname => fullName; // Alias for compatibility
}
