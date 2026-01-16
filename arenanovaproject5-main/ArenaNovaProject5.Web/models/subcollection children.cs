using System;
using System.Threading.Tasks;

namespace ArenaNovaProject5.Web.models
{
    public class ChildSubcollection
    {
        public string? id { get; set; } // Document ID from Firestore
        public string? ChildUid { get; set; }
        public string? FullName { get; set; }
        public object? JoinedAt { get; set; }
        public string? Active { get; set; }
        public int Age { get; set; }
    }
}