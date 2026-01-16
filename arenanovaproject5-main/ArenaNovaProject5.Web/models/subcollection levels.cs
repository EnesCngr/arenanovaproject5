using System;
using System.Text.Json.Serialization;

namespace ArenaNovaProject5.Web.models
{
    public class LevelSubcollection
    {
        public string? id { get; set; }
        
        [JsonPropertyName("completed at")]
        public object? CompletedAt { get; set; }
        
        [JsonPropertyName("level completed")]
        public bool LevelCompleted { get; set; }

        [JsonPropertyName("quiz score")]
        public int QuizScore { get; set; }
        
        [JsonPropertyName("level number")]
        public int LevelNumber { get; set; }
        
        public int duration { get; set; }
    }
}