using System;
using System.Threading.Tasks;

namespace ArenaNovaProject5.Web.models
{
    public class ChildProgressModel
    {
        public string ChildUid { get; set; }
        public string FullName { get; set; }
        public object LastPlayed { get; set; }
        public string parentuid { get; set; }
        public string status { get; set; }
        public int TotalLevelsCompleted { get; set; }
    }
}