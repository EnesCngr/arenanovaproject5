using System;
using System.Threading.Tasks;

namespace ArenaNovaProject5.Web.models
{
    public class ChildAccountModel
    {
        public string? Childid { get; set; }
        public string? FullName { get; set; }
        private string? password { get; set; }
        public int renzolvl { get; set; }
    }
}