namespace ArenaNovaProject5.Web.models;

public class LevelRecord
{
    public string Id { get; set; } = string.Empty;
    public int levelNumber { get; set; }
    public bool levelCompleted { get; set; }
    public int quizScore { get; set; }
    
    // completedAt field - Firestore Timestamp or string
    public object? completedAt { get; set; }
    
    // Computed properties for UI compatibility
    public int LevelNumber => levelNumber;
    public int Score => quizScore;
    public int StudyTime { get; set; } // Not in schema, default to 0

    public DateTime ResolvedDate 
    {
        get 
        {
            if (completedAt == null) return DateTime.Now;
            
            // Try as Firestore Timestamp (object with seconds/nanoseconds)
            if (completedAt is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("seconds", out var secondsProp) && 
                    secondsProp.TryGetInt64(out var seconds))
                {
                    return DateTimeOffset.FromUnixTimeSeconds(seconds).LocalDateTime;
                }
            }
            
            // Try as string
            if (completedAt is string dateString && DateTime.TryParse(dateString, out var parsedDate))
            {
                return parsedDate;
            }
            
            return DateTime.Now;
        }
    }
}
