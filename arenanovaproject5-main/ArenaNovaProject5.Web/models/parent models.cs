using System;
using System.Collections.Generic;

namespace ArenaNovaProject5.Web.models
{
    public class ParentModel
    {
        public string? Uid { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ChildSubcollection> Children { get; set; } = new();
    }
}